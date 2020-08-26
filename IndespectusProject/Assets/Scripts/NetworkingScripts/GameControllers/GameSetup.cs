using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameSetup : MonoBehaviour {
    
    public static GameSetup GS;

    public Transform[] spawnPoints;

    // Start is called before the first frame update
    private void OnEnable() {
        if(GameSetup.GS == null) {
            GameSetup.GS = this;
        }
        PhotonNetwork.SendRate = 50;
        PhotonNetwork.SerializationRate = 50;
    }

    // Update is called once per frame
    void Update() {
        
    }
}
    