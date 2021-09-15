using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;


public class InfoButtonClickTrigger : MonoBehaviour
{
    VisualElement rootVisualElement;
    Button artInfoButton;
    TourManager tourManager;
    RoomBuilder roomBuilder;
    ArtPiece artPiece;
    Artist _artist;

    int thisWorkIndex;

    // Start is called before the first frame update
    void Start()
    {
        roomBuilder = GameObject.Find("RoomBuilder").GetComponent<RoomBuilder>();
    }

    public void OnTriggerEnter(Collider other)
    {
        //only if colliding with player and lobby not active
        if (other.transform.name == "Player" && GameObject.Find("Player").GetComponent<fps>().enabled)
        {
            tourManager = GameObject.Find("sceneUI").GetComponent<TourManager>();

            transform.gameObject.GetComponent<UIDocument>().enabled = true;

            rootVisualElement = transform.GetComponent<UIDocument>().rootVisualElement;
            DOTween.To(x => rootVisualElement.style.opacity = x, 0, 1, 0.3f);

            artInfoButton = rootVisualElement.Q<Button>("artInfo");
            artInfoButton.RegisterCallback<ClickEvent>(ev => ShowArtInfo());

            thisWorkIndex = tourManager.GetTourIndex(transform.parent.gameObject.transform.Find("snapTarget").gameObject);
            Debug.Log("Art Index: " + thisWorkIndex);
            artPiece = roomBuilder.artworks._artworks[thisWorkIndex];

            // Get artist information here for the links and so on
            _artist = Array.Find(roomBuilder._artists._artists, a => a._id.ToString() == artPiece._artistID);
            artInfoButton.text = artPiece._name + " || " + _artist._name;
        }

    }

    public void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "Player")
        {
            transform.gameObject.GetComponent<UIDocument>().enabled = false;
        }
    }

    public void ShowArtInfo()
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
        //hide the button
        transform.GetComponent<UIDocument>().enabled = false;
    }
}