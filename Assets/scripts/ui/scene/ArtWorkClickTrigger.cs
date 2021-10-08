using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArtWorkClickTrigger : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(ResetCollider());
    }

    IEnumerator ResetCollider()
    {
        Destroy(this.gameObject.GetComponent("BoxCollider"));
        yield return 0;
        this.gameObject.AddComponent<BoxCollider>();
        Debug.Log("Collider renewed.");
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseUpAsButton()
    {
        Debug.Log("Art work clicked before if statement");
        if (GameObject.Find("Player").GetComponent<fps>().enabled)
        {
            Debug.Log("Picture clicked");
            GameObject player = GameObject.FindWithTag("Player");
            Debug.Log("triggered Art trigger.");
            var fps = player.GetComponent<fps>();

            // snap to object
            fps.ActivateMoveTo(transform.parent.gameObject.transform.Find("snapTarget").gameObject.transform);
        }
    }
}

