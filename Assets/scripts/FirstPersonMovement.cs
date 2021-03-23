using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Camera-Control/Smooth Mouse Look")]
public class FirstPersonMovement : MonoBehaviour
{
    // Original comment:
    // Simple flycam I made, since I couldn't find any others made public.
    // Made simple to use(drag and drop, done) for regular keyboard layout.
    //Controls:
    // WASD  : Directional movement
    // Shift : Increase speed
    // Space : Moves camera directly up per its local Y-axis

    public float mainSpeed = 0.5f;   // Regular speed
    public float shiftAdd = 2.0f;   // Amount to accelerate when shift is pressed
    public float maxShift = 4.0f;  // Maximum speed when holding shift
    public float camSens = 0.15f;   // Mouse sensitivity

    //MOUSE ROTATION VARIABLES
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    // kind of in the middle of the screen, rather than at the top (play)
    private Vector3 lastMouse = new Vector3(255, 255, 255);
    private float totalRun = 1.0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }
    void Update()
    {
        freeMove();
    }

    //moves player on desktop with wasd and mouse
    private void freeMove()
    {
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
        // Mouse camera angle done.  

        // Keyboard commands
        Vector3 p = GetBaseInput();
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
        transform.Translate(p);
    }
    // Returns the basic values, if it's 0 than it's not active.
    private Vector3 GetBaseInput()
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

        // Up
        if (Input.GetKey(KeyCode.Space))
            p_Velocity += new Vector3(0, 1, 0);

        // Down
        if (Input.GetKey(KeyCode.LeftControl))
            p_Velocity += new Vector3(0, -1, 0);

        return p_Velocity;
    }

    public void moveToTarget(GameObject target)
    {
        GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(target.transform.position);
        GameObject sceneUI = GameObject.Find("sceneUI");
        var TourManager = sceneUI.GetComponent<TourManager>();
        TourManager.setTourIndex(target);
    }
}
