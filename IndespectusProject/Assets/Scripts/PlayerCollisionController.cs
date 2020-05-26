using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisionController : MonoBehaviour
{

    private PlayerResources playerResources;

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
        }
    }

    // Triggers
    private void OnTriggerEnter(Collider other)
    {
        
    }

    // Collisions
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            print("You've been damaged!");
            playerResources.LooseHealth(30);

            // Play sound
            // Play visual effect
        }
    }
}
