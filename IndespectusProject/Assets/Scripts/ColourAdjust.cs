using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourAdjust : MonoBehaviour
{
    public Color selectedColour;
    public ParticleSystem[] particleSystems;
    public Material[] materials;

    // Start is called before the first frame update
    void Start()
    {
        // Changing particle system colours to selected colour.
        foreach(ParticleSystem particleSystem in particleSystems)
        {
            var main = particleSystem.main;
            main.startColor = selectedColour;
        }

        // Changing material colours to selected colour.
        foreach(Material material in materials)
        {
            material.SetColor("_Color", selectedColour);
            material.SetColor("_EmissionColor", selectedColour);
        }
    }
}
