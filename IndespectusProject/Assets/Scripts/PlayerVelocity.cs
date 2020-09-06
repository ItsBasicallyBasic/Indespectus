using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
// using Valve.VR;
// using Valve.VR.InteractionSystem;

public class PlayerVelocity : MonoBehaviour/*PunCallbacks, IPunObservable*/ {

    public float velocity;
    public float desiredVelocity;

    private float headVelocity;
    private float leftHandVelocity;
    private float rightHandVelocity;

    private Vector3 headPreviousPos;
    private Vector3 rightHandPreviousPos;
    private Vector3 leftHandPreviousPos;

    public Transform head;
    public Transform rightHand;
    public Transform leftHand;
    public Material[] myMats;

    [SerializeField]
    private PhotonView PV;

    // [SerializeField]
    // private SetMaterials mySM;

    [SerializeField]
    private float lerpSpeed = 3f;

    public int scale = 10;

    [SerializeField] bool set = false;
    private float networkVelocity;

    public int myNumber;
    
    [SerializeField] private GameObject[] allPlayers;

    void Start()
    {
        // Change values from 0
        headPreviousPos = head.position;
        rightHandPreviousPos = rightHand.position;
        leftHandPreviousPos = leftHand.position;
        
    }

    #region  SettingMaterials
    [SerializeField] public Material[] p1Materials;
    [SerializeField] public Material[] p2Materials;
    [SerializeField] public Material[] p3Materials;
    [SerializeField] public Material[] p4Materials;
    [SerializeField] private GameObject PlayerMain;
    [SerializeField] private GameObject Handle;
    [SerializeField] private GameObject[] Weapons;
    private bool allUI;
    private GameObject[] playerUI;
    #endregion

    // Update is called once per frame
    void Update() {
        if(!set && !allUI) {
            allPlayers = GameObject.FindGameObjectsWithTag("Player");
            if(allPlayers.Length == PhotonNetwork.PlayerList.Length) {
                for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++) {
                    foreach(GameObject player in allPlayers) {
                        if(PhotonNetwork.PlayerList[i] == player.GetComponent<PhotonView>().Owner) {
                            player.GetComponent<PlayerVelocity>().myNumber = i;
                        }
                    }
                }

                if (myNumber == 0) {
                    myMats = p1Materials;
                } else if (myNumber == 1) {
                    myMats = p2Materials;
                } else if (myNumber == 2) {
                    myMats = p3Materials;
                } else if (myNumber == 3) {
                    myMats = p4Materials;
                }
                PlayerMain.GetComponent<Renderer>().material = myMats[1];
                Handle.GetComponent<Renderer>().material = myMats[0];
                foreach(GameObject weapon in Weapons) {
                    weapon.GetComponent<Renderer>().material = myMats[2];
                }
                set = true;
            }

            int numActive = 0;
            playerUI = GameObject.FindGameObjectsWithTag("playerUI");
            foreach(GameObject ui in playerUI) {
                if(ui.activeInHierarchy) {
                    numActive++;
                }
            }
            if(numActive != PhotonNetwork.PlayerList.Length) {
                allUI = false;
            }
            if (allUI) {
                foreach(GameObject ui in playerUI) {
                    if(!ui.transform.GetComponent<PhotonView>().IsMine){
                        ui.SetActive(false);
                    }
                }
            } 
        } else {
        
            TransitionMaterials();

            SetVelocity();
        }
    }
    void TransitionMaterials() {
        foreach(Material mat in myMats) {
            mat.SetFloat("_Transition", velocity/scale);
        }
    }

    void SetVelocity()
    {
        // Assign velocity based off previous frame position
        leftHandVelocity = Vector3.Distance(leftHand.position, leftHandPreviousPos) * 7000 * Time.deltaTime;
        rightHandVelocity = Vector3.Distance(rightHand.position, rightHandPreviousPos) * 7000 * Time.deltaTime;
        headVelocity = Vector3.Distance(head.position, headPreviousPos) * 7000 * Time.deltaTime;

        // Update previous frame with current frame position for next frame
        headPreviousPos = head.position;
        leftHandPreviousPos = leftHand.position;
        rightHandPreviousPos = rightHand.position;

        // Update player velocity with sensory output that was the highest
        if (leftHandVelocity > rightHandVelocity && leftHandVelocity > headVelocity)
        {
            desiredVelocity = leftHandVelocity;
        }
        else if (rightHandVelocity > leftHandVelocity && rightHandVelocity > headVelocity)
        {
            desiredVelocity = rightHandVelocity;
        }
        else if (headVelocity > rightHandVelocity && headVelocity > leftHandVelocity)
        {
            desiredVelocity = headVelocity;
        }
        // Else in the unlikely case that all sensory inputs are moving at the same speed
        else
        {
            desiredVelocity = headVelocity;
        }

        velocity = Mathf.Lerp(velocity, desiredVelocity, lerpSpeed * Time.deltaTime);

        //desiredVelocity = desiredVelocity/100;
        desiredVelocity = Mathf.Clamp(desiredVelocity, 0, 1.5f);
        velocity = Mathf.Clamp(velocity, 0, 1.5f);
    }

    public void OverrideVelocity(float v)
    {
        desiredVelocity = v;
        velocity = v;
    }

    public float GetRightHandVelocity()
    {
        return rightHandVelocity;
    }
}
