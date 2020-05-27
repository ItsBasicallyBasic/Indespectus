using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerController : MonoBehaviour
{

    public SteamVR_Action_Vector2 input;
    public float speed = 1;
    private CharacterController characterController;
    private CapsuleCollider capsuleCollider;

    public Transform head;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(input.axis.magnitude > 0.1f)
        {
            Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(input.axis.x, 0, input.axis.y));
            characterController.Move(speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up) - new Vector3(0,9.81f,0) * Time.deltaTime);
        }

        // Get head height in local space
        float headHeight = Mathf.Clamp(head.localPosition.y, 1, 2);
        characterController.height = headHeight;
        capsuleCollider.height = headHeight;

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
        capsuleCollider.center = newCenter;
    }
}

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerController : MonoBehaviour
{

    public float sensitivity = 0.1f;
    public float maxSpeed;

    public SteamVR_Action_Boolean movePress;
    public SteamVR_Action_Vector2 moveValue;

    private float speed = 0.0f;

    private CharacterController characterController;
    private Transform cameraRig;
    private Transform head;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        cameraRig = SteamVR_Render.Top().origin;
        head = SteamVR_Render.Top().head;
    }

    private void Update()
    {
        HandleHeight();
        HandleHead();
        CalculateMovement();
    }

    private void HandleHead()
    {
        // Store current
        Vector3 oldPosition = cameraRig.position;
        Quaternion oldRotation = cameraRig.rotation;

        // Rotation
        transform.eulerAngles = new Vector3(0.0f, head.rotation.eulerAngles.y, 0.0f);

        // Restore
        cameraRig.position = oldPosition;
        cameraRig.rotation = oldRotation;
    }

    private void CalculateMovement()
    {
        // Figure out movement orientation
        Vector3 orientationEuler = new Vector3(0, transform.eulerAngles.y, 0);
        Quaternion orientation = Quaternion.Euler(orientationEuler);
        Vector3 movement = Vector3.zero;

        // If not moving
        if (movePress.GetStateUp(SteamVR_Input_Sources.Any))
        {
            speed = 0;
        }

        // If button pressed
        if (movePress.state)
        {
            // Add, clamp
            speed += moveValue.axis.y * sensitivity;
            speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);

            // Orientation
            movement += orientation * (speed * Vector3.forward) * Time.deltaTime;
        }

        // Apply
        characterController.Move(movement);
    }

    private void HandleHeight()
    {
        // Get head height in local space
        float headHeight = Mathf.Clamp(head.localPosition.y, 1, 2);
        characterController.height = headHeight;

        // Cut in half
        Vector3 newCenter = Vector3.zero;
        newCenter.y = characterController.height / 2;
        newCenter.y += characterController.skinWidth;

        // Move capsule in local space
        newCenter.x = head.localPosition.x;
        newCenter.z = head.localPosition.x;

        // Rotate
        newCenter = Quaternion.Euler(0, -transform.eulerAngles.y, 0) * newCenter;

        // Apply
        characterController.center = newCenter;

    }
}*/
