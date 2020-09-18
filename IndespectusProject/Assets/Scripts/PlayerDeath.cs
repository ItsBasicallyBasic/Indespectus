using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

public class PlayerDeath : MonoBehaviour {
    
    [SerializeField] VRIK vrik;
    [SerializeField] Animator animatior;
    [SerializeField] GameObject[] Weapons;
    [SerializeField] PlayerVelocity playerVelocity;
    [SerializeField] Material MainMat;

    // // Start is called before the first frame update
    // void Start() {
        
    // }

    // // Update is called once per frame
    // void Update() {
        
    // }

    public void DeathAnimation() {
        vrik.enabled = false;
        animatior.Play("deathAnim");
        foreach(GameObject weapon in Weapons) {
            weapon.SetActive(false);
        }
        playerVelocity.OverrideVelocity(1.5f);
        playerVelocity.overrideV = true;
        MainMat = playerVelocity.myMats[1];
        MainMat.SetColor("_GlowColor", new Vector4(0.0006f, 0.0173f, 0.0186f, 1f));

        
    }
}
