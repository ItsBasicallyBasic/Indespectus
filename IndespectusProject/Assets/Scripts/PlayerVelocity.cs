using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< Updated upstream
=======
using Valve.VR;
using Valve.VR.InteractionSystem;
>>>>>>> Stashed changes

public class PlayerVelocity : MonoBehaviour
{

    public float velocity;
<<<<<<< Updated upstream
=======
    public float desiredVelocity;
>>>>>>> Stashed changes

    private float headVelocity;
    private float leftHandVelocity;
    private float rightHandVelocity;

    private Vector3 headPreviousPos;
    private Vector3 rightHandPreviousPos;
    private Vector3 leftHandPreviousPos;

    public Transform head;
    public Transform rightHand;
    public Transform leftHand;

<<<<<<< Updated upstream
=======
    public Material material;
    public Material material2;

    private float lerpSpeed = 0.1f;

>>>>>>> Stashed changes
    // Start is called before the first frame update
    void Start()
    {
        // Change values from 0
        headPreviousPos = head.position;
        rightHandPreviousPos = rightHand.position;
        leftHandPreviousPos = leftHand.position;
<<<<<<< Updated upstream
=======

        if (gameObject.tag != "Player")
        {
            //material = GetComponent<MeshRenderer>().material;
        }
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream
        // Assign velocity based off previous frame position
        leftHandVelocity = Vector3.Distance(leftHand.position, leftHandPreviousPos) * Time.deltaTime;
        rightHandVelocity = Vector3.Distance(rightHand.position, rightHandPreviousPos) * Time.deltaTime;
        headVelocity = Vector3.Distance(head.position, headPreviousPos) * Time.deltaTime;
=======
        if(gameObject.tag != "Player")
        {
            material.SetFloat("_Transition", velocity);
            material2.SetFloat("_Transition", velocity);
        }

        // Assign velocity based off previous frame position
        leftHandVelocity = Vector3.Distance(leftHand.position, leftHandPreviousPos) * 5000 * Time.deltaTime;
        rightHandVelocity = Vector3.Distance(rightHand.position, rightHandPreviousPos) * 5000 * Time.deltaTime;
        headVelocity = Vector3.Distance(head.position, headPreviousPos) * 5000 * Time.deltaTime;
>>>>>>> Stashed changes

        // Update previous frame with current frame position for next frame
        headPreviousPos = head.position;
        leftHandPreviousPos = leftHand.position;
        rightHandPreviousPos = rightHand.position;

        // Update player velocity with sensory output that was the highest
        if(leftHandVelocity > rightHandVelocity || leftHandVelocity > headVelocity)
        {
<<<<<<< Updated upstream
            velocity = leftHandVelocity;
        }
        else if (rightHandVelocity > leftHandVelocity || rightHandVelocity > headVelocity)
        {
            velocity = rightHandVelocity;
        }
        else if (headVelocity > rightHandVelocity || headVelocity > leftHandVelocity)
        {
            velocity = headVelocity;
=======
            desiredVelocity = leftHandVelocity;
        }
        else if (rightHandVelocity > leftHandVelocity || rightHandVelocity > headVelocity)
        {
            desiredVelocity = rightHandVelocity;
        }
        else if (headVelocity > rightHandVelocity || headVelocity > leftHandVelocity)
        {
            desiredVelocity = headVelocity;
>>>>>>> Stashed changes
        }
        // Else in the unlikely case that all sensory inputs are moving at the same speed
        else
        {
<<<<<<< Updated upstream
            velocity = headVelocity;
        }
=======
            desiredVelocity = headVelocity;
        }

        velocity = Mathf.Lerp(velocity, desiredVelocity, lerpSpeed);
        if (desiredVelocity > 1) desiredVelocity = 1;
>>>>>>> Stashed changes
    }
}
