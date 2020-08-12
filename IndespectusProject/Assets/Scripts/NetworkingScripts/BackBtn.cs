using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackBtn : MonoBehaviour
{
    public void onBackBtnClick() {
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad() {
        PhotonNetwork.LeaveRoom();
        while(PhotonNetwork.InRoom)
            yield return null;
        SceneManager.LoadScene(MultiplayerSettings.multiplayerSettings.menuScene);
    }
}
