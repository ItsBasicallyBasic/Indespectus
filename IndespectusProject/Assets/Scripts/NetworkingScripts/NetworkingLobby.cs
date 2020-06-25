using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkingLobby : MonoBehaviourPunCallbacks {
    
   
    /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
    [SerializeField]
    private byte maxPlayersPerRoom = 4;


    /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    string gameVersion = "1";
    
    [SerializeField] 
    private GameObject playButton;
    [SerializeField]
    private GameObject cancelButton;

   
    public void Connect() {
        
        playButton.SetActive(true);
        cancelButton.SetActive(false);

        // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.AutomaticallySyncScene = true;
        playButton.SetActive(true);
    }


    public override void OnDisconnected(DisconnectCause cause) {
        //
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        Debug.Log("No random room available, so we create one.\nCalling: CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        CreateRoom();
    }

    public override void OnJoinedRoom() {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
    }

    void CreateRoom() {
        Debug.Log("Creating a new Room");
        int randomRoomNumber = Random.Range(0, 10000);
        RoomOptions roomOptions  = new RoomOptions() {IsVisible = true, IsOpen = true, MaxPlayers = (byte) maxPlayersPerRoom};
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOptions);
        Debug.Log(randomRoomNumber);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        Debug.Log("Failed to create room... trying again");
        CreateRoom();
    }

    public void QCancel() {
        playButton.SetActive(true);
        cancelButton.SetActive(false);
        PhotonNetwork.LeaveRoom();
    }

}
