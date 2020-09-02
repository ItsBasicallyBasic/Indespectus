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
    [SerializeField] int MAX_HEALTH = 0;
    [SerializeField] int MAX_ESSCENCE = 0;

    [SerializeField] float gameTime;

    enum GameMode {
        OneDeath,
        Timed
    }
    
    [SerializeField] GameMode gameMode;
    public int maxDeaths;

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
        
    }

    // Update is called once per frame
    void Update() {
        if(!ready) {
            if (PlayersSpawned == PhotonNetwork.PlayerList.Length) {
                ready = true;
                players = new Player[PlayersSpawned];
                for (int i = 0; i < PlayersSpawned; i++)
                {
                    players[i] = new Player()
                    {
                        ID = i,
                        Health = MAX_HEALTH,
                        Kills = 0,
                        Deaths = 0
                    };
                }
            } else if (notNetworked) {
                ready = true;
            } else { return; }
        }

        switch(gameMode) {
            case GameMode.OneDeath : OneDeath();
            break;

            case GameMode.Timed : Timed();
            break;
        }
        
    }

    private void Timed() {
        if(gameTime <= 0) {
            endGame();
        } else {
            gameTime -= Time.deltaTime;
        }
    }

    private void OneDeath() {
        foreach(Player p in players) {
            if(p.Deaths >= maxDeaths) {
                endGame();
            }
        }
    }

    private void endGame() {
        if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.endScene);
    }

    internal class Player {
        public int ID { get; set; }
        public float Health { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
    }
}
