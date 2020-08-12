using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BackBtn : MonoBehaviour
{
    public void onBackBtnClick() {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.menuScene);
        
    }
}
