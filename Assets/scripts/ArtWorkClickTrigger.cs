using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtWorkClickTrigger : MonoBehaviour
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
        Debug.Log("Picture clicked");
        GameObject player = GameObject.FindWithTag("Player");
        Debug.Log("triggered Art trigger.");
        var fps = player.GetComponent<fps>();

        // snap to object
        fps.ActivateMoveTo(transform.parent.gameObject.transform.Find("snapTarget").gameObject.transform);
    }
}