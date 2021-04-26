using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtWorkClickTrigger : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseUpAsButton()
    {
        Debug.Log("Picture clicked");
        GameObject player = GameObject.FindWithTag("Player");
        Debug.Log("triggered Art trigger.");
        var fps = player.GetComponent<fps>();

        //transform.localScale = transform.localScale * 1.2f;
        //transform.localScale = transform.localScale / 1.2f;

        // snap to object
        fps.ActivateMoveTo(transform.parent.gameObject.transform.Find("snapTarget").gameObject.transform);
    }
    public void OnMouseDown()
    {
        Debug.Log("MouseEnter");
        transform.localScale = transform.localScale * 1.2f;
    }
    public void OnMouseUp()
    {
        Debug.Log("MouseExit");
        transform.localScale = transform.localScale / 1.2f;
    }
}