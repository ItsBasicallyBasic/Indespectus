using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class SetMaterials : MonoBehaviour {
    
    [SerializeField] public Material[] p1Materials;
    [SerializeField] public Material[] p2Materials;
    [SerializeField] public Material[] p3Materials;
    [SerializeField] public Material[] p4Materials;
    [SerializeField] private GameObject PlayerMain;
    [SerializeField] private GameObject Handle;
    [SerializeField] private GameObject[] Weapons;

    public int myNumber;

    PhotonView PV;


    // Start is called before the first frame update
    void Start() {
        PV = GetComponent<PhotonView>(); 
        AssignMaterials();
    }

    private void AssignMaterials() {
        if(this.gameObject.tag != "Enemy") {
            if(PV.IsMine){
                for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++) {
                    if(PhotonNetwork.PlayerList[i].IsLocal) {
                        PV.RPC("RPC_SetColor", RpcTarget.AllBuffered, i);
                        myNumber = i;
                    }
                }
            }
        } else {
            RPC_SetColor (1);
        }
    }

    [PunRPC]
    void RPC_SetColor(int i) {
        if(i== 0) {
            PlayerMain.GetComponent<Renderer>().material = p1Materials[0];
            Handle.GetComponent<Renderer>().material = p1Materials[1];
            foreach(GameObject weapon in Weapons) {
                weapon.GetComponent<Renderer>().material = p4Materials[2];
            }
        } else if(i== 1) {
            PlayerMain.GetComponent<Renderer>().material = p2Materials[0];
            Handle.GetComponent<Renderer>().material = p2Materials[1];
            foreach(GameObject weapon in Weapons) {
                weapon.GetComponent<Renderer>().material = p4Materials[2];
            }
        } else if(i== 2) {
            PlayerMain.GetComponent<Renderer>().material = p3Materials[0];
            Handle.GetComponent<Renderer>().material = p3Materials[1];
            foreach(GameObject weapon in Weapons) {
                weapon.GetComponent<Renderer>().material = p4Materials[2];
            }
        } else if(i== 3) {
            PlayerMain.GetComponent<Renderer>().material = p4Materials[0];
            Handle.GetComponent<Renderer>().material = p4Materials[1];
            foreach(GameObject weapon in Weapons) {
                weapon.GetComponent<Renderer>().material = p4Materials[2];
            }
        }
    }
}
