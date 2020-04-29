using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourAdjust : MonoBehaviour
{

    public Color selectedColour;
    public ParticleSystem handle;
    public ParticleSystem top;

    public GameObject sword;
    public ParticleSystem swordEquip;
    public ParticleSystem swordUnequip;
    public ParticleSystem swordParticles;

    public GameObject gun;
    public ParticleSystem gunEquip;
    public ParticleSystem gunUnequip;
    public ParticleSystem gunParticles;

    public ParticleSystem bulletHit;

    public ParticleSystem bullet;

    public Material weaponMaterial;

    public ParticleSystem shieldParticles;

    public SpriteRenderer UI;

    // Start is called before the first frame update
    void Start()
    {
        var main = handle.GetComponent<ParticleSystem>().main;
        main.startColor = selectedColour;
        main = top.GetComponent<ParticleSystem>().main;
        main.startColor = selectedColour;

        //main = sword.GetComponent<ParticleSystem>().main;
       // main.startColor = selectedColour;

        //main = gun.GetComponent<ParticleSystem>().main;
        //main.startColor = selectedColour;

        main = swordEquip.main;
        main.startColor = selectedColour;

        main = swordUnequip.main;
        main.startColor = selectedColour;

        main = gunEquip.main;
        main.startColor = selectedColour;

        main = gunUnequip.main;
        main.startColor = selectedColour;

        main = bullet.main;
        main.startColor = selectedColour;

        main = swordParticles.main;
        main.startColor = selectedColour;

        main = gunParticles.main;
        main.startColor = selectedColour;

        main = bullet.main;
        main.startColor = selectedColour;

        main = bulletHit.main;
        main.startColor = selectedColour;

        main = shieldParticles.main;
        main.startColor = selectedColour;

        //UI.color = selectedColour;

        weaponMaterial.SetColor("_Color", selectedColour);
        weaponMaterial.SetColor("_EmissionColor", selectedColour);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
