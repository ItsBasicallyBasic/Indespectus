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

    private int rebounds = 0;

    void Awake() {
        PV = gameObject.GetComponent<PhotonView>();
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
                Destroy(gameObject);
            }
        }
    }

    [PunRPC]
    void RPC_HitOther(Collider other) {
        if(PV.IsMine) {
            if(other.gameObject.tag == "Enemy") {
                PhotonNetwork.Instantiate(Path.Combine("NetworkPrefabs", "Hit"), transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(PV.IsMine) {
            PV.RPC("RPC_HitOther", RpcTarget.All, other);
        }
    }

    [PunRPC]
    void Hit(Collision collision) {
        if(PV.IsMine) {
            ContactPoint contact = collision.contacts[0];
            Vector3 position = contact.point;
            GameObject hit = PhotonNetwork.Instantiate(Path.Combine("NetworkPrefabs", "Hit"), transform.position, transform.rotation);
            hit.transform.rotation = Quaternion.FromToRotation(hit.transform.up, contact.normal);
            Destroy(hit, 0.5f);
        }
    }
}
