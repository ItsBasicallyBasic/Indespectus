using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour {
    
    public static GameManager GM = null;

    public int PlayersSpawned { get; internal set; }
    public bool notNetworked;

    [SerializeField] internal Player[] players;

    [SerializeField] bool ready = false;
    [SerializeField] public int MAX_HEALTH;
    [SerializeField] public int MAX_ESSENCE;
    [SerializeField] public bool gameOver;
    [SerializeField] public AudioManager audioManager;

    PhotonView PV;

    internal enum GameMode {
        MaxDeaths,
        Timed
    }
    
    [SerializeField] internal GameMode gameMode;
    [SerializeField] int maxDeaths;
    [SerializeField] internal float gameTime;

    void Awake() {
        // Singleton
        if(GM == null) { 
            GM = this;
        } else if(GM != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
        if(!PhotonNetwork.IsConnected) {
            notNetworked = true; 
        } else {
            notNetworked = false;
        }
        
    }

    // Start is called before the first frame update
    void Start() {
        if(PV == null) {PV = gameObject.GetComponent<PhotonView>();}
        players = new Player[] {
            new Player(),
            new Player(),
            new Player(),
            new Player()
        };
    }

    // Update is called once per frame
    void Update() {
        
        if(!gameOver) {
            switch(gameMode) {
                case GameMode.MaxDeaths : MaxDeaths();
                break;

                case GameMode.Timed : Timed();
                break;
            }
        }
        
        
    }

    private void Timed() {
        if(gameTime <= 0) {
            PV.RPC("endGame", RpcTarget.AllBuffered);
            endGame();
        } else {
            gameTime -= Time.deltaTime;
        }
    }

    private void MaxDeaths() {
        foreach(Player p in players) {
            if(p.Deaths >= maxDeaths) {
                PV.RPC("endGame", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    private void endGame() {
        gameOver = true;
        PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.endScene);
    }
    
    internal void updateKDFromPlayer(int deadID, int killID) {
        PV.RPC("RPC_UpdateKD", RpcTarget.AllBuffered, deadID, killID);
    }

    [PunRPC]
    private void RPC_UpdateKD(int deadID, int killID){
        players[deadID].Deaths++;
        players[killID].Kills++;

    }

    public class Player {
        public int ID { get; set; }
        public float Health { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }

        public Player(){;
            Health = 0;
            Kills = 0;
            Deaths = 0;
        }
    }
}
