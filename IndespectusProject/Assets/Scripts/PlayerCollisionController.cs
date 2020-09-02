using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisionController : MonoBehaviour
{

    private PlayerResources playerResources;
    
    //[SerializeField]
    //private EnemyAI enemy;
    [SerializeField] private PhotonView PV;

    // Haptic feedback
    [SerializeField]
    private AudioClip hapticAudioClip;
    [SerializeField] CheckNetworked cn;

    private void Start() {
        if(PV == null) {PV = gameObject.GetComponent<PhotonView>();}
        if(cn == null) {cn = GameObject.FindGameObjectWithTag("NetworkCheck").GetComponent<CheckNetworked>();}
        if(PV.IsMine || !cn.networked) {
            playerResources = GetComponent<PlayerResources>();
            playerResources.SetHealth(100);
            // enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAI>();
        }

    }

    private void Update()
    {
        if(PV.IsMine || !cn.networked) {
            if(playerResources.GetHealth() <= 0)
            {
                SceneManager.LoadScene(0);
                Destroy(gameObject);
            }
        }
        if(!cn.networked) {
            //if(enemy == null) {enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAI>();}
        } // else {
        //     if(players == null) {
        //         players = GameObject.FindGameObjectsWithTag("Player")
        //     }
        // }
        
    }

    // Triggers
    private void OnTriggerEnter(Collider other)
    {
        PhotonView otherPhotonView = other.transform.GetComponent<PhotonView>();

        if (otherPhotonView != null && !otherPhotonView.IsMine && PV.IsMine /*|| !cn.networked*/) {
            if (other.gameObject.tag == "Sword" || other.gameObject.tag == "Bullet")
            {

                // Sending hit over the network
                //PhotonView otherPhotonView = other.transform.root.GetComponent<PhotonView>();
                //otherPhotonView.RPC("DealDamage", otherPhotonView.Owner, 30);


                print("You've been damaged!");
                playerResources.LooseHealth(30);

                if(!cn.networked) {
                    //enemy.hitOrMiss = true;
                }

                // Play sound
                // Play visual effect

                // Haptic feedback
                OVRHapticsClip hapticsClip = new OVRHapticsClip(hapticAudioClip);
                OVRHaptics.RightChannel.Preempt(hapticsClip);
                OVRHaptics.LeftChannel.Preempt(hapticsClip);
            }
        }
    }

    [PunRPC]
    void DealDamage(float damage)
    {
        GetComponent<PlayerResources>().LooseHealth(damage);
    }


    // Collisions
    private void OnCollisionEnter(Collision collision)
    {

    }
}
