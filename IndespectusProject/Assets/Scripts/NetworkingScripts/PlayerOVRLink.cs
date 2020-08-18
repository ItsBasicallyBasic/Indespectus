using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using RootMotion.FinalIK;
using UnityEngine;

public class PlayerOVRLink : MonoBehaviour, IPunObservable {

    [SerializeField] private Transform ovrRig;
    [SerializeField] private Transform[] bones;

    [SerializeField] private VRIK vRIK;

    [SerializeField] private PhotonView PV;

    void Start () {
        Debug.Log("Player instantiated");

        if(PV.IsMine) {
            ovrRig = GameObject.FindGameObjectWithTag("OVRRig").transform;
            ovrRig.transform.parent = this.transform;
            FindRig();
            Debug.Log("Player is mine");
        }
    }

    private void FindRig() {
        vRIK.solver.spine.headTarget = ovrRig.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor/Head");
        vRIK.solver.leftArm.target = ovrRig.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor/hand.L");
        vRIK.solver.rightArm.target = ovrRig.Find("OVRCameraRig/TrackingSpace/RightHandAnchor/hand.R");
        
    }

    // void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
    //     if (stream.IsWriting) {
    //         stream.SendNext(ovrRig.position);
    //         stream.SendNext(ovrRig.rotation);
    //         foreach(Transform bone in bones) {
    //             stream.SendNext(bone.position);
    //             stream.SendNext(bone.rotation);
    //         }
    //     } else {
    //         this.transform.position = (Vector3)stream.ReceiveNext();
    //         this.transform.rotation = (Quaternion)stream.ReceiveNext();
    //         foreach(Transform bone in bones) {
    //             bone.transform.localPosition = (Vector3)stream.ReceiveNext();
    //             bone.transform.localRotation = (Quaternion)stream.ReceiveNext();
    //         }
    //     }
    // }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(ovrRig.position);
            stream.SendNext(ovrRig.rotation);
            foreach(Transform bone in bones) {
                stream.SendNext(bone.position);
                stream.SendNext(bone.rotation);
            }
        } else {
            this.transform.position = (Vector3)stream.ReceiveNext();
            this.transform.rotation = (Quaternion)stream.ReceiveNext();
            foreach(Transform bone in bones) {
                bone.transform.localPosition = (Vector3)stream.ReceiveNext();
                bone.transform.localRotation = (Quaternion)stream.ReceiveNext();
            }
        }
    }
}
