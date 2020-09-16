using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackDoor : MonoBehaviourPunCallbacks {

    public void OnBackDoorClick() {
        RoomScript.room.playersInRoom++;
        RoomScript.room.StartGame();
        Debug.Log("starting backdoor");
    }


}
