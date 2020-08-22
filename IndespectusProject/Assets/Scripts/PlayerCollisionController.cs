﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisionController : MonoBehaviour
{

    private PlayerResources playerResources;
    
    [SerializeField]
    private EnemyAI enemy;
    private PhotonView PV;

    private void Start() {
        PV = gameObject.GetComponent<PhotonView>();
        playerResources = GetComponent<PlayerResources>();
        playerResources.SetHealth(100);
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAI>();

    }

    private void Update()
    {
        if(PV.IsMine) {
            if(playerResources.GetHealth() <= 0)
            {
                SceneManager.LoadScene(0);
                Destroy(gameObject);
            }
        }
        //if(enemy == null) {enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAI>();}
    }

    // Triggers
    private void OnTriggerEnter(Collider other)
    {
        if(PV.IsMine) {
            if (other.gameObject.tag == "Sword")
            {
                print("You've been damaged!");
                playerResources.LooseHealth(30);

                enemy.hitOrMiss = true;

                // Play sound
                // Play visual effect
            }
        }
    }

    // Collisions
    private void OnCollisionEnter(Collision collision)
    {

    }
}
