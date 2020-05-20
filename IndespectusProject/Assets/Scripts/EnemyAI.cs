using System.Collections;
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

    // Rigidbody
    public Rigidbody rb;

    // Animation variables
    public Animator anim;

    // Attack timer variables
    public float attackTime;
    private float attackTimer;

    // Start is called before the first frame update
    void Start()
    {
        enemyBehaviour = EnemyBehaviours.Scouting;

        // Get nav mesh agent from gameobject
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();

        // Add each scout point to scoutLocations list
        foreach(Transform child in scoutLocationsParent)
        {
            scoutLocations.Add(child);
        }

        // Assign first scout location
        int randomIndex = Random.Range(0, scoutLocations.Count);
        nextLocation = scoutLocations[randomIndex];

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        playerVelocity = player.GetComponent<PlayerVelocity>().velocity;

        if (!attacking)
        {
            // Look at target
            RotateTowardsTarget(new Vector3(target.x, transform.position.y, target.z));
        }

        if (playerVelocity > 0.5f)
        {
            playerLastKnownLocation = player.GetComponent<PlayerVelocity>().head.transform.position;
            enemyBehaviour = EnemyBehaviours.Attacking;
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
    }

    void Scouting()
    {

        // Setting position to scout toward
        target = nextLocation.position;

        // Enable NavMeshAgent
        enemyNavMeshAgent.enabled = true;

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

        if (Vector3.Distance(transform.position, playerLastKnownLocation) <= attackDistance)
        {
            enemyNavMeshAgent.enabled = false;
            GetComponent<PlayerVelocity>().velocity = 1;
            
            if (Time.time > attackTimer)
            {
                attackTimer = Time.time + attackTime;
                
                // INSERT ATTACK ANIMATION TRIGGER HERE
            }
        }
        else if(Vector3.Distance(transform.position, playerLastKnownLocation) > attackDistance && Vector3.Distance(transform.position, playerLastKnownLocation) > runDistance)
        {
            // Enable NavMeshAgent, set speed and set destination
            enemyNavMeshAgent.enabled = true;
            enemyNavMeshAgent.speed = stealthSpeed;
            enemyNavMeshAgent.destination = playerLastKnownLocation;
        }
        else if (Vector3.Distance(transform.position, playerLastKnownLocation) < runDistance)
        {
            // Enable NavMeshAgent, set speed and set destination
            enemyNavMeshAgent.enabled = true;
            enemyNavMeshAgent.speed = runSpeed;
            enemyNavMeshAgent.destination = playerLastKnownLocation;
        }
    }

    void Fleeing()
    {
        enemyNavMeshAgent.enabled = false;
        target = playerLastKnownLocation;
        transform.Translate(Vector3.forward * -runSpeed * Time.deltaTime);

        if(Vector3.Distance(target, transform.position) > 5)
        {
            enemyBehaviour = EnemyBehaviours.Scouting;
        }
    }

    void Dead()
    {
        enemyNavMeshAgent.enabled = false;
    }

    void RotateTowardsTarget(Vector3 targetPos)
    {
        //Lerp Towards target
        targetRotation = Quaternion.LookRotation(targetPos - transform.position);
        adjRotSpeed = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, adjRotSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Sword")
        {

            enemyBehaviour = EnemyBehaviours.Idle;
            enemyNavMeshAgent.enabled = false;
            rb.isKinematic = false;
        }
    }
}
