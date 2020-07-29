using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyScript : MonoBehaviourPunCallbacks {
    
    public static LobbyScript lobby;
    public GameObject connectButton;
    public GameObject cancelButton;
    public GameObject refreshButton;

    void Awake() {
        lobby = this;
    }
    // Start is called before the first frame update
    void Start() {
        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster() {
        Debug.Log("Player has connected to master server");
        PhotonNetwork.AutomaticallySyncScene = true;
        refreshButton.SetActive(false);
        connectButton.SetActive(true);
    }

    public void OnRefreshButtonClicked() {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnConnectButtonClicked() {
        Debug.Log("Connecting...");
        connectButton.SetActive(false);
        PhotonNetwork.JoinRandomRoom();
        cancelButton.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        Debug.Log("tried to join random room but failed");
        CreateRoom();
    }

    void CreateRoom() {
        Debug.Log("Creating new room...");
        int rRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() {
            IsVisible = true, 
            IsOpen = true, 
            MaxPlayers = (byte)MultiplayerSettings.multiplayerSettings.maxPlayers
        };
        PhotonNetwork.CreateRoom("Room" + rRoomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        Debug.Log("tried to create new room but failed");
        CreateRoom();
    }

    public void OnCancelButtonClicked () {
        Debug.Log("Cancelling...");
        cancelButton.SetActive(false);
        PhotonNetwork.LeaveRoom();
        connectButton.SetActive(true);
    }

    // Update is called once per frame
    void Update() {
        
    }
}
