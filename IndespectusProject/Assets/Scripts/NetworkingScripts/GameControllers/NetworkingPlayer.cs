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
                PV.RPC("RPC_SetColor", RpcTarget.All, i);
                spawnPicker = i;
            }
        }
        if(PV.IsMine) {
            playerAvatar = PhotonNetwork.Instantiate(Path.Combine("NetworkPrefabs", "PlayerAvatar"), GameSetup.GS.spawnPoints[spawnPicker].position, GameSetup.GS.spawnPoints[spawnPicker].rotation, 0);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) {

    }


    // Update is called once per frame
    void Update() {
        
    }
}
