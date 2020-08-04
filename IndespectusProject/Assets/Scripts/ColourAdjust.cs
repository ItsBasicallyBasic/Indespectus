using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourAdjust : MonoBehaviour
{

    public Color selectedColour;

    public GameObject sword;

    public GameObject gun;

    public ParticleSystem bulletHit;

    public ParticleSystem bullet;

    public Material weaponMaterial;

    public SpriteRenderer UI;

    // Start is called before the first frame update
    void Start()
    {
        var main = GetComponent<ParticleSystem>().main;

        //main = sword.GetComponent<ParticleSystem>().main;
       // main.startColor = selectedColour;

        //main = gun.GetComponent<ParticleSystem>().main;
        //main.startColor = selectedColour;

        main = bullet.main;
        main.startColor = selectedColour;

        main = bullet.main;
        main.startColor = selectedColour;

        main = bulletHit.main;
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
