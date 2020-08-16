using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerOVRLink : MonoBehaviour {

    public Transform playerGlobal;
    public Transform headLocal;
    public Transform rHandLocal;
    public Transform lHandLocal;
    public GameObject head;
    public GameObject lHand;
    public GameObject rHand;

    [SerializeField] private PhotonView PV;

    void Start () {
        Debug.Log("Player instantiated");

       // if(PV.IsMine) {
            Debug.Log("Player is mine");

            playerGlobal = GameObject.FindGameObjectWithTag("OVRRig").transform;
            headLocal = playerGlobal.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor");
            lHandLocal = playerGlobal.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor");
            rHandLocal = playerGlobal.Find("OVRCameraRig/TrackingSpace/RightHandAnchor");

            this.transform.SetParent(headLocal);
            this.transform.localPosition = Vector3.zero;
       // }
    }
	
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(playerGlobal.position);
            stream.SendNext(playerGlobal.rotation);
            stream.SendNext(headLocal.localPosition);
            stream.SendNext(headLocal.localRotation);
            stream.SendNext(lHandLocal.localPosition);
            stream.SendNext(lHandLocal.localRotation);
            stream.SendNext(rHandLocal.localPosition);
            stream.SendNext(rHandLocal.localRotation);
        } else {
            this.transform.position = (Vector3)stream.ReceiveNext();
            this.transform.rotation = (Quaternion)stream.ReceiveNext();
            head.transform.localPosition = (Vector3)stream.ReceiveNext();
            head.transform.localRotation = (Quaternion)stream.ReceiveNext();
            lHand.transform.localPosition = (Vector3)stream.ReceiveNext();
            lHand.transform.localRotation = (Quaternion)stream.ReceiveNext();
            rHand.transform.localPosition = (Vector3)stream.ReceiveNext();
            rHand.transform.localRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
