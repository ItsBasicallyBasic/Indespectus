using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    public GameObject sword;
    public GameObject gun;
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

    [SerializeField]
    private PlayerResources playerResources;

    [SerializeField]
    private GameObject shield;

    // Haptic feedback
    [SerializeField]
    private AudioClip hapticAudioClip;

    [SerializeField] private PhotonView PV;

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
        if(PV.IsMine) {
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

            // If player has more than 10 essence
            if(playerResources.GetEssence() > 10)
            {
                // ENable shield
                shield.SetActive(true);
            }
        }

    }

    void SwordEquipped() {
        // if(PV.IsMine) {
            if (OVRInput.GetDown(OVRInput.Button.Two) && gameObject.tag == "Player")
            {
                multitoolState = MultitoolStates.Gun;
            }

            if (currSelected == 1)
            {
                currSelected = 0;

                if(gameObject.tag == "Player")
                {
                    // DeactivateGun();
                    PV.RPC("DeactivateGun", RpcTarget.AllBuffered);
                }

                if (!swordBroken)
                {
                    // ActivateSword();
                    PV.RPC("ActivateSword", RpcTarget.AllBuffered);
                }

                if (swordBroken)
                {
                    swordActive = false;
                }
            }

            if (!swordBroken && !swordActive)
            {
                // ActivateSword();
                PV.RPC("ActivateSword", RpcTarget.AllBuffered);
            }

            if (swordBroken && swordActive)
            {
                // DeactivateSword();
                PV.RPC("DeactivateSword", RpcTarget.AllBuffered);
            }
        // }
    }

    void GunEquipped()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            multitoolState = MultitoolStates.Sword;
        }

        if (currSelected == 0)
        {
            currSelected = 1;

            // ActivateGun();
            PV.RPC("ActivateGun", RpcTarget.AllBuffered);

            if (!swordBroken)
            {
                // DeactivateSword();
                PV.RPC("DeactivateSword", RpcTarget.AllBuffered);
            }

        }

        if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.5f)
        {
            // Fire();
            PV.RPC("Fire", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void ActivateGun()
    {
        gun.gameObject.SetActive(true);
    }

    [PunRPC]
    void DeactivateGun()
    {
        gun.SetActive(false);
    }

    [PunRPC]
    void ActivateSword()
    {
        swordActive = true;
        sword.SetActive(true);
    }

    [PunRPC]
    void DeactivateSword()
    {
        swordActive = false;
        sword.SetActive(false);
    }

    [PunRPC]
    void Fire()
    {
        if(PV.IsMine) {
            if(Time.time > nextShot && playerResources.GetEssence() > 0)
            {
                playerResources.LooseEssence(20);

                // Haptic feedback
                OVRHapticsClip hapticsClip = new OVRHapticsClip(hapticAudioClip);
                OVRHaptics.RightChannel.Preempt(hapticsClip);

                nextShot = Time.time + fireRate;
                // GameObject bulletObject = Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                GameObject bulletObject = PhotonNetwork.Instantiate(Path.Combine("NetworkPrefabs", "Bullet"), bulletSpawn.transform.position, bulletSpawn.transform.rotation);
                Rigidbody rb = bulletObject.GetComponent<Rigidbody>();
                rb.velocity = bulletSpawn.transform.forward * 1000 * Time.deltaTime;
                Destroy(bulletObject, 5f);
                animator.Play("Recoil");
                //Sound??

                shotReveal = true;
                sRevealTimer = Time.time +sRevealTime;
            }
        }
    }
}
