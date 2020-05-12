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
        nextShot = nextShot + fireRate;
        currSelected = 0;
        SwitchWeapons(3);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            SwitchWeapons(1);
        }
        if (Input.GetKeyDown("2"))
        {
            SwitchWeapons(2);
        }
        if (Input.GetKeyDown("3"))
        {
            SwitchWeapons(3);
        }

        if (changeWeapon.stateDown && currSelected == 2)
        {
            SwitchWeapons(3);
            return;
        }

        if (changeWeapon.stateDown && currSelected == 3)
        {
            SwitchWeapons(2);
            return;
        }

        //SteamVR_Input_Sources source = interactable.attachedToHand.handType;
        if (Input.GetButtonDown("Fire1") || fireWeapon.stateDown)
        {
            if(currSelected == 3) Fire();
        }
    }

    void SwitchWeapons(int selected)
    {
        if (selected == 1)
        {
            if(currSelected == 2)
            {
                swordDisappear.Play();
                sword.SetActive(false);
                swordParticles.SetActive(false);
            }
            if(currSelected == 3)
            {
                gunDisappear.Play();
                gun.SetActive(false);
                gunParticles.SetActive(false);
            }
            currSelected = 1;
            return;
        }

        if (selected == 2)
        {
            if (currSelected == 3)
            {
                gunDisappear.Play();
                gun.SetActive(false);
                gunParticles.SetActive(false);
            }
            currSelected = 2;
            swordAppear.Play();
            sword.SetActive(true);
            swordParticles.SetActive(true);
            return;
        }

        if (selected == 3)
        {
            if(currSelected == 2)
            {
                swordDisappear.Play();
                sword.SetActive(false);
                swordParticles.SetActive(false);
            }
            currSelected = 3;
            gunAppear.Play();
            gun.gameObject.SetActive(true);
            gunParticles.SetActive(true);
            return;
        }
    }

    void Fire()
    {
        if(Time.time > nextShot)
        {
            nextShot = nextShot + fireRate;
            GameObject bulletObject = Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
            Rigidbody rb = bulletObject.GetComponent<Rigidbody>();
            rb.velocity = bulletSpawn.transform.forward * 1000 * Time.deltaTime;
            Destroy(bulletObject, 5f);
            animator.Play("Recoil");
            //Sound??
        }
    }
}
