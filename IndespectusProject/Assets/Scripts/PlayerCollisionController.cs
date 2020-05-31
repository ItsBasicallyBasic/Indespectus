using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisionController : MonoBehaviour
{

    private PlayerResources playerResources;
    
    [SerializeField]
    private EnemyAI enemy;

    private void Start()
    {
        playerResources = GetComponent<PlayerResources>();
        playerResources.SetHealth(100);
        
    }

    private void Update()
    {
        if(playerResources.GetHealth() <= 0)
        {
            SceneManager.LoadScene(0);
            Destroy(gameObject);
        }
        if(enemy == null && GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAI>()) {enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAI>();}
    }

    // Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword")
        {
            print("You've been damaged!");
            playerResources.LooseHealth(30);

            enemy.hitOrMiss = true;

            // Play sound
            // Play visual effect
        }
    }

    // Collisions
    private void OnCollisionEnter(Collision collision)
    {

    }
}
