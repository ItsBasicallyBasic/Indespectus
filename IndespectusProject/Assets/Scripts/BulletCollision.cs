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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    private void OnCollisionEnter(Collision collision)
    {
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

    [PunRPC]
    void RPC_HitOther(Collider other) {
        if(other.gameObject.tag == "Enemy") {
            PhotonNetwork.Instantiate(Path.Combine("NetworkPrefabs", "Hit"), transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other) {
        PV.RPC("RPC_HitOther", RpcTarget.All, other);
    }

    [PunRPC]
    void Hit(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 position = contact.point;
        GameObject hit = PhotonNetwork.Instantiate(Path.Combine("NetworkPrefabs", "Hit"), transform.position, transform.rotation);
        hit.transform.rotation = Quaternion.FromToRotation(hit.transform.up, contact.normal);
        Destroy(hit, 0.5f);
    }
}
