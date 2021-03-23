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
    public bool playerMoves = false;
    private Vector3 motion;
    private Rigidbody rb;
    CharacterController characterController;
    public GameObject targetCam;
    public GameObject mainCamera;
    public GameObject cameraController;
    public GameObject sceneUI;
    public enum navEnum 
        {
           tour,
           free
        };
    public navEnum navMode = navEnum.free;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.visible = true;
        rb = GetComponent<Rigidbody>();
        mainCamera =  GameObject.Find("mainCamera");
        cameraController = GameObject.Find("cameraController");
        sceneUI = GameObject.Find("sceneUI");
    }

    // Update is called once per frame
    void Update()
    {
        switch (navMode)
        {
            case navEnum.free:
                hor += rotationSpeed * Input.GetAxis("Mouse X");
                transform.eulerAngles = new Vector3(0.0f, hor, 0.0f);
                Vector3 motion =  transform.right * Input.GetAxisRaw("Horizontal");
                motion *= speed * Time.deltaTime;
                characterController.Move(motion);
                break;
            case navEnum.tour:
                break;
        }

        //check if player moves
        if (rb.velocity.magnitude >= 0.2f)
        {
            playerMoves = true;
        }
        else
        {
            playerMoves = false;
        }
    }

    public void SwitchNavMode(string targetMode = null) 
    {

        Debug.Log("SwitchNavMode called.");
        var TourManager = sceneUI.GetComponent<TourManager>();
        switch (targetMode)
        {
            case "free":
                Debug.Log("case free");
                navMode = navEnum.free;
                break;
            case "tour":
                Debug.Log("case tour");
                navMode = navEnum.tour;

                //Get target camera
                
                targetCam = TourManager.artWorks[TourManager.tourIndex];

                //Blend camera
                SnapToObject(TourManager.artWorks[TourManager.tourIndex]);

                break;
            case null:
                Debug.Log("case ELSE");
                if (navMode == navEnum.free)
                {
                    navMode = navEnum.tour;
                    //Get target camera
                    targetCam = TourManager.artWorks[TourManager.tourIndex];

                    //Blend camera
                    SnapToObject(TourManager.artWorks[TourManager.tourIndex]);
                    break;
                }
                else
                {
                    navMode = navEnum.free;
                    SwitchToMainCamera();
                    break;
                }
        }
      
    }


    // snap to objext
    public void SnapToObject(GameObject targetCamTemp)
    {
        if (targetCamTemp)
        {
            // always switch to main camera
            SwitchToMainCamera();

            //Movement of player
            GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(targetCamTemp.transform.position);

            // wait for player to arrive
            while (playerMoves)
            {
            }

            targetCam = targetCamTemp;
            Debug.Log("target Cam set to: " + targetCam.transform.parent.name);

            SwitchCamera();
        }
        SwitchNavMode("tour");
   
    }


    private void SwitchCamera()
    {
        // switch to target 
        CinemachineVirtualCamera activeCam;
        activeCam = targetCam.GetComponent<CinemachineVirtualCamera>();
        activeCam.Priority = 2;

        // Main camera lower priority
        CinemachineVirtualCamera mainCameraCM;
        mainCameraCM = mainCamera.GetComponent<CinemachineVirtualCamera>();
        mainCameraCM.Priority = 1;

        // set tour index to be correct
        GameObject sceneUI = GameObject.Find("sceneUI");
        var TourManager = sceneUI.GetComponent<TourManager>();
        TourManager.setTourIndex(targetCam);
        targetCam = null;
    }


    private void SwitchToMainCamera(){
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

    private void SwitchToTargetCamera(){
        // switch to main camrea
        SwitchToMainCamera();

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