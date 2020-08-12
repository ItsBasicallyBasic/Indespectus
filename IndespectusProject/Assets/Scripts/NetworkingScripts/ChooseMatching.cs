using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ChooseMatching : MonoBehaviourPunCallbacks {
    
    public GameObject auto;
    public GameObject custom;
    public GameObject choice;
    public GameObject backBtn;

    public void AutoOnClick() {
        auto.SetActive(true);
        backBtn.SetActive(true);
        choice.SetActive(false);
    }

    public void CustomOnClick() {
        custom.SetActive(true);
        backBtn.SetActive(true);
        choice.SetActive(false);
    }

    public void BackOnClick() {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause) {
        auto.SetActive(false);
        custom.SetActive(false);
        backBtn.SetActive(false);
        choice.SetActive(true);
    }

}
