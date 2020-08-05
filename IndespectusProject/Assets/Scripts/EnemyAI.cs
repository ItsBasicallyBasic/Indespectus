﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    // AI ESM Enumerator
    public enum EnemyBehaviours
    {
        Idle,
        Attacking,
        Fleeing,
        Scouting,
        Dead
    }

    public EnemyBehaviours enemyBehaviour;

    // Nav Mesh Agent
    private NavMeshAgent enemyNavMeshAgent;

    // Target GameObject
    public GameObject player;

    // Target's collider
    public Transform collider;

    // Target Location
    public Vector3 target;

    // Player's last known location
    private Vector3 playerLastKnownLocation;

    // Enemy scout variables
    public Transform scoutLocationsParent;
    public List<Transform> scoutLocations;
    public Transform nextLocation;
    public float locationTimer = 10;
    private float locationTime;

    // Flee variables
    public Transform fleeLocationsParent;
    public List<Transform> fleeLocations;
    private bool fleeLocationFound = false;

    // Enemy attack range
    public float attackDistance = 1;

    // Enemy run range
    public float runDistance = 5;

    // Enemy movement speeds
    public float runSpeed = 5;
    public float stealthSpeed = 1;

    // Rotation Variables
    private float rotationSpeed = 5.0f;
    private float adjRotSpeed;
    private Quaternion targetRotation;

    // Animation variables
    public bool attacking = false;

    // Player velocity variables
    private float playerVelocity;

    // Animation variables
    public Animator anim;

    // Attack timer variables
    public float attackTime;
    private float attackTimer;

    [SerializeField]
    private WeaponBehaviour weaponBehaviour;

    // Health components
    public PlayerResources enemyResources;

    // Retreat if Miss Vars
    public bool hitOrMiss =  false;
    public bool attackingMiss;
    private float missTimer;
    [SerializeField]
    private float missTime;

    [SerializeField]
    private AISpawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        if(scoutLocationsParent == null)
            scoutLocationsParent = GameObject.FindGameObjectWithTag("ScoutLocations").transform;
        if(fleeLocationsParent == null)
            fleeLocationsParent = GameObject.FindGameObjectWithTag("FleeLocations").transform;
        if(collider == null)
            collider = GameObject.FindGameObjectWithTag("MainCamera").transform;
        if(anim == null)
            anim = GetComponent<Animator>();
        if(spawner == null)
            spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<AISpawner>();

        
        enemyResources = GetComponent<PlayerResources>();
        enemyResources.SetHealth(100);
        enemyBehaviour = EnemyBehaviours.Scouting;

        // Get nav mesh agent from gameobject
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();

        // Add each scout point to scoutLocations list
        foreach(Transform child in scoutLocationsParent)
        {
            scoutLocations.Add(child);
        }

        // Add each flee point to fleeLocations list
        foreach (Transform child in fleeLocationsParent)
        {
            fleeLocations.Add(child);
        }

        // Assign first scout location
        int randomIndex = Random.Range(0, scoutLocations.Count);
        nextLocation = scoutLocations[randomIndex];

        //anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if(enemyResources.GetHealth() <= 0)
        {
            enemyBehaviour = EnemyBehaviours.Dead;
        }

        if (enemyResources.GetHealth() <= 10 && enemyResources.GetHealth() > 0 && enemyBehaviour == EnemyBehaviours.Attacking)
        {
            enemyBehaviour = EnemyBehaviours.Fleeing;
        }

        playerVelocity = player.GetComponent<PlayerVelocity>().velocity;

        if (!attacking)
        {
            // Look at target
            RotateTowardsTarget(new Vector3(target.x, transform.position.y, target.z));
        }

        if (playerVelocity > 0.5f && enemyBehaviour != EnemyBehaviours.Fleeing && enemyBehaviour != EnemyBehaviours.Dead && !weaponBehaviour.swordBroken)
        {
            // ...player location and AI location as Vector3s
            //Vector3 origin = transform.position;
            //Vector3 position = collider.position;

            // ...Calculate direction and distance from the AI to the location
            //Vector3 dir = position - origin;

            //RaycastHit hit;

            // If player can be seen...
            //if(Physics.Raycast(origin, dir, out hit) && hit.transform.gameObject.tag == "Player")
            //{
                // Update player last known location & attack
                playerLastKnownLocation = player.GetComponent<PlayerVelocity>().head.transform.position;
                enemyBehaviour = EnemyBehaviours.Attacking;
            //}
        }

        // Enemy Behaviours State Switch
        switch (enemyBehaviour)
        {
            case EnemyBehaviours.Scouting:
                Scouting();
                break;
            case EnemyBehaviours.Attacking:
                Attacking();
                break;
            case EnemyBehaviours.Fleeing:
                Fleeing();
                break;
            case EnemyBehaviours.Dead:
                Dead();
                break;
        }

        // check hit or miss
        if(attackingMiss) {
            if(Time.time > missTimer) {
                if(!hitOrMiss) {
                    enemyBehaviour = EnemyBehaviours.Fleeing;
                }
                hitOrMiss = false;
                attackingMiss = false;
            }
        }

    }

    void Scouting()
    {

        // INSERT WALK TRIGGER HERE
        anim.SetBool("isWalking", true);
        anim.SetBool("isRunning", false);

        // Setting position to scout toward
        target = nextLocation.position;

        // Enable NavMeshAgent
        enemyNavMeshAgent.isStopped = false;

        // Set NavMeshAgent Speed
        enemyNavMeshAgent.speed = stealthSpeed;

        // Scout location timer
        if(Time.time > locationTime || Vector3.Distance(nextLocation.position, transform.position) < 1f)
        {
            locationTime = Time.time + locationTimer;

            // Choose enemy scout location using random index
            int randomIndex = Random.Range(0, scoutLocations.Count);
            enemyNavMeshAgent.destination = scoutLocations[randomIndex].position;

            nextLocation = scoutLocations[randomIndex];
        }

        // If player becomes visible attack
    }

    void Attacking()
    {
        // Setting target to attack
        target = playerLastKnownLocation;

        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(playerLastKnownLocation.x, 0, playerLastKnownLocation.z)) <= attackDistance)
        {

            GetComponent<PlayerVelocity>().OverrideVelocity(1.5f);

            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);

            enemyNavMeshAgent.isStopped = true;

            if (Time.time > attackTimer)
            {
                attackTimer = Time.time + attackTime;

                anim.SetTrigger("isAttacking");
                
                // hit or miss
                attackingMiss = true;
                missTimer = Time.time + missTime;
            }

            if (weaponBehaviour.swordBroken)
            {
                enemyBehaviour = EnemyBehaviours.Fleeing;
            }

            return;
        }
        if(Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(playerLastKnownLocation.x, 0, playerLastKnownLocation.z)) > attackDistance && Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(playerLastKnownLocation.x, 0, playerLastKnownLocation.z)) > runDistance)
        {
            // Enable NavMeshAgent, set speed and set destination
            enemyNavMeshAgent.isStopped = false;
            enemyNavMeshAgent.speed = stealthSpeed;
            enemyNavMeshAgent.destination = playerLastKnownLocation;

            anim.SetBool("isWalking", true);
            anim.SetBool("isRunning", false);
            return;
        }
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(playerLastKnownLocation.x, 0, playerLastKnownLocation.z)) < runDistance && Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(playerLastKnownLocation.x, 0, playerLastKnownLocation.z)) > attackDistance)
        {

            //GetComponent<PlayerVelocity>().OverrideVelocity(1.5f);

            // Enable NavMeshAgent, set speed and set destination
            enemyNavMeshAgent.isStopped = false;
            enemyNavMeshAgent.speed = runSpeed;
            enemyNavMeshAgent.destination = playerLastKnownLocation;

            anim.SetBool("isRunning", true);
            anim.SetBool("isWalking", false);
            return;
        }
    }

    void Fleeing()
    {

        //GetComponent<PlayerVelocity>().OverrideVelocity(1.5f);

        if (!fleeLocationFound)
        {
            // Prevent repeated calls
            fleeLocationFound = true;

            // Returns flee location to move towards
            target = FindFleeLocation(fleeLocationsParent);
            enemyNavMeshAgent.isStopped = false;
            enemyNavMeshAgent.destination = target;
        }

        // ...player location and flee location as Vector3s
        Vector3 origin = player.transform.position;
        Vector3 fleePos = target;

        // ...Calculate direction and distance from the player to the flee location
        Vector3 dir = fleePos - origin;

        Debug.DrawRay(transform.position, dir, Color.red);

        anim.SetBool("isRunning", true);
        anim.SetBool("isWalking", false);

        enemyNavMeshAgent.speed = runSpeed;

        //enemyNavMeshAgent.isStopped = true;
        //target = playerLastKnownLocation;
        //transform.Translate(Vector3.forward * -runSpeed * Time.deltaTime);

        //if(Vector3.Distance(target, transform.position) > 5)
        //{
        //enemyBehaviour = EnemyBehaviours.Scouting;
        //}

        if (Vector3.Distance(transform.position, target) < 1)
        {
            fleeLocationFound = false;
            enemyBehaviour = EnemyBehaviours.Scouting;
            enemyResources.SetHealth(100);
        }
    }

    void Dead()
    {
        GetComponent<PlayerVelocity>().desiredVelocity = 1.5f;
        // ------------------------------------------------
        //FindObjectOfType<AudioManager>().Play("PlayerDeath");
        //----------------------------------------------
        Destroy(gameObject);
        // INSERT DEATH TRIGGER HERE
        spawner.Spawnable();

        enemyNavMeshAgent.enabled = false;
    }

    void RotateTowardsTarget(Vector3 targetPos)
    {
        //Lerp Towards target
        targetRotation = Quaternion.LookRotation(targetPos - transform.position);
        adjRotSpeed = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, adjRotSpeed);
    }

    private Vector3 FindFleeLocation(Transform locations)
    {
        // Fallback variables used in case no flee locations can be found
        float maxDistance = 0;
        Transform fallbackPos = locations;

        // Sort through each flee location...
        foreach (Transform fleeLocation in locations)
        {

            // ...player location and flee location as Vector3s
            Vector3 origin = playerLastKnownLocation;
            Vector3 fleePos = fleeLocation.position;

            // ...Calculate direction and distance from the player to the flee location
            Vector3 dir = fleePos - origin;

            // Bitshifting layer 8 & then telling the layermask to hit everything except layer 8
            int layerMask = 1 << 8;
            layerMask = ~layerMask;

            // ...Cast a ray from the player to the flee location and if it hits something return position
            if (Physics.Raycast(origin, dir, layerMask))
            {
                print("flee location found!");
                return fleeLocation.position;
            }

            // ...Calculate distance between player and flee location..
            float dist = Vector3.Distance(origin, fleePos);

            //  ...If that distance is greater than the current maximumDistance, replace it
            if (dist > maxDistance)
            {
                maxDistance = dist;
                fallbackPos = fleeLocation;
            }
        }

        // Fallback in case flee locations out of line of sight can be found
        return fallbackPos.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Sword" && other.gameObject.transform.root.GetComponent<PlayerVelocity>().GetRightHandVelocity() > 1)
        {
            // Deal damage
            print("Dealt damage!");
            enemyResources.LooseHealth(30);

            // Feedback systems:
            // Enemy hit animation
            anim.SetTrigger("isHit");

            // Particle effect
            // Sound
        }

        if (other.gameObject.tag == "Bullet")
        {
            // Deal damage
            print("Dealt damage!");
            enemyResources.LooseHealth(30);

            // Feedback systems:
            // Enemy hit animation
            anim.SetTrigger("isHit");

            // Particle effect
            // Sound
        }
    }
}
