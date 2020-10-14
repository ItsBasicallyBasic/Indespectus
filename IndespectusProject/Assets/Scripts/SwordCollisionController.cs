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

    [SerializeField] private GameObject swordBlock;
    [SerializeField] private GameObject swordBreak;
    [SerializeField] private GameObject damagedParticleEffect;

    private GameObject currentSpark;
    [SerializeField] private Transform raycastPoint;

    // Start is called before the first frame update
    void Start()
    {
        if (PV == null) { PV = gameObject.GetComponent<PhotonView>(); }
        if (cn == null) { cn = GameObject.FindGameObjectWithTag("NetworkCheck").GetComponent<CheckNetworked>(); }
        if (PV.IsMine /*|| !cn.networked*/)
        {
            weaponBehaviour = GetComponentInParent<WeaponBehaviour>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Instantiate(damagedParticleEffect, transform.position + transform.up * 0.5f, transform.rotation);
        //Instantiate(swordBlock, transform.position + transform.up * 0.5f, transform.rotation);
        PhotonView otherPhotonView = other.transform.GetComponent<PhotonView>();

        if (otherPhotonView != null && !otherPhotonView.IsMine && PV.IsMine /*|| !cn.networked*/)
        {
            if (other.gameObject.tag == "Sword" || other.gameObject.tag == "Shield")
            {
                print("Blocked!");
                Instantiate(swordBlock, transform.position + transform.up * 0.5f, transform.rotation);
                Instantiate(swordBreak, transform.position, transform.rotation);
                GameManager.GM.audioManager.PlaySound(transform.parent.GetComponent<AudioSource>(), "swordBreak", 1);
                weaponBehaviour.swordBroken = true;
                if (other.gameObject.tag == "Sword")
                {
                    other.transform.parent.gameObject.GetComponent<WeaponBehaviour>().swordBroken = true;
                    Instantiate(swordBreak, other.gameObject.transform.parent.position, other.gameObject.transform.parent.rotation);
                }

            }

            if (other.gameObject.tag == "Player")
            {
                Instantiate(damagedParticleEffect, transform.position + transform.up * 0.5f, transform.rotation);
                GameManager.GM.audioManager.PlaySound(transform.parent.GetComponent<AudioSource>(), "Damaged1", 1);
            }
        }

        if (other.gameObject.tag == "Sword" || other.gameObject.tag == "Shield")
        {
            Instantiate(swordBlock, transform.position + transform.up * 0.5f, transform.rotation);
        }

        if (other.gameObject.layer == 0)
        {
            GameManager.GM.audioManager.PlayRepeatingSound(GetComponentInParent<AudioSource>(), "swordCollide");

            if (currentSpark != null)
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
                if (currentSpark != null)
                {
                    currentSpark.transform.position = hit.point;
                    currentSpark.transform.rotation = Quaternion.LookRotation(hit.normal);
                }

                if (currentSpark == null)
                {
                    currentSpark = Instantiate(GameManager.GM.collisionParticleEffect, transform.position, Quaternion.identity);
                }
            }
            else
            {
                if (currentSpark != null)
                {
                    currentSpark.GetComponent<ParticleSystem>().Stop();
                    Destroy(currentSpark, 1);
                    currentSpark = null;
                }
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
            GameManager.GM.audioManager.StopRepeatingSound(GetComponentInParent<AudioSource>());

            if (currentSpark != null)
            {

                currentSpark.GetComponent<ParticleSystem>().Stop();
                Destroy(currentSpark, 1);
                currentSpark = null;
            }
        }
    }

    public void ForceDestroySpark()
    {
        GameManager.GM.audioManager.StopRepeatingSound(GetComponentInParent<AudioSource>());

        if (currentSpark != null)
        {
            currentSpark.GetComponent<ParticleSystem>().Stop();
            Destroy(currentSpark, 1);
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

    [PunRPC]
    void BreakSword()
    {
        weaponBehaviour.swordBroken = true;
    }
}
