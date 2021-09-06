using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class fps : MonoBehaviour
{
    // References
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CharacterController characterController;

    //NavMesh
    [SerializeField] private NavMeshAgent navMeshAgent;


    // Player settings
    [SerializeField] private float cameraSensitivity;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveInputDeadZone;

    // Touch detection
    private int leftFingerId, rightFingerId;
    private float halfScreenWidth;

    // Camera control
    private Vector2 lookInput;
    private float cameraPitch;

    // Player movement
    private Vector2 moveTouchStartPosition;
    private Vector2 moveInput;

    //move to postion
    private bool getsMoved = false;
    private bool getsRotated = false;

    private Transform moveToTarget;

    //Desktop/Web controls
    [SerializeField]
    private bool useWasd = false;

    // Original comment:
    // Simple flycam I made, since I couldn't find any others made public.
    // Made simple to use(drag and drop, done) for regular keyboard layout.
    //Controls:
    // WASD  : Directional movement
    // Shift : Increase speed
    // Space : Moves camera directly up per its local Y-axis

    public float mainSpeed = 4.0f;   // Regular speed
    public float shiftAdd = 2.0f;   // Amount to accelerate when shift is pressed
    public float maxShift = 4.0f;  // Maximum speed when holding shift
    public float camSens = 0.15f;   // Mouse sensitivity

    //MOUSE ROTATION VARIABLES
    public float speedH = 4.0f;
    public float speedV = 4.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    // kind of in the middle of the screen, rather than at the top (play)
    private Vector3 lastMouse = new Vector3(255, 255, 255);
    private float totalRun = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer) //Application.platform == RuntimePlatform.OSXEditor ||
        {
            Debug.Log("Using wasd steering.");
            useWasd = true;
        }

        // id = -1 means the finger is not being tracked
        leftFingerId = -1;
        rightFingerId = -1;

        // only calculate once
        halfScreenWidth = Screen.width / 2;

        // calculate the movement input dead zone
        moveInputDeadZone = Mathf.Pow(Screen.height / moveInputDeadZone, 2);

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = true;
        Debug.Log(navMeshAgent.destination.ToString());
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Handles input
        GetTouchInput();

        if (!navMeshAgent.pathPending && getsMoved)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    Debug.Log("Target reached");
                    navMeshAgent.ResetPath();
                    GameObject sceneUI = GameObject.Find("sceneUI");
                    var TourManager = sceneUI.GetComponent<TourManager>();
                    TourManager.SetTourIndex(moveToTarget.gameObject);
                    getsRotated = true;
                    getsMoved = false;
                    return;
                }
            }
        }
       
        
        else if (getsRotated)
        {
            // Rotation Done ?
            Debug.Log("Rotating");
            if ( Mathf.Abs(Quaternion.Angle(transform.rotation, moveToTarget.rotation)) < 4f)
            {
                Debug.Log("Rotation done");
                getsRotated = false;
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, moveToTarget.rotation, 0.4f);

        }
        //free movement possible
        else if (useWasd)
        {
            wasdMove();
        }
        else
        {
            //Debug.Log("Free Movement");
            if (rightFingerId != -1)
            {
                // Ony look around if the right finger is being tracked
                //Debug.Log("Rotating");
                LookAround();
            }

            if (leftFingerId != -1)
            {
                // Ony move if the left finger is being tracked
                //Debug.Log("Moving");
                Move();
            }
        }
    }

    void GetTouchInput()
    {
        // Iterate through all the detected touches
        for (int i = 0; i < Input.touchCount; i++)
        {

            Touch t = Input.GetTouch(i);

            // Check each touch's phase
            switch (t.phase)
            {
                case TouchPhase.Began:

                    if (t.position.x < halfScreenWidth && leftFingerId == -1)
                    {
                        // Start tracking the left finger if it was not previously being tracked
                        leftFingerId = t.fingerId;

                        // Set the start position for the movement control finger
                        moveTouchStartPosition = t.position;
                    }
                    else if (t.position.x > halfScreenWidth && rightFingerId == -1)
                    {
                        // Start tracking the rightfinger if it was not previously being tracked
                        rightFingerId = t.fingerId;
                    }

                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:

                    if (t.fingerId == leftFingerId)
                    {
                        // Stop tracking the left finger
                        leftFingerId = -1;
                        //Debug.Log("Stopped tracking left finger");
                    }
                    else if (t.fingerId == rightFingerId)
                    {
                        // Stop tracking the right finger
                        rightFingerId = -1;
                        Debug.Log("Stopped tracking right finger");
                    }

                    break;
                case TouchPhase.Moved:

                    // Get input for looking around
                    if (t.fingerId == rightFingerId)
                    {
                        lookInput = t.deltaPosition * cameraSensitivity * Time.deltaTime;
                    }
                    else if (t.fingerId == leftFingerId)
                    {

                        // calculating the position delta from the start position
                        moveInput = t.position - moveTouchStartPosition;
                    }

                    break;
                case TouchPhase.Stationary:
                    // Set the look input to zero if the finger is still
                    if (t.fingerId == rightFingerId)
                    {
                        lookInput = Vector2.zero;
                    }
                    break;
            }
        }
    }
    //moves player on desktop with wasd and mouse
    private void wasdMove()
    {
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
        // Mouse camera angle done.  

        // Keyboard commands
        Vector3 p = GetDesktopInput();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            totalRun += Time.deltaTime;
            p *= totalRun * shiftAdd;
            p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
            p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
            p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
        }
        else
        {
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p *= mainSpeed;
        }

        p *= Time.deltaTime;
        //transform.Translate(p);
        //Dont move if small dist
        //if (moveInput.sqrMagnitude <= moveInputDeadZone) return;
        characterController.Move(transform.right * p.x +
                                    transform.forward * p.z +
                                    new Vector3(0, -8, 0));
    }
    // Returns the basic values, if it's 0 than it's not active.
    private Vector3 GetDesktopInput()
    {
        Vector3 p_Velocity = new Vector3();

        // Forwards
        if (Input.GetKey(KeyCode.W))
            p_Velocity += new Vector3(0, 0, 1);

        // Backwards
        if (Input.GetKey(KeyCode.S))
            p_Velocity += new Vector3(0, 0, -1);

        // Left
        if (Input.GetKey(KeyCode.A))
            p_Velocity += new Vector3(-1, 0, 0);

        // Right
        if (Input.GetKey(KeyCode.D))
            p_Velocity += new Vector3(1, 0, 0);

        //// Up
        //if (Input.GetKey(KeyCode.Space))
        //    p_Velocity += new Vector3(0, 1, 0);

        //// Down
        //if (Input.GetKey(KeyCode.LeftControl))
        //    p_Velocity += new Vector3(0, -1, 0);

        return p_Velocity;
    }

    void LookAround()
    {

        // vertical (pitch) rotation
        cameraPitch = Mathf.Clamp(cameraPitch + lookInput.y, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(-cameraPitch, 0, 0);

        // horizontal (yaw) rotation
        transform.Rotate(transform.up, lookInput.x);
    }

    void Move()
    {

        // Don't move if the touch delta is shorter than the designated dead zone
        if (moveInput.sqrMagnitude <= moveInputDeadZone) return;

        // Multiply the normalized direction by the speed
        Vector2 movementDirection = moveInput.normalized * moveSpeed * Time.deltaTime;
        // Move relatively to the local transform's direction
        characterController.Move(transform.right * movementDirection.x +
                                    transform.forward * movementDirection.y +
                                    new Vector3(0, -8, 0));
    }


    public void ActivateMoveTo(Transform target)
    {
        //getsMoved = true;
        moveToTarget = target;
        float distanceToArt = 0.5f;
        navMeshAgent.destination = moveToTarget.position + new Vector3(0f, distanceToArt, 0f);
        getsMoved = true;
        // reset camera pitch
        lookInput.y = 0;
        cameraPitch = 0;
        cameraTransform.localRotation = Quaternion.Euler(0, 0, 0);

    }
}
