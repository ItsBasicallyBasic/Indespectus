using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class NetworkingPlayer : MonoBehaviour {
    
    private PhotonView PV;
    public GameObject playerAvatar;
    
    // Start is called before the first frame update
    void Start() {
        PV = GetComponent<PhotonView>();
        int spawnPicker = Random.Range(0, GameSetup.GS.spawnPoints.Length);
        if(PV.IsMine) {
            playerAvatar = PhotonNetwork.Instantiate(Path.Combine("NetworkPrefabs", "PlayerAvatar"), GameSetup.GS.spawnPoints[spawnPicker].position, GameSetup.GS.spawnPoints[spawnPicker].rotation, 0);
        }
    }

    // Update is called once per frame
    void Update() {
        
    }
}
