using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

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
        base.OnJoinedRoom();
        Debug.Log("Joined a room");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;
        PhotonNetwork.NickName = myNumberInRoom.ToString();

        if(MultiplayerSettings.multiplayerSettings.delayStart) {
            Debug.Log("displayer players in room out of max players posible (" + playersInRoom + ":" + MultiplayerSettings.multiplayerSettings.maxPlayers + ")");
            numPlayersText.SetText("Players in room out of max players posible (" + playersInRoom + ":" + MultiplayerSettings.multiplayerSettings.maxPlayers + ")");
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
        Debug.Log("A new player has jooined the room");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;
        if(MultiplayerSettings.multiplayerSettings.delayStart) {
            numPlayersText.SetText("Players in room out of max players posible (" + playersInRoom + ":" + MultiplayerSettings.multiplayerSettings.maxPlayers + ")");
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

    void StartGame() {
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
        Debug.Log("ive instantiated a network player");
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
            
            
            if(readyToCount || readyToStart) {
                countdownText.SetText("Starting in: " + (int)timeToStart);
            } else {countdownText.SetText("Game startting in: n/a");}
            if(!isGameLoaded) {
                if(readyToStart) {
                    atMaxPlayers -= Time.deltaTime;
                    lessThanMaxPlayers = atMaxPlayers;
                    timeToStart = atMaxPlayers;
                } else if(readyToCount) {
                    lessThanMaxPlayers -= Time.deltaTime;
                    timeToStart = lessThanMaxPlayers;
                }
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
            PV.RPC("RPC_CreatePlayer", RpcTarget.AllBuffered);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log(otherPlayer.NickName + " has left the game.");
        playersInGame--;
    }
}
