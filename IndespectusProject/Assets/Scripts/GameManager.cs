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
    public int MAX_HEALTH { get; internal set; }
    public int MAX_ESSENCE { get; internal set; }
    public bool gameOver { get; private set; }

    PhotonView PV;

    enum GameMode {
        MaxDeaths,
        Timed
    }
    
    [SerializeField] GameMode gameMode;
    [SerializeField] int maxDeaths;
    [SerializeField] float gameTime;

    void Awake() {
        // Singleton
        if(GM == null) { 
            GM = this;
        } else if(GM != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
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
        if(!ready) {
            if (PlayersSpawned == PhotonNetwork.PlayerList.Length) {
                ready = true;
                
                for (int i = 0; i < PlayersSpawned; i++) {
                    // players[i] = new Player();
                    players[i].ID = i;
                }
            } else if (notNetworked) {
                ready = true;
            } else { return; }
        }
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
            // PV.RPC("endGame", RpcTarget.AllBuffered);
            endGame();
        } else {
            gameTime -= Time.deltaTime;
        }
    }

    private void MaxDeaths() {
        foreach(Player p in players) {
            if(p.Deaths >= maxDeaths) {
                // PV.RPC("endGame", RpcTarget.AllBuffered);
                endGame();
            }
        }
    }

    [PunRPC]
    private void endGame() {
        // if(PhotonNetwork.IsMasterClient)
        gameOver = true;
        PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.endScene);
    }

    internal class Player {
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
