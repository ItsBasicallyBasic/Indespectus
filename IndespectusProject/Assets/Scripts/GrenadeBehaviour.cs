using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBehaviour : MonoBehaviour
{

    private bool activated = false;
    private bool thrown = false;

    public enum GrenadeTypes
    {
        Invisibility,
        Syphoning,
        Revealing
    }

    public GrenadeTypes grenadeType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "grenade")
        {
            activated = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "grenade")
        {
            thrown = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        TriggerGrenade();
    }

    private void TriggerGrenade()
    {
        if(grenadeType == GrenadeTypes.Invisibility)
        {

        }
        else if (grenadeType == GrenadeTypes.Syphoning)
        {

        }
        else if (grenadeType == GrenadeTypes.Revealing)
        {

        }
    }

    private void InvisibilityEffect()
    {

    }

    private void SyphoningEffect()
    {

    }

    private void RevealingEffect()
    {

    }
}