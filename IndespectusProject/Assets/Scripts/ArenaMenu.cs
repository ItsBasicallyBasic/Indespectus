using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArenaMenu : MonoBehaviourPunCallbacks {
    
    [SerializeField] GameObject arenaMenu;
    [SerializeField] GameObject Hands;
    [SerializeField] bool menuActive = false;
    [SerializeField] bool handsActive = false;
    Scene currentScene;
    void Start() {
        currentScene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update() {
        if((OVRInput.Get(OVRInput.RawButton.Start) || Input.GetKey(KeyCode.Escape)) && !menuActive) {
            menuActive = true;
            handsActive = true;
        }
        if((OVRInput.GetDown(OVRInput.Button.Two) || Input.GetKeyDown(KeyCode.Escape)) && menuActive) {
            menuActive = false;
            handsActive = false;
        }
        if((OVRInput.GetDown(OVRInput.Button.Four) || Input.GetKeyDown(KeyCode.Tilde)) && menuActive) {
            Application.Quit();
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene(2);
        }
        if(currentScene.buildIndex != 1){
            handsActive = true;
        } 
        arenaMenu.SetActive(menuActive);
        Hands.SetActive(handsActive);
    }

    public override void OnDisconnected(DisconnectCause cause) {
        base.OnDisconnected(cause);
        SceneManager.LoadScene(MultiplayerSettings.multiplayerSettings.endScene);
    }

}
