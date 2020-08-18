using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class NetworkingPlayer : MonoBehaviourPunCallbacks {
    
    private PhotonView PV;
    public GameObject playerAvatar;
    
    // Start is called before the first frame update
    void Start() {
        PV = GetComponent<PhotonView>();
        int spawnPicker = -1;
        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++) {
            if(PhotonNetwork.PlayerList[i].IsLocal) {
                // PV.RPC("RPC_SetColor", RpcTarget.AllBuffered, i);
                spawnPicker = i;
            }
        }
        Debug.Log("i am about to spawn a player avatar for player" + spawnPicker);
        if(PV.IsMine) {
            PV.RPC("RPC_SpawnAvatar", RpcTarget.AllBuffered, spawnPicker);
            Debug.Log("ällbuffered");
            // PV.RPC("RPC_SpawnAvatar", RpcTarget.AllBuffered, spawnPicker);
            // Debug.Log("all (not buffered");
        }
    }

    [PunRPC]
    void RPC_SpawnAvatar(int i) {
        playerAvatar = PhotonNetwork.Instantiate(Path.Combine("NetworkPrefabs", "PlayerAvatar"), GameSetup.GS.spawnPoints[i].position, GameSetup.GS.spawnPoints[i].rotation, 0);
        Debug.Log("ive instantiated player avatar for player" + i);
    }

    // public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) {

    // }


    // Update is called once per frame
    void Update() {
        
    }
}
