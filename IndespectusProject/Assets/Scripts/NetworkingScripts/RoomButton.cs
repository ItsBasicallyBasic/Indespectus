using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour {
    
    public TMP_Text nameText;
    public TMP_Text sizeText;
    
    public int roomSize;
    public string roomName;

    public void SetRoom() {
        nameText.SetText(roomName);
        sizeText.SetText(roomSize.ToString());
    }

    public void JoinRoomClick() {
        PhotonNetwork.JoinRoom(roomName);
    }


}
