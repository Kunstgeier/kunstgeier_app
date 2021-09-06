using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Debug.Log("Picture clicked");
        GameObject player = GameObject.FindWithTag("Player");
        Debug.Log("triggered Art trigger.");
        var fps = player.GetComponent<fps>();



        //StartCoroutine(ExampleCoroutine());


        // snap to object
        fps.ActivateMoveTo(transform.parent.gameObject.transform.Find("snapTarget").gameObject.transform);
    }
    //public void OnMouseDown()
    //{
    //    Debug.Log("MouseEnter");
    //    transform.localScale = transform.localScale * 1.2f;
    //}
    //public void OnMouseUp()
    //{
    //    Debug.Log("MouseExit");
    //    transform.localScale = transform.localScale / 1.2f;
    //}

    IEnumerator ExampleCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        transform.localScale = transform.localScale * 1.1f;

        yield return new WaitForSeconds(0.1f);

        transform.localScale = transform.localScale / 1.1f;

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
}

