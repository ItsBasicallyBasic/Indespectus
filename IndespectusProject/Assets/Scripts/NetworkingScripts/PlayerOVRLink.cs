using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using RootMotion.FinalIK;
using UnityEngine;

public class PlayerOVRLink : MonoBehaviour, IPunObservable {

    [SerializeField] private Transform ovrRig;
    [SerializeField] private Transform[] bonesIKRef;
    [SerializeField] private Transform[] backupBonesIKRef;
    [SerializeField] private bool NonNetworked;
    // [SerializeField] private Transform[] bones;
    // [SerializeField] private LocomotionController LC;
    // [SerializeField] private TeleportInputHandlerTouch TH;
    // [SerializeField] private TeleportTransitionBlink TT;

    [SerializeField] private VRIK vRIK;

    [SerializeField] private PhotonView PV;

    void Start () {
        Debug.Log("Player instantiated");
        bonesIKRef = new Transform[3];

        if(ovrRig == null) {
            if(PV.IsMine) {
                ovrRig = GameObject.FindGameObjectWithTag("OVRRig").transform;
                ovrRig.position = this.transform.position;

                bonesIKRef[0] = ovrRig.Find("TrackingSpace/CenterEyeAnchor/Head");
                bonesIKRef[1] = ovrRig.Find("TrackingSpace/LeftHandAnchor/hand.L");
                bonesIKRef[2] = ovrRig.Find("TrackingSpace/RightHandAnchor/hand.R");

                Debug.Log("Player is mine");
            } else {
                // vRIK.enabled = false;
                bonesIKRef = backupBonesIKRef;
            }
            FindRig();
            
        } else {
            bonesIKRef[0] = ovrRig.Find("TrackingSpace/CenterEyeAnchor/Head");
            bonesIKRef[1] = ovrRig.Find("TrackingSpace/LeftHandAnchor/hand.L");
            bonesIKRef[2] = ovrRig.Find("TrackingSpace/RightHandAnchor/hand.R");
        }
        print(bonesIKRef[0]);
        this.transform.SetParent(GameObject.FindGameObjectWithTag("OVRRig").transform); // <- used a direct reference because using ovrRig did not work
        this.transform.localPosition = Vector3.zero + Vector3.up * 1;
    }
    private void FindRig() {
        vRIK.solver.spine.headTarget = bonesIKRef[0];
        vRIK.solver.leftArm.target = bonesIKRef[1];
        vRIK.solver.rightArm.target = bonesIKRef[2];        
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(ovrRig.position);
            stream.SendNext(ovrRig.rotation);
            foreach(Transform bone in bonesIKRef) {
                stream.SendNext(bone.position);
                stream.SendNext(bone.rotation);
            }
        } else {
            this.transform.position = (Vector3)stream.ReceiveNext();
            this.transform.rotation = (Quaternion)stream.ReceiveNext();
            foreach(Transform bone in bonesIKRef) {
                bone.transform.position = (Vector3)stream.ReceiveNext();
                bone.transform.rotation = (Quaternion)stream.ReceiveNext();
            }
        }
    }
}
