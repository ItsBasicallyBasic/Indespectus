using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SyncHeadHands : MonoBehaviour
{
    public GameObject head;

    public GameObject lH;
    
    public GameObject rH;
    public GameObject headRef;

    public GameObject lHRef;
    
    public GameObject rHREf;

    public PhotonView PV;

    
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update() {
        if(PV.IsMine) {
            if(headRef.GetComponent<PhotonView>().IsMine) {
                if(lHRef.GetComponent<PhotonView>().IsMine) {
                    if(rHREf.GetComponent<PhotonView>().IsMine) {
                        PV.RPC("RPC_MoveMe", RpcTarget.All);
                    }
                }
            }
        }
    }

    void RPC_MoveMe() {
        head.transform.position = headRef.transform.position;
        lH.transform.position = lHRef.transform.position;
        rH.transform.position = rHREf.transform.position;
    }
}
