using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoButtonClickTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseUpAsButton()
    {
        Debug.Log("Info Button clicked");
        GameObject artInfo = transform.parent.gameObject.transform.Find("artInfo").gameObject;
        Debug.Log("triggered info Button.");
        if (artInfo.activeSelf)
        {
            //deactivate
            artInfo.SetActive(false);
            Debug.Log("Activated");
        }
        else
        {
            //activate
            artInfo.SetActive(true);
            Debug.Log("Activated");
        }

        // snap to object
    }
}