using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SendData : MonoBehaviour {
    
    [SerializeField] private PhotonView PV;

    private void Awake() {
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update() {
        PV.RPC("RPC_MaterialUPdate", RpcTarget.All);
        // PV.RPC("", RpcTarget.All);
        // PV.RPC("", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_MaterialUPdate() {
            
    }
}
