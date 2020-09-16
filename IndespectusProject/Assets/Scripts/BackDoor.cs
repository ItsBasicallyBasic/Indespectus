using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackDoor : MonoBehaviourPunCallbacks {
    
    GameManager GM;
    public void OnBackDoorClick() {
        GameManager.GM.gameMode = GameManager.GameMode.Timed;
        GameManager.GM.gameTime = 86400;
        SceneManager.LoadScene(1);
    }


}
