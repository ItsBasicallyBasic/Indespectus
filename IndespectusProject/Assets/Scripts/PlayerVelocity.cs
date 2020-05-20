﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerVelocity : MonoBehaviour
{

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

    public Material material;
    public Material material2;

    [SerializeField]
    private float lerpSpeed = 0.1f;

    public int scale = 1;

    // Start is called before the first frame update
    void Start()
    {
        // Change values from 0
        headPreviousPos = head.position;
        rightHandPreviousPos = rightHand.position;
        leftHandPreviousPos = leftHand.position;

        if (gameObject.tag != "Player")
        {
            //material = GetComponent<MeshRenderer>().material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag != "Player")
        {
            material.SetFloat("_Transition", velocity/scale);
            material2.SetFloat("_Transition", velocity/scale);
        }

        // Assign velocity based off previous frame position
        leftHandVelocity = Vector3.Distance(leftHand.position, leftHandPreviousPos) * 5000 * Time.deltaTime;
        rightHandVelocity = Vector3.Distance(rightHand.position, rightHandPreviousPos) * 5000 * Time.deltaTime;
        headVelocity = Vector3.Distance(head.position, headPreviousPos) * 5000 * Time.deltaTime;

        // Update previous frame with current frame position for next frame
        headPreviousPos = head.position;
        leftHandPreviousPos = leftHand.position;
        rightHandPreviousPos = rightHand.position;

        // Update player velocity with sensory output that was the highest
        if(leftHandVelocity > rightHandVelocity || leftHandVelocity > headVelocity)
        {
            desiredVelocity = leftHandVelocity;
        }
        else if (rightHandVelocity > leftHandVelocity || rightHandVelocity > headVelocity)
        {
            desiredVelocity = rightHandVelocity;
        }
        else if (headVelocity > rightHandVelocity || headVelocity > leftHandVelocity)
        {
            desiredVelocity = headVelocity;
        }
        // Else in the unlikely case that all sensory inputs are moving at the same speed
        else
        {
            desiredVelocity = headVelocity;
        }

        velocity = Mathf.Lerp(velocity, desiredVelocity, lerpSpeed);
        
        desiredVelocity = desiredVelocity/100;

        if (desiredVelocity > 1) desiredVelocity = 1;
        //if (desiredVelocity > 1) desiredVelocity = 1;
    }
}
