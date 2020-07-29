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

    public bool swordBroken = false;
    private bool swordActive = true;

    private bool brokenTimeStart;
    private float brokenTime = 5;
    private float brokenTimer;

    public ParticleSystem swordDisappear;
    public ParticleSystem swordAppear;
    public ParticleSystem gunDisappear;
    public ParticleSystem gunAppear;

    public Animator animator;

    // Shoot Reveal Vars
    private bool shotReveal; 
    [SerializeField]
    private float sRevealTime;
    private float sRevealTimer;

    // Multitool States
    public enum MultitoolStates
    {
        Sword,
        Gun
    }

    public MultitoolStates multitoolState;

    private PlayerResources playerResources;

    [SerializeField]
    private GameObject shield;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        nextShot = nextShot + fireRate;
        currSelected = 1;
        multitoolState = MultitoolStates.Sword;

        playerResources = GetComponentInParent<PlayerResources>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (multitoolState)
        {
            case MultitoolStates.Sword:
                SwordEquipped();
                break;
            case MultitoolStates.Gun:
                GunEquipped();
                break;
        }

        if (swordBroken)
        {
            if (!brokenTimeStart)
            {
                brokenTimeStart = true;
                brokenTimer = Time.time + brokenTime;
            }

            if (Time.time > brokenTimer)
            {
                swordBroken = false;
                brokenTimeStart = false;
            }
        }

        // Shoot Reveal
        if(shotReveal) {

            GetComponentInParent<PlayerVelocity>().OverrideVelocity(1.5f);

            if(Time.time > sRevealTimer) {
                shotReveal = false;
            }
        }

        // Increase essence gradually
        playerResources.GainEssence(10 * Time.deltaTime);

        // If player has less than 10 essence
        if(playerResources.GetEssence() <= 10)
        {
            // Disable shield
            shield.SetActive(false);
        }
    }

        // If player has more than 10 essence
        if(playerResources.GetEssence() > 10)
        {
            // ENable shield
            shield.SetActive(true);
        }

    }

    void SwordEquipped()
    {
        if (changeWeapon.stateDown && gameObject.tag == "Player")
        {
            multitoolState = MultitoolStates.Gun;
        }

        if (currSelected == 1)
        {
            currSelected = 0;

            if(gameObject.tag == "Player")
            {
                DeactivateGun();
            }

            if (!swordBroken)
            {
                ActivateSword();
            }

            if (swordBroken)
            {
                swordActive = false;
            }
        }

        if (!swordBroken && !swordActive)
        {
            ActivateSword();
        }

        if (swordBroken && swordActive)
        {
            DeactivateSword();
        }
    }

    void GunEquipped()
    {
        if (changeWeapon.stateDown)
        {
            multitoolState = MultitoolStates.Sword;
        }

        if (currSelected == 0)
        {
            currSelected = 1;

            ActivateGun();

            if (!swordBroken)
            {
                DeactivateSword();
            }

        }

        if (fireWeapon.stateDown)
        {
            Fire();
        }
    }

    void ActivateGun()
    {
        gunAppear.Play();
        gun.gameObject.SetActive(true);
        gunParticles.SetActive(true);
    }

    void DeactivateGun()
    {
        gunDisappear.Play();
        gun.SetActive(false);
        gunParticles.SetActive(false);
    }

    void ActivateSword()
    {
        swordActive = true;
        swordAppear.Play();
        sword.SetActive(true);
        swordParticles.SetActive(true);
    }

    void DeactivateSword()
    {
        swordActive = false;
        swordDisappear.Play();
        sword.SetActive(false);
        swordParticles.SetActive(false);
    }

    void Fire()
    {
        if(Time.time > nextShot && playerResources.GetEssence() > 0)
        {
            playerResources.LooseEssence(20);

            nextShot = nextShot + fireRate;
            GameObject bulletObject = Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
            Rigidbody rb = bulletObject.GetComponent<Rigidbody>();
            rb.velocity = bulletSpawn.transform.forward * 1000 * Time.deltaTime;

            // Destroy after 5 seconds to cleanup bullet instances that go out of bounds
            Destroy(bulletObject, 5f);

            // Play recoil animation
            animator.Play("Recoil");
            //Sound??

            shotReveal = true;
            sRevealTimer = Time.time +sRevealTime;
        }
    }
}
