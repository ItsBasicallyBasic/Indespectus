using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkingLobby : MonoBehaviourPunCallbacks {
  
    /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 4;

    /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    string gameVersion = "1";
    
    [SerializeField]
    private GameObject playButton;
    [SerializeField]
    private GameObject cancelButton;
    [SerializeField]
    private GameObject progressLabel;


    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    void Awake() {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their lev l automatically
        // PhotonNetwork.AutomaticallySyncScene = true;
    }

    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    void Start() {
        
        
    }

    public void Connect() {
        
        cancelButton.SetActive(true);
        playButton.SetActive(false);

        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Connect");
    }


    public override void OnConnectedToMaster() {
        Debug.Log("OnConnectedToMaster() was called by PUN");
        PhotonNetwork.AutomaticallySyncScene = true;
        playButton.SetActive(true);

    }

    public override void OnDisconnected(DisconnectCause cause) {
        Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);
        cancelButton.SetActive(false);
        playButton.SetActive(true);
    }

    public override void OnJoinedRoom() {
        Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room.");
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: CreateRoom");
        CreateRoom();
        // // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        // PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = maxPlayersPerRoom});
    }
    public override void OnCreateRoomFailed(short returnCode, string message) {
        Debug.Log("Failed to Create Room... Trying again");
        CreateRoom();
    }

    void CreateRoom() {
        Debug.Log("Creating Room");
        int randomRoomNumber = Random.Range(0, 1000);
        RoomOptions roomOptions = new RoomOptions () {IsVisible = true, IsOpen = true, MaxPlayers = (byte) maxPlayersPerRoom};
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOptions);
        Debug.Log(randomRoomNumber);
    }

    public void QCancel() {
        cancelButton.SetActive(false);
        playButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
