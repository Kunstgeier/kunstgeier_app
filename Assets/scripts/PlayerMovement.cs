using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 0.2f;
    public float rotationSpeed = 5.0f;
    public float smoothFactor = 0.8f;
    private float hor = 0.0f;
    public bool snapped = false;
    private Vector3 motion;
    private Rigidbody rb;
    CharacterController characterController;
    public GameObject targetCam;
    public GameObject mainCamera;
    public GameObject cameraController;
    public enum navEnum 
        {
            wasd, 
            click,
            paused,
            tour
        };
    public navEnum navMode = navEnum.click;
    public navEnum navState = navEnum.click;
    // private bool isDragging = false;
    // private Vector3 dragOrigin;

    // void OnMouseDrag(){
    //     isDragging = true;
        
    //     //Your code
    // }haracter
    
    // void OnMouseUp(){
    //     isDragging = false;
    // }



    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.visible = true;
        rb = GetComponent<Rigidbody>();
        mainCamera =  GameObject.Find("mainCamera");
        cameraController = GameObject.Find("cameraController");
    }

    // Update is called once per frame
    void Update()
    {
        switch (navState)
        {
            case navEnum.wasd:
                hor += rotationSpeed * Input.GetAxis("Mouse X");
                transform.eulerAngles = new Vector3(0, hor, 0.0f);
                Vector3 motion =  transform.right * Input.GetAxisRaw("Horizontal") + transform.forward * Input.GetAxisRaw("Vertical");
                motion *= speed * Time.deltaTime;
                characterController.Move(motion);
                break;
            case navEnum.click:
                // if (Input.GetMouseButtonDown(0))
                // {
                //     dragOrigin = Input.mousePosition;
                //     return;
                // }
        
                // if (!Input.GetMouseButton(0)) return;
        
                // Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
                if (Input.GetMouseButtonUp(0))
                {
                    // hor = -(Input.mousePosition - dragOrigin).x * 0.2f;
                    Vector3 move = new Vector3(0, hor, 0);
                    // transform.eulerAngles += new Vector3(0, hor, 0.0f);
                    transform.Translate(move, Space.World);
                }

                if(Input.GetMouseButtonUp(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitInfo;
                    if(Physics.Raycast(ray, out hitInfo))
                    {
                        GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(hitInfo.point);
                    }
                }
                break;
            case navEnum.tour:
                Debug.Log("tour Mode.");
                break;
            case navEnum.paused:
                // if(targetCam)
                // {
                //     transform.position = Vector3.Lerp(transform.position, targetPoint.transform.position, Time.deltaTime * smoothFactor);
                //     transform.rotation = Quaternion.Slerp(transform.rotation, targetPoint.transform.rotation, Time.deltaTime * smoothFactor);
                // }
                break;
            default:
                Debug.Log("Default Case.");
                break;
        }
    }

    public void switchNavMode(string targetMode = null) 
    {
        switch(targetMode)
        {
            case "wasd":
                navMode = navEnum.wasd;
                navState = navEnum.wasd;
                break;
            case "click":
                navMode = navEnum.click;
                navState = navEnum.click;
                break;
            case "tour":
                navMode = navEnum.tour;
                break;
            case "paused":
                navState = navEnum.paused;
                switchToTargetCamera();
                // CinemachineVirtualCamera activeCam;
                // activeCam = targetCam.GetComponent<CinemachineVirtualCamera>();
                // activeCam.Priority = 11;
                // // Main camera lower priority
                // CinemachineVirtualCamera mainCameraCM;
                // mainCameraCM = mainCamera.GetComponent<CinemachineVirtualCamera>();
                // mainCameraCM.Priority = 1;
                break;
            default:
                if(navState == navEnum.paused)
                {
                    navState = navMode;
                    switchToMainCamera();
                }
                else if(navState == navEnum.wasd)
                {
                    navMode = navEnum.click;
                    navState = navEnum.click;
                }
                else if (navState == navEnum.click)
                {
                    navMode = navEnum.wasd;
                    navState = navEnum.wasd;
                }
                else 
                {
                    navMode = navEnum.wasd;
                    navState = navEnum.wasd;
                }
                break;
        }
    }
    // snap to objext
    public void snapToObject(GameObject targetCamTemp)
    {
        if (targetCamTemp)
        {
            targetCam = targetCamTemp;
            Debug.Log("target Cam set to: " + targetCam.transform.parent.name);
        }
        

        switchNavMode("paused");
        
        // show exit mode button

        // show information

        // track players time  for this artwork and so on !
    }

    private void switchToMainCamera(){
        // Main camera higher priority
        CinemachineVirtualCamera mainCameraCM;
        mainCameraCM = mainCamera.GetComponent<CinemachineVirtualCamera>();
        mainCameraCM.Priority = 2;
        // get main camera brain to find out active camera
        CinemachineBrain cameraBrain;
        cameraBrain = cameraController.GetComponent<CinemachineBrain>();
        ICinemachineCamera activeCam;
        activeCam = cameraBrain.ActiveVirtualCamera;
        Debug.Log(activeCam.Description);
        // active camera gets lower priority
        // CinemachineVirtualCamera activeCamCM;
        // activeCamCM = activeCam.GetComponent<CinemachineVirtualCamera>();
        activeCam.Priority = 1;
    }

    private void switchToTargetCamera(){
        // switch to main camrea
        switchToMainCamera();

        //move player
        GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(targetCam.transform.position);

        // switch to target 
        CinemachineVirtualCamera activeCam;
        activeCam = targetCam.GetComponent<CinemachineVirtualCamera>();
        activeCam.Priority = 2;
        // Main camera lower priority
                // CinemachineVirtualCamera mainCameraCM;
                // mainCameraCM = mainCamera.GetComponent<CinemachineVirtualCamera>();
                // mainCameraCM.Priority = 1;
                // targetCam = null;
        CinemachineBrain cameraBrain;
        cameraBrain = cameraController.GetComponent<CinemachineBrain>();
        ICinemachineCamera previousCam;
        previousCam = cameraBrain.ActiveVirtualCamera;
        Debug.Log(previousCam.Description);
        // active camera gets lower priority
        previousCam.Priority = 1;
        // set tour index to be correct
        GameObject sceneUI = GameObject.Find("sceneUI");
        var TourManager = sceneUI.GetComponent<TourManager>();
        TourManager.setTourIndex(targetCam);
    }
}

// MOVE CHARACTER AND THEN SNAP CAMERA TO PREVENT GOING THROUGH WALLS !!