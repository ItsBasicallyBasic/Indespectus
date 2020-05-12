using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class WeaponBehaviour : MonoBehaviour
{
    public SteamVR_Action_Boolean fireWeapon;
    public SteamVR_Action_Boolean changeWeapon;
    private Interactable interactable;

    public GameObject sword;
    public GameObject gun;
    public GameObject swordParticles;
    public GameObject gunParticles;
    private int currSelected = 0;

    public float fireRate = 1;
    private float nextShot = 0;
    public GameObject bullet;
    public GameObject bulletSpawn;

    public ParticleSystem swordDisappear;
    public ParticleSystem swordAppear;
    public ParticleSystem gunDisappear;
    public ParticleSystem gunAppear;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currSelected = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Weapon state switch
        switch (currSelected)
        {
            case 1:
                SwordBroken();
                if (changeWeapon.stateDown)
                {
                    currSelected = 3;
                }
                break;
            case 2:
                SwordEquipped();
                if (changeWeapon.stateDown)
                {
                    currSelected = 3;
                }
                break;
            case 3:
                GunEquipped();
                if (changeWeapon.stateDown)
                {
                    currSelected = 2;
                }
                break;
        }

        if (fireWeapon.stateDown && currSelected == 3)
        {
            Fire();
        }
    }

    // Sword broken state
    void SwordBroken()
    {
        swordDisappear.Play();
        sword.SetActive(false);
        swordParticles.SetActive(false);
    }

    // Sword equipped state
    void SwordEquipped()
    {
        gunDisappear.Play();
        gun.SetActive(false);
        gunParticles.SetActive(false);

        swordAppear.Play();
        sword.SetActive(true);
        swordParticles.SetActive(true);
    }

    // Gun equipped state
    void GunEquipped()
    {
        swordDisappear.Play();
        sword.SetActive(false);
        swordParticles.SetActive(false);

        gunAppear.Play();
        gun.gameObject.SetActive(true);
        gunParticles.SetActive(true);
    }

    // Fire gun
    void Fire()
    {
        // If statement to limit firerate
        if(Time.time > nextShot)
        {
            nextShot = Time.time + fireRate;
            // Create bullet instance, set the velocity of the rigidbody
            GameObject bulletObject = Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
            Rigidbody rb = bulletObject.GetComponent<Rigidbody>();
            rb.velocity = bulletSpawn.transform.forward * 1000 * Time.deltaTime;

            // Destroy after 5 seconds to cleanup bullet instances that go out of bounds
            Destroy(bulletObject, 5f);

            // Play recoil animation
            animator.Play("Recoil");

            // Sound??
        }
    }
}
