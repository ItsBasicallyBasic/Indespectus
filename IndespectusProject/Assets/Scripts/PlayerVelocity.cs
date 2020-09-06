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

    [SerializeField]
    private SetMaterials mySM;

    [SerializeField]
    private float lerpSpeed = 3f;

    public int scale = 10;

    [SerializeField] bool NonNetworked;
    private float networkVelocity;

    public int myNumber;

    void Start()
    {
        // Change values from 0
        headPreviousPos = head.position;
        rightHandPreviousPos = rightHand.position;
        leftHandPreviousPos = leftHand.position;
        
        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++) { 
            if(PhotonNetwork.PlayerList[i] == this.GetComponent<PhotonView>().Owner) {
                myNumber = i;
            }
        }

        if (myNumber == 0) {
            myMats = mySM.p1Materials;
        } else if (myNumber == 1) {
            myMats = mySM.p2Materials;
        } else if (myNumber == 2) {
            myMats = mySM.p3Materials;
        } else if (myNumber == 3) {
            myMats = mySM.p4Materials;
        }
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
    //    if(stream.IsWriting) {
    //        stream.SendNext(velocity);
    //    } else {
    //        /*networkVelocity*/ velocity = (float)stream.ReceiveNext();
    //    }    
    //}

    // Update is called once per frame
    void Update() {
        // if(velocity != networkVelocity) {
        //     velocity = Mathf.Lerp(velocity, networkVelocity, lerpSpeed);
        // }
        // if(velocity != networkVelocity) {
        //    velocity = Mathf.Lerp(velocity, desiredVelocity, lerpSpeed);
        // }
        
        TransitionMaterials();

        SetVelocity();
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
