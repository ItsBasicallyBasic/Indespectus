using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour {
    
    public static GameSetup GS;

    public Transform[] spawnPoints;

    // Start is called before the first frame update
    void Start() {
        if(GameSetup.GS == null) {
            GameSetup.GS = this;
        }
    }

    // Update is called once per frame
    void Update() {
        
    }
}
