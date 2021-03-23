using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseUpAsButton() 
    {
        Debug.Log("Picture clicked");
        GameObject player = GameObject.FindWithTag("Player");
        Debug.Log("triggered Art trigger.");
        var playerMovement = player.GetComponent<FirstPersonMovement>();

        // snap to object
        //playerMovement.SwitchNavMode("tour");
        playerMovement.moveToTarget(transform.Find("snapTarget").gameObject);
    }

    //private void OnMouseEnter()
    //{
    //    Debug.Log("Mouse over detected!!");
    //    Cursor.lockState = CursorLockMode.None;
    //    transform.localScale *= -10f;
    //}
    //private void OnMouseExit()
    //{
        
    //    Debug.Log("Mouse over detected!!");
    //    transform.localScale *= 10f;
    //    Cursor.lockState = CursorLockMode.Locked;
    //}

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject player = GameObject.FindWithTag("Player");
            Debug.Log("triggered Art trigger.");
            var playerMovement = player.GetComponent<PlayerMovement>();

            // snap to object
            playerMovement.SwitchNavMode("tour");
            playerMovement.SnapToObject(transform.Find("virtualCam").gameObject);
        }

    }
}
