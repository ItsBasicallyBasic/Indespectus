using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class RoomScript : MonoBehaviourPunCallbacks, IInRoomCallbacks {
    
    // Room info
    public static RoomScript room;
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

    
    public TMP_Text numPlayersText;
    public TMP_Text countdownText;

    public TMP_Text connectionState;
    string connected = "Connected to Master Server";
    string disconnected = "Not connected to Master Server";

    public Button StartBtn;
    private GameObject roomPannel;
    private GameObject lobbyPannel;

    private void Awake() {
        if(RoomScript.room == null) {
            RoomScript.room = this;
        } else {
            if(RoomScript.room != this) {
                Destroy(RoomScript.room.gameObject);
                RoomScript.room = this;
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
        roomPannel.SetActive(false);
        lobbyPannel.SetActive(true);
        base.OnJoinedRoom();
        Debug.Log("Joined a room");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;
        PhotonNetwork.NickName = myNumberInRoom.ToString();

        if(MultiplayerSettings.multiplayerSettings.delayStart) {
            numPlayersText.SetText("Players in room out of max players possible (" + playersInRoom + "/" + MultiplayerSettings.multiplayerSettings.maxPlayers + ")");
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
        } else {
            StartGame();            
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        base.OnPlayerEnteredRoom(newPlayer);
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;
        if(MultiplayerSettings.multiplayerSettings.delayStart) {
            numPlayersText.SetText("Players in room out of max players possible (" + playersInRoom + "/" + MultiplayerSettings.multiplayerSettings.maxPlayers + ")");
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
            //    RPC_CreatePlayer();
            }
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer() {
        PhotonNetwork.Instantiate(Path.Combine("NetworkPrefabs", "NetworkPlayer"), transform.position, Quaternion.identity, 0);
        Destroy(this.gameObject);
    }
    
    // Start is called before the first frame update
    void Start() {
        PV = GetComponent<PhotonView>();
        RestartTimer();
    }

    // Update is called once per frame
    void Update() {
        if(currentScene == 0) {
            if(MultiplayerSettings.multiplayerSettings.delayStart) {
                if(playersInGame == 1) {
                    // RestartTimer();
                    StartBtn.interactable = false;
                } 
                
                
                if(readyToCount || readyToStart) {
                    StartBtn.interactable = true;
                } else {StartBtn.interactable = false;}
                // if(!isGameLoaded) {
                //     if(readyToStart) {
                //         atMaxPlayers -= Time.deltaTime;
                //         lessThanMaxPlayers = atMaxPlayers;
                //         timeToStart = atMaxPlayers;
                //     } else if(readyToCount) {
                //         lessThanMaxPlayers -= Time.deltaTime;
                //         timeToStart = lessThanMaxPlayers;
                //     }
                //     if(timeToStart <= 0) {
                //         StartGame();
                //     }
                // }
            }
            // if(!PhotonNetwork.InRoom) {
            //     numPlayersText.SetText("Players in room out of max players possible (n/a), not in room");
            // }
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
            PV.RPC("RPC_CreatePlayer", RpcTarget.AllBuffered);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        base.OnPlayerLeftRoom(otherPlayer);
        playersInRoom--;
        numPlayersText.SetText("Players in room out of max players possible (" + playersInRoom + "/" + MultiplayerSettings.multiplayerSettings.maxPlayers + ")");
    }
}
