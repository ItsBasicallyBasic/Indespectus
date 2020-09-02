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

    // Start is called before the first frame update
    void Start()
    {
        if(PV == null) {PV = gameObject.GetComponent<PhotonView>();}
        if(cn == null) {cn = GameObject.FindGameObjectWithTag("NetworkCheck").GetComponent<CheckNetworked>();} 
        if(PV.IsMine || !cn.networked) {
            weaponBehaviour = GetComponentInParent<WeaponBehaviour>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
