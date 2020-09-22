using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SwordCollisionController : MonoBehaviour
{

    [SerializeField] CheckNetworked cn;
    [SerializeField] private PhotonView PV;
    public WeaponBehaviour weaponBehaviour;
    public PlayerVelocity playerVelocity;

    private GameObject currentSpark;

    // Start is called before the first frame update
    void Start()
    {
        if(PV == null) {PV = gameObject.GetComponent<PhotonView>();}
        if(cn == null) {cn = GameObject.FindGameObjectWithTag("NetworkCheck").GetComponent<CheckNetworked>();} 
        if(PV.IsMine || !cn.networked) {
            weaponBehaviour = GetComponentInParent<WeaponBehaviour>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PhotonView otherPhotonView = other.transform.GetComponent<PhotonView>();

        if (otherPhotonView != null && !otherPhotonView.IsMine && PV.IsMine /*|| !cn.networked*/) {
            if(other.gameObject.tag == "Sword" || other.gameObject.tag == "Shield")
            {
                print("Blocked!");
                weaponBehaviour.swordBroken = true;
            }
        }

        if(other.gameObject.tag != "Player")
        {
            if(currentSpark != null)
            {
                Destroy(currentSpark);
                currentSpark = null;
            }
            currentSpark = Instantiate(GameManager.GM.collisionParticleEffect, transform.position, transform.rotation);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        PhotonView otherPhotonView = other.transform.GetComponent<PhotonView>();

        if (otherPhotonView != null && !otherPhotonView.IsMine && PV.IsMine /*|| !cn.networked*/)
        {
            if (currentSpark != null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.up * 1, out hit))
                {
                    currentSpark.transform.position = hit.point;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(currentSpark != null)
        {
            Destroy(currentSpark);
            currentSpark = null;
        }
    }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     if(collision.gameObject.tag == "Shield")
    //     {
    //         //print("Blocked!");
    //         //weaponBehaviour.swordBroken = true;
    //     }
    // }
}
