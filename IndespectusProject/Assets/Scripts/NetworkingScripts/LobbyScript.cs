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
    public GameObject backButton;
    public GameObject errorText;
    private int connectAttempts = 0;

    void Awake() {
        lobby = this;
    }
    // Start is called before the first frame update
    void Start() {
        PhotonNetwork.ConnectUsingSettings();
        connectButton.SetActive(false);

    }

    public override void OnConnectedToMaster() {
        Debug.Log("Player has connected to master server");
        PhotonNetwork.AutomaticallySyncScene = true;
        connectButton.SetActive(true);
        connectAttempts = 0;
    }

    public void OnRefreshButtonClicked() {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnConnectButtonClicked() {
        Debug.Log("Connecting...");
        connectButton.SetActive(false);
        refreshButton.SetActive(false);
        backButton.SetActive(false);
        PhotonNetwork.JoinRandomRoom();
        // cancelButton.SetActive(true);
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
        PhotonNetwork.LeaveRoom();
        cancelButton.SetActive(false);
        connectButton.SetActive(true);
        refreshButton.SetActive(true);
        backButton.SetActive(true);
    }

    // Update is called once per frame
    void Update() {
        if(PhotonNetwork.IsConnected && !connectButton.activeInHierarchy) {
            connectButton.SetActive(true);
        } else if(!PhotonNetwork.IsConnected && connectAttempts < 5) {
            PhotonNetwork.ConnectUsingSettings();
            connectAttempts++;
        }
        if(connectAttempts > 5) {
            errorText.SetActive(true);
        }
        if(Input.GetKey(KeyCode.Return) || OVRInput.GetDown(OVRInput.Button.One))  {
            OnConnectButtonClicked();
        }
        // if(Input.GetKey(KeyCode.Escape) || OVRInput.GetDown(OVRInput.Button.Two))  {
        //     OnCancelButtonClicked();
        // }
        if(Input.GetKey(KeyCode.R) || OVRInput.GetDown(OVRInput.Button.Three))  {
            OnRefreshButtonClicked();
        }
        
    }

}
