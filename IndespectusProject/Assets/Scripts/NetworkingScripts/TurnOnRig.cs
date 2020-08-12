using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TurnOnRig : MonoBehaviour {
    
    [SerializeField] PhotonView PV;
    [SerializeField] GameObject rig;

    // Start is called before the first frame update
    void Start() {
        if(PV.IsMine)
            rig.SetActive(true);
    }
}
