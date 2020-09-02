using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using static GameManager;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class EndScreen : MonoBehaviourPunCallbacks  {
    
    [SerializeField] private TMP_Text yourNum;
    [SerializeField] private TMP_Text[] PlayerResults;
    private PhotonView PV;

    [SerializeField] GameObject leaving;
    [SerializeField] GameObject results;

    private string winner =  "";

    // Start is called before the first frame update
    void Start() {
        if(PV == null) {PV = gameObject.GetComponent<PhotonView>();}
        // if(PV.IsMine){
            int j = -1;
            for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++) {
                if(PhotonNetwork.PlayerList[i].IsLocal) {
                    yourNum.SetText("You were Player " + (i + 1));
                }
                if(j < GameManager.GM.players[i].Kills) {
                    j = i;
                }
            }
        // }
        for(int i = 0; i < 4; i++){
            if(i == j) {
                winner = "Winner";
            } else {
                winner = "";
            }
            if(i < PhotonNetwork.PlayerList.Length) {
                PlayerResults[i].SetText("Player " + (i + 1) + " " + GameManager.GM.players[i].Kills  + " | " + GameManager.GM.players[i].Deaths + " | " + winner);
            } else {
                PlayerResults[i].SetText("");
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if(OVRInput.GetDown(OVRInput.Button.One) || Input.anyKey) {
            results.SetActive(false);
            leaving.SetActive(true);
            var obj = GameObject.Find ("GameManager");
            Destroy(obj);
            PhotonNetwork.Disconnect();
            // SceneManager.LoadScene(MultiplayerSettings.multiplayerSettings.menuScene);
            //PhotonNetwork.LeaveRoom();
        }
    }
    
    public override void OnDisconnected	(DisconnectCause cause)	{
        SceneManager.LoadScene(MultiplayerSettings.multiplayerSettings.menuScene);
    }

}
