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
    [SerializeField] private GameObject[] playerUI;
    [SerializeField] private GameObject[] allPlayers;
    private bool allUI;
    private bool set;
    public int myNumber;

    PhotonView PV;


    // Start is called before the first frame update
    void Start() {
        PV = GetComponent<PhotonView>(); 
    }

    void Update() {
        if(playerUI.Length != PhotonNetwork.PlayerList.Length || (playerUI.Length == PhotonNetwork.PlayerList.Length && allUI)) {
            playerUI = GameObject.FindGameObjectsWithTag("playerUI");
            allUI = true;
            int numActive = 0;
            foreach(GameObject ui in playerUI) {
                if(ui.activeInHierarchy) {
                    numActive++;
                }
            }
            if(numActive != PhotonNetwork.PlayerList.Length) {
                allUI = false;
            }
            if (allUI) {
                foreach(GameObject ui in playerUI) {
                    if(!ui.transform.GetComponent<PhotonView>().IsMine){
                        ui.SetActive(false);
                    }
                }
            } 
        }
        if(!set) {
            AssignMaterials();
        }
    }

    private void AssignMaterials() {
        allPlayers = GameObject.FindGameObjectsWithTag("Player");
        if(allPlayers.Length == PhotonNetwork.PlayerList.Length) {
            for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++) {
                foreach(GameObject player in allPlayers) {
                    if(PhotonNetwork.PlayerList[i] == player.GetComponent<PhotonView>().Owner) {
                        player.GetComponent<PlayerVelocity>().myNumber = i;
                    }
                }
            }
            set = true;
        }
        PV.RPC("RPC_SetColor", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void RPC_SetColor() {
        if(myNumber== 0) {
            PlayerMain.GetComponent<Renderer>().material = p1Materials[0];
            Handle.GetComponent<Renderer>().material = p1Materials[1];
            foreach(GameObject weapon in Weapons) {
                weapon.GetComponent<Renderer>().material = p1Materials[2];
            }
        } else if(myNumber== 1) {
            PlayerMain.GetComponent<Renderer>().material = p2Materials[0];
            Handle.GetComponent<Renderer>().material = p2Materials[1];
            foreach(GameObject weapon in Weapons) {
                weapon.GetComponent<Renderer>().material = p2Materials[2];
            }
        } else if(myNumber== 2) {
            PlayerMain.GetComponent<Renderer>().material = p3Materials[0];
            Handle.GetComponent<Renderer>().material = p3Materials[1];
            foreach(GameObject weapon in Weapons) {
                weapon.GetComponent<Renderer>().material = p3Materials[2];
            }
        } else if(myNumber== 3) {
            PlayerMain.GetComponent<Renderer>().material = p4Materials[0];
            Handle.GetComponent<Renderer>().material = p4Materials[1];
            foreach(GameObject weapon in Weapons) {
                weapon.GetComponent<Renderer>().material = p4Materials[2];
            }
        }
    }
}
