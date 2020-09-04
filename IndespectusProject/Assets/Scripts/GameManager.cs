using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour, IPunObservable {
    
    public static GameManager GM = null;

    public int PlayersSpawned { get; internal set; }
    public bool notNetworked;

    [SerializeField] internal Player[] players;

    [SerializeField] bool ready = false;
    [SerializeField] public int MAX_HEALTH;
    [SerializeField] public int MAX_ESSENCE;
    [SerializeField] public bool gameOver;

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
        maxDeaths = 2;
        print("i made " + players.Length + " players");
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
                // endGame();
            }
        }
    }

    [PunRPC]
    private void endGame() {
        // if(PhotonNetwork.IsMasterClient)
        gameOver = true;
        PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.endScene);
    }
    
    [PunRPC]
    internal void updateKDFromPlayer(int deadID, int killID) {
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
    //     if(stream.IsWriting) {
    //         Debug.Log("about to stream");
    //         foreach(Player p in players) {
    //             Debug.Log("Sent " + p.Kills + ", " + p.Deaths + ", " + p.Health + ", ");
    //             stream.SendNext(p.Kills);
    //             stream.SendNext(p.Deaths);
    //             stream.SendNext(p.Health);
    //         }
    //     } else {
    //         Debug.Log("about to receive");
    //         foreach(Player p in players) {
    //             int k = (int)stream.ReceiveNext();
    //             int d = (int)stream.ReceiveNext();
    //             int h = (int)stream.ReceiveNext();
    //             if(k > p.Kills) {
    //                 p.Kills = k;
    //             }
    //             if(d > p.Deaths) {
    //                 p.Deaths = d;
    //             }
    //             if(h > p.Health) {
    //                 p.Health = h;
    //             }
    //             Debug.Log("Rec'd & set to " + p.Kills + ", " + p.Deaths + ", " + p.Health + ", ");
    //         }
    //     }    
    }
}
