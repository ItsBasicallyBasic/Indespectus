﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro;

public class CustomRoomScript : MonoBehaviourPunCallbacks, IInRoomCallbacks {
    
    // Room info
    public static CustomRoomScript room;
    private PhotonView PV;

    public bool isGameLoaded;
    public int currentScene;

    // Player info
    Player[] photonPlayers;
    public int playersInRoom;
    public int myNumberInRoom;

    public int playersInGame;

    // Delayed Start
    private bool readyToCount;
    private bool readyToStart;
    public float startingTime;
    private float lessThanMaxPlayers;
    private float atMaxPlayers;
    private float timeToStart;

    public GameObject lobbyGO;
    public GameObject roomGO;
    public Transform playersPanel;
    public GameObject playerListingPrefab;
    public GameObject startBtn;


    
    private void Awake() {
        if(CustomRoomScript.room == null) {
            CustomRoomScript.room = this;
        } else {
            if(CustomRoomScript.room != this) {
                Destroy(CustomRoomScript.room.gameObject);
                CustomRoomScript.room = this;
            }         
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnEnable() {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable() {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    public override void OnJoinedRoom() {
        base.OnJoinedRoom();
        Debug.Log("Joined a room");

        lobbyGO.SetActive(false);
        roomGO.SetActive(true);
        if(PhotonNetwork.IsMasterClient) {
            startBtn.SetActive(true);
        }

        ClearPlayerListing();
        ListPlayers();

        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;
        

        if(MultiplayerSettings.multiplayerSettings.delayStart) {
            Debug.Log("displayer players in room out of max players posible (" + playersInRoom + ":" + MultiplayerSettings.multiplayerSettings.maxPlayers + ")");
            if(playersInRoom > 1) {
                readyToCount = true;
            }
            if(playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers) {
                readyToStart = true;
                if(!PhotonNetwork.IsMasterClient) {
                    return;
                }
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        } /*else {
            StartGame();            
        }*/
    }

    private void ClearPlayerListing() {
        
        for(int i = playersPanel.childCount -1; i >= 0; i--) {
            Destroy(playersPanel.GetChild(i).gameObject);
        }
    }
    private void ListPlayers() {
        if(PhotonNetwork.InRoom) {
            foreach(Player p in PhotonNetwork.PlayerList) {
                GameObject tempListing = Instantiate(playerListingPrefab, playersPanel);
                TMP_Text tempText = tempListing.transform.GetChild(0).GetComponent<TMP_Text>();
                tempText.SetText(p.NickName);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("A new player has jooined the room");

        ClearPlayerListing();
        ListPlayers();

        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;
        if(MultiplayerSettings.multiplayerSettings.delayStart) {
            Debug.Log("displayer players in room out of max players posible (" + playersInRoom + ":" + MultiplayerSettings.multiplayerSettings.maxPlayers + ")");
            if(playersInRoom > 1) {
                readyToCount = true;
            }
            if(playersInRoom == MultiplayerSettings.multiplayerSettings.maxPlayers) {
                readyToStart = true;
                if(!PhotonNetwork.IsMasterClient) {
                    return;
                }
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }

    public void StartGame() {
        isGameLoaded = true;
        if(!PhotonNetwork.IsMasterClient)
            return;
        if(MultiplayerSettings.multiplayerSettings.delayStart) {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.multiplayerScene);
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode) {
        currentScene = scene.buildIndex;
        if(currentScene == MultiplayerSettings.multiplayerSettings.multiplayerScene) {
            isGameLoaded = true;
            if(MultiplayerSettings.multiplayerSettings.delayStart) {
                PV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
            } else {
                RPC_CreatePlayer();
            }
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer() {
        PhotonNetwork.Instantiate(Path.Combine("NetworkPrefabs", "NetworkPlayer"), transform.position, Quaternion.identity, 0);
    }
    
    // Start is called before the first frame update
    void Start() {
        PV = GetComponent<PhotonView>();
        RestartTimer();
    }

    // Update is called once per frame
    void Update() {
        if(MultiplayerSettings.multiplayerSettings.delayStart) {
            if(playersInGame == 1) {
                RestartTimer();
            }
            if(!isGameLoaded) {
                if(readyToStart) {
                    atMaxPlayers -= Time.deltaTime;
                    lessThanMaxPlayers = atMaxPlayers;
                    timeToStart = atMaxPlayers;
                } else if(readyToCount) {
                    lessThanMaxPlayers -= Time.deltaTime;
                    timeToStart = lessThanMaxPlayers;
                }
                // Debug.Log("Display Time to start to the players " + timeToStart);
                if(timeToStart <= 0) {
                    StartGame();
                }
            }
        }
    }

    void RestartTimer() {
        lessThanMaxPlayers = startingTime;
        atMaxPlayers = 6;
        timeToStart = startingTime;
        readyToCount = false;
        readyToStart = false;
    }

    [PunRPC]
    void RPC_LoadedGameScene() {
        playersInGame++;
        if(playersInGame == PhotonNetwork.PlayerList.Length) {
            PV.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log(otherPlayer.NickName + " has left the game.");
        playersInGame--;
        ClearPlayerListing();
        ListPlayers();
    }

    public void OnCancelButtonClicked () {
        Debug.Log("Cancelling...");
        startBtn.SetActive(false);
        roomGO.SetActive(false);
        PhotonNetwork.LeaveRoom();
        lobbyGO.SetActive(true);
        
    }
}