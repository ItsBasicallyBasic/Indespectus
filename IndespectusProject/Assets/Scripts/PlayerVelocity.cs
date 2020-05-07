using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVelocity : MonoBehaviour
{

    public float velocity;

    private float headVelocity;
    private float leftHandVelocity;
    private float rightHandVelocity;

    private Vector3 headPreviousPos;
    private Vector3 rightHandPreviousPos;
    private Vector3 leftHandPreviousPos;

    public Transform head;
    public Transform rightHand;
    public Transform leftHand;

    // Start is called before the first frame update
    void Start()
    {
        // Change values from 0
        headPreviousPos = head.position;
        rightHandPreviousPos = rightHand.position;
        leftHandPreviousPos = leftHand.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Assign velocity based off previous frame position
        leftHandVelocity = Vector3.Distance(leftHand.position, leftHandPreviousPos) * Time.deltaTime;
        rightHandVelocity = Vector3.Distance(rightHand.position, rightHandPreviousPos) * Time.deltaTime;
        headVelocity = Vector3.Distance(head.position, headPreviousPos) * Time.deltaTime;

        // Update previous frame with current frame position for next frame
        headPreviousPos = head.position;
        leftHandPreviousPos = leftHand.position;
        rightHandPreviousPos = rightHand.position;

        // Update player velocity with sensory output that was the highest
        if(leftHandVelocity > rightHandVelocity || leftHandVelocity > headVelocity)
        {
            velocity = leftHandVelocity;
        }
        else if (rightHandVelocity > leftHandVelocity || rightHandVelocity > headVelocity)
        {
            velocity = rightHandVelocity;
        }
        else if (headVelocity > rightHandVelocity || headVelocity > leftHandVelocity)
        {
            velocity = headVelocity;
        }
        // Else in the unlikely case that all sensory inputs are moving at the same speed
        else
        {
            velocity = headVelocity;
        }
    }
}
