using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CheckNetworked : MonoBehaviour {
    
    [SerializeField] GameObject NonNetworked;
    [SerializeField] GameObject Networked;
    public bool networked;

    void Awake() {
        networked = PhotonNetwork.IsConnected;
        if(networked) {
            Instantiate(Networked);
        } else {
            Instantiate(NonNetworked);
        }
    }
}
