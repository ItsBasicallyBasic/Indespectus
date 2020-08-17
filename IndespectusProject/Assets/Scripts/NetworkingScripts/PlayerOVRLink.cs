using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerOVRLink : MonoBehaviour {

    public Transform playerGlobal;
    public Transform posLocal;
    // public Transform rHandLocal;
    // public Transform lHandLocal;
    public GameObject thisAnchor;
    // public GameObject lHand;
    // public GameObject rHand;
    public enum Anchor
    {
        Head,
        LeftHand,
        RightHand
    }

    public Anchor anchor;

    [SerializeField] private PhotonView PV;

    void Start () {
        Debug.Log("Player instantiated");

        if(PV.IsMine) {
            playerGlobal = GameObject.FindGameObjectWithTag("OVRRig").transform;
            switch(anchor) {
                case Anchor.Head:
                    posLocal = playerGlobal.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor");
                    break;
                case Anchor.LeftHand:
                    posLocal = playerGlobal.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor");
                    break;
                case Anchor.RightHand:
                    posLocal = playerGlobal.Find("OVRCameraRig/TrackingSpace/RightHandAnchor");
                    break;
            }
            Debug.Log("Player is mine");

            this.transform.SetParent(posLocal);
            this.transform.localPosition = Vector3.zero;
        }
    }
	
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(playerGlobal.position);
            stream.SendNext(playerGlobal.rotation);
            stream.SendNext(posLocal.localPosition);
            stream.SendNext(posLocal.localRotation);
        } else {
            this.transform.position = (Vector3)stream.ReceiveNext();
            this.transform.rotation = (Quaternion)stream.ReceiveNext();
            thisAnchor.transform.localPosition = (Vector3)stream.ReceiveNext();
            thisAnchor.transform.localRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
