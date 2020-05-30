using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTeleport : MonoBehaviour {

    private bool teleported; 
    [SerializeField]
    private float tRevealTime;
    private float tRevealTimer;

    void onVRTK_BasicTeleport() {
        teleported = true;
        tRevealTimer = Time.time + tRevealTime;
    }

    void Update() {
        if(teleported) {

            GetComponent<PlayerVelocity>().OverrideVelocity(1.5f);

            if(Time.time > tRevealTimer) {
                teleported = false;
            }
        }
    }

}