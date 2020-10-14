using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class LobbyScript : MonoBehaviourPunCallbacks {
    
    public static LobbyScript lobby;
    public Button connectButton;
    public GameObject cancelButton;
    public GameObject refreshButton;
    public GameObject loadingButton;
    public GameObject roomPannel;
    public GameObject lobbyPannel;
    public Button findBtn;
    public GameObject errorText;
    private int connectAttempts = 0;
    private float findFail = 0;
    
    [SerializeField]
    private TMP_Text findTimer;
    
    [SerializeField]
    private GameObject findFailTxt;

    public TMP_Text connectionStatus;
    
    string connected = "Connected to Master Server";
    string disconnected = "Not connected to Master Server";

    void Awake() {
        lobby = this;
        connectionStatus.text = disconnected;
    }
    // Start is called before the first frame update
    void Start() {
        PhotonNetwork.ConnectUsingSettings();
        
        findBtn.interactable = false;
        connectButton.interactable = false;

    }

    public override void OnConnectedToMaster() {
        Debug.Log("Player has connected to master server");
        connectionStatus.text = connected;
        PhotonNetwork.AutomaticallySyncScene = true;
        connectAttempts = 0;
        findBtn.interactable = true;
        connectButton.interactable = true;
        findFail = -1;
    }

    public void OnRefreshButtonClicked() {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnCreateButtonClicked() {
        Debug.Log("Connecting...");
        // connectButton.SetActive(false);
        // refreshButton.SetActive(false);
        // // loadingButton.SetActive(false);
        // PhotonNetwork.JoinRandomRoom();
        // cancelButton.SetActive(true);
        CreateRoom();
        findFailTxt.SetActive(false);
    }
    
    public void OnFindRoomButtonClicked() {
        PhotonNetwork.JoinRandomRoom();
        findBtn.interactable = false;
        findFail = 5;
        findFailTxt.SetActive(false);
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        Debug.Log("tried to join random room but failed");
        findTimer.text = "" + (int) findFail;
        if(findFail >= 0) {
            PhotonNetwork.JoinRandomRoom();
            Debug.Log("trying again");
        } else {
            findFail = -1;
            findBtn.interactable = true;
            Debug.Log("Found no rooms");
            findFailTxt.SetActive(true);
        }
        // CreateRoom();
    }

    void CreateRoom() {
        Debug.Log("Creating new room...");
        int rRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() {
            IsVisible = true, 
            IsOpen = true, 
            MaxPlayers = (byte)MultiplayerSettings.multiplayerSettings.maxPlayers
        };
        PhotonNetwork.CreateRoom("" + rRoomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        // Debug.Log("tried to create new room but failed");
        CreateRoom();
    }

    public void OnCancelButtonClicked () {
        // Debug.Log("Cancelling...");
        PhotonNetwork.LeaveRoom();
        // cancelButton.SetActive(false);
        // connectButton.SetActive(true);
        // refreshButton.SetActive(true);
        // loadingButton.SetActive(false);
        roomPannel.SetActive(false);
        lobbyPannel.SetActive(true);
    }

    // Update is called once per frame
    void Update() {
        if(!PhotonNetwork.IsConnected && connectAttempts < 5) {
            PhotonNetwork.ConnectUsingSettings();
            connectAttempts++;
            // connectButton.SetActive(false);
            // loadingButton.SetActive(true);
            connectButton.interactable = false;
            findBtn.interactable = false;
        } else {
            loadingButton.SetActive(false);
        }
        if(connectAttempts > 5) {
            errorText.SetActive(true);
        }
        
        if(findFail != -1) {
            findFail -= Time.deltaTime;
        } else {
            findTimer.text = "";
        }
        // if(Input.GetKey(KeyCode.Return) || OVRInput.GetDown(OVRInput.Button.One))  {
        //     OnConnectButtonClicked();
        // }
        // if(Input.GetKey(KeyCode.Escape) || OVRInput.GetDown(OVRInput.Button.Two))  {
        //     OnCancelButtonClicked();
        // }
        // if(Input.GetKey(KeyCode.R) || OVRInput.GetDown(OVRInput.Button.Three))  {
        //     OnRefreshButtonClicked();
        // }
        
    }

}
