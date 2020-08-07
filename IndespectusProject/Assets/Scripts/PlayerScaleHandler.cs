using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScaleHandler : MonoBehaviour
{
    private CharacterController characterController;

    public Transform head;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // ADJUST PLAYER CONTROLLER AND COLLIDER DURING PLAY
        // Get head height in local space
        float headHeight = Mathf.Clamp(head.localPosition.y, 1, 2);
        characterController.height = headHeight;

        // Cut in half
        Vector3 newCenter = Vector3.zero;
        newCenter.y = characterController.height / 2;
        newCenter.y += characterController.skinWidth;

        // Move capsule in local space
        newCenter.x = head.localPosition.x;
        newCenter.z = head.localPosition.z;

        // Rotate
        newCenter = Quaternion.Euler(0, -transform.eulerAngles.y, 0) * newCenter;

        // Apply
        characterController.center = newCenter;
    }
}
