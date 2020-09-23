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
    [SerializeField] private Transform raycastPoint;

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

        if(other.gameObject.layer == 0)
        {
            if(currentSpark != null)
            {
                Destroy(currentSpark);
                currentSpark = null;
            }
            currentSpark = Instantiate(GameManager.GM.collisionParticleEffect, transform.position, Quaternion.identity);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 0)
        {
            RaycastHit hit;
            int layerMask = 1 << 8;
            layerMask = ~layerMask;
            if (Physics.Raycast(raycastPoint.position, transform.up, out hit, 1, layerMask))
            {
                if(currentSpark != null)
                {
                    currentSpark.transform.position = hit.point;
                    currentSpark.transform.rotation = Quaternion.LookRotation(hit.normal);
                }

                if(currentSpark == null)
                {
                    currentSpark = Instantiate(GameManager.GM.collisionParticleEffect, transform.position, Quaternion.identity);
                }
            }
            else
            {
                currentSpark.GetComponent<ParticleSystem>().Stop();
                Destroy(currentSpark, 1);
                currentSpark = null;
            }


            /*if (currentSpark != null)
            {
                RaycastHit hit;
                int layerMask = 1 << 8;
                layerMask = ~layerMask;
                if (Physics.Raycast(raycastPoint.position, transform.up, out hit, 1, layerMask))
                {
                    currentSpark.transform.position = hit.point;
                    currentSpark.transform.rotation = Quaternion.LookRotation(hit.normal);
                }
                else
                {
                    currentSpark.GetComponent<ParticleSystem>().Stop();
                    Destroy(currentSpark, 1);
                    currentSpark = null;
                }
            }*/
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 0)
        {
            if (currentSpark != null)
            {

                currentSpark.GetComponent<ParticleSystem>().Stop();
                Destroy(currentSpark, 1);
                currentSpark = null;
            }
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
