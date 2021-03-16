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
        var playerMovement = player.GetComponent<PlayerMovement>();

        // snap to object
        playerMovement.snapToObject(transform.Find("virtualCam").gameObject);
    }
    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject player = GameObject.FindWithTag("Player");
            Debug.Log("triggered Art trigger.");
            var playerMovement = player.GetComponent<PlayerMovement>();

            // snap to object
            playerMovement.snapToObject(transform.Find("virtualCam").gameObject);
        }

    }
}
