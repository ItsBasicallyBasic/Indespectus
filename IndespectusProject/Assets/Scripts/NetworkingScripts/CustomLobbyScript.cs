using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class CustomLobbyScript : MonoBehaviourPunCallbacks, ILobbyCallbacks {
    
    public static CustomLobbyScript lobby;

    public string roomName;
    public int roomSize;
    public GameObject roomListingPrefab;
    public Transform roomsPanel;

    public List<RoomInfo> roomListings;
    public GameObject errorMsg;

    public TMP_Text nNplaceholder;

    void Awake() {
        lobby = this;
    }
    // Start is called before the first frame update
    void Start() {
        PhotonNetwork.ConnectUsingSettings();
        roomListings = new List<RoomInfo>();
    }

    public override void OnConnectedToMaster() {
        Debug.Log("Player has connected to master server");
        PhotonNetwork.AutomaticallySyncScene = true;
        if(PhotonNetwork.NickName == "")
            PhotonNetwork.NickName = "Player " + Random.Range(0, 1000);
        nNplaceholder.SetText(PhotonNetwork.NickName + "...");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        base.OnRoomListUpdate(roomList);
        // RemoveRoomListings();
        int tempIndex;

        foreach (RoomInfo room in roomList){
            if(roomListings != null) {
                tempIndex = roomListings.FindIndex(ByName(room.Name));
            } else {
                tempIndex = -1;
            }
            if(tempIndex != -1) {
                roomListings.RemoveAt(tempIndex);
                Destroy(roomsPanel.GetChild(tempIndex).gameObject);
            } else {
                roomListings.Add(room);
                ListRoom(room);
            }
            
        }
    }

    private static System.Predicate<RoomInfo> ByName(string name) {
        return delegate(RoomInfo room) {
            return room.Name == name;
        };
    }

    private void ListRoom(RoomInfo room) {
        if(room.IsOpen && room.IsVisible) {
            GameObject tempListing = Instantiate(roomListingPrefab, roomsPanel);
            RoomButton tempBtn = tempListing.GetComponent<RoomButton>();
            tempBtn.roomName = room.Name;
            tempBtn.roomSize = room.MaxPlayers;
            tempBtn.SetRoom();
        }
    }

    // private void RemoveRoomListings() {
    //     int i = 0;
    //     while(roomsPanel.childCount != 0) {
    //         Destroy(roomsPanel.GetChild(i).gameObject);
    //         i++;
    //     }
    // }



    public void CreateRoom() {
        Debug.Log("Creating new room...");
        errorMsg.SetActive(false);
        if(roomName.Length < 1) {
            roomName = "Room:"+ Random.Range(0, 10000);
        }
        RoomOptions roomOps = new RoomOptions() {
            IsVisible = true, 
            IsOpen = true, 
            MaxPlayers = (byte)roomSize
        };
        PhotonNetwork.CreateRoom(roomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        Debug.Log("tried to create new room but failed");
        errorMsg.SetActive(true);
        //CreateRoom();
    }

    public void OnRoomNameChanged(string nameIn) {
        roomName = nameIn;
    }
    public void OnNicknameChanged(string nameIn) {
        PhotonNetwork.NickName = nameIn;
    }

    public void OnSizeSelected(int sizeIn) {
        roomSize = sizeIn;
    }

    public void JoinLobbyOnClick() {
        if(!PhotonNetwork.InLobby) {
            PhotonNetwork.JoinLobby();
        }
    }

    // Update is called once per frame
    void Update() {
        
    }
}
