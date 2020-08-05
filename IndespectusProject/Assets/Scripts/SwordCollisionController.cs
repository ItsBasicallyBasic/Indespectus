using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollisionController : MonoBehaviour
{

    public WeaponBehaviour weaponBehaviour;
    public PlayerVelocity playerVelocity;

    // Start is called before the first frame update
    void Start()
    {
        weaponBehaviour = GetComponentInParent<WeaponBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Sword" || other.gameObject.tag == "Shield")
        {
            print("Blocked!");
            weaponBehaviour.swordBroken = true;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Shield")
        {
            //print("Blocked!");
            //weaponBehaviour.swordBroken = true;
        }
    }
}
