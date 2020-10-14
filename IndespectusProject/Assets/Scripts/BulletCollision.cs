using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{

    [SerializeField] private GameObject bulletHit;
    [SerializeField] private GameObject bulletBlocked;
    [SerializeField] private GameObject bulletDamage;

    private Vector3 hitPos;

    [SerializeField]
    private PhotonView PV;
    [SerializeField] CheckNetworked cn;

    private int rebounds = 0;

    void Awake() {
        if(PV == null) {PV = gameObject.GetComponent<PhotonView>();}
        if(cn == null) {cn = GameObject.FindGameObjectWithTag("NetworkCheck").GetComponent<CheckNetworked>();}
    }

    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 1))
        {
            hitPos = hit.point;
        }
    }

    [PunRPC]
    private void OnCollisionEnter(Collision collision) {
        if(PV.IsMine) {
            rebounds++;
            if (collision.gameObject.tag == "Sword")
            {
                rebounds --;
            }
            if (rebounds > 1 || collision.gameObject.tag == "Shield")
            {
                PV.RPC("Hit", RpcTarget.All, collision);
                PhotonNetwork.Destroy(gameObject);
            }
        }

        // if(!cn.networked) {
        //     rebounds++;
        //     if (collision.gameObject.tag == "Sword")
        //     {
        //         rebounds --;
        //     }
        //     if (rebounds > 1 || collision.gameObject.tag == "Shield")
        //     {
        //         PV.RPC("Hit", RpcTarget.All, collision);
        //         PhotonNetwork.Destroy(gameObject);
        //     }
        // }

    }

    [PunRPC]
    void RPC_HitOther(Collider other) {
        if(PV.IsMine || !cn.networked) {
            // if(other.gameObject.tag == "Enemy") {
                PhotonNetwork.Instantiate(Path.Combine("NetworkPrefabs", "Hit"), transform.position, transform.rotation);
                PhotonNetwork.Destroy(gameObject);
            // }
        }
    }
    private void OnTriggerEnter(Collider other) {
        print("hit something");
        //if(PV.IsMine) {
            //PV.RPC("RPC_HitOther", RpcTarget.All, other);

            if(other.tag == "Shield" || other.tag == "Sword")
            {
                // blocked particle effect
                Instantiate(bulletBlocked, hitPos, transform.rotation);
                Instantiate(bulletHit, hitPos, transform.rotation);

                Destroy(gameObject);
            }

            if(other.tag == "Player")
            {
                print("hit shield or sword!");

                // bullet explosion particle effect
                Instantiate(bulletHit, hitPos, transform.rotation);
                Instantiate(bulletDamage, hitPos, transform.rotation);
                //Instantiate(bulletBlocked, hitPos, transform.rotation);

                Destroy(gameObject);
            }

            if(other.tag == "Untagged")
            {
                Instantiate(bulletHit, hitPos, transform.rotation);
        }
        //}
        if(!cn.networked) {
            RPC_HitOther(other);
        }
    }

    [PunRPC]
    void Hit(Collision collision) {
        if(PV.IsMine  || !cn.networked) {
            ContactPoint contact = collision.contacts[0];
            Vector3 position = contact.point;
            GameObject hit = PhotonNetwork.Instantiate(Path.Combine("NetworkPrefabs", "Hit"), transform.position, transform.rotation);
            hit.transform.rotation = Quaternion.FromToRotation(hit.transform.up, contact.normal);
            // PhotonNetwork.Destroy(hit);
        }
    }
}
