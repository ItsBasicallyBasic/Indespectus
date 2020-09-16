using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{

    // public GameObject bulletHit;

    [SerializeField]
    private PhotonView PV;
    [SerializeField] CheckNetworked cn;

    private int rebounds = 0;

    void Awake() {
        if(PV == null) {PV = gameObject.GetComponent<PhotonView>();}
        if(cn == null) {cn = GameObject.FindGameObjectWithTag("NetworkCheck").GetComponent<CheckNetworked>();}
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

            if(other.tag == "Shield" || other.tag == "Sword" || other.tag == "Player" || other.tag == "Untagged")
            {
                print("hit shield or sword!");
                Destroy(gameObject);
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
