using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class ArtInteractionController : MonoBehaviour
{
    private Button instagramButton;
    private Button orderButton;
    private Button closeInfoButton;
    private Button heartButton;
    private Button fullscreenButton;

    public int tourIndex;
    private fps playerFPS;

    private TourManager tourManager;
    private int thisWorkIndex;
    private RoomBuilder roomBuilder;
    [SerializeField]
    private GameObject artInfo;
    [SerializeField]
    private GameObject fullscreenView;

    private ArtPiece artPiece;

    private Artist _artist;

    [SerializeField]
    float zoomFactor;
    // private var playerMovement;

    // Start is called before the first frame update
    //void Update()
    //{
    //    //close artInfo if player to far away
    //    if (thisWorkIndex != tourManager.tourIndex)
    //    {
    //        Debug.Log("This Work Index" + thisWorkIndex);
    //        Debug.Log("Current Art Index" + tourManager.tourIndex);

    //        CloseArtInfo();
    //    }

    //}



    public void ActivateInfoPage(int index)
    {
        artInfo.gameObject.SetActive(true);
        //get art index of this table
        tourManager = GameObject.Find("sceneUI").GetComponent<TourManager>();
        thisWorkIndex = index;
        roomBuilder = GameObject.Find("RoomBuilder").GetComponent<RoomBuilder>();
        artPiece = roomBuilder.artworks._artworks[thisWorkIndex];

        // Get artist information here for the links and so on
        _artist = Array.Find(roomBuilder._artists._artists, a => a._id.ToString() == artPiece._artistID);

        CreateContent();

        var rootVisualElement = artInfo.GetComponent<UIDocument>().rootVisualElement;
        instagramButton = rootVisualElement.Q<Button>("instagramButton");
        orderButton = rootVisualElement.Q<Button>("orderButton");

        instagramButton.RegisterCallback<ClickEvent>(ev => OpenInstagram());
        orderButton.RegisterCallback<ClickEvent>(ev => AddToCart());
        //exitButton 
        closeInfoButton = rootVisualElement.Q<Button>("exitButton");
        closeInfoButton.RegisterCallback<ClickEvent>(ev => CloseArtInfo());

        // heart button
        heartButton = rootVisualElement.Q<Button>("heartButton");
        heartButton.RegisterCallback<ClickEvent>(ev => AddToWishlist());

        // fullscreen button
        fullscreenButton = rootVisualElement.Q<Button>("fullscreenButton");
        fullscreenButton.RegisterCallback<ClickEvent>(ev => ShowFullscreen());

        playerFPS = GameObject.Find("Player").GetComponent<fps>();
        playerFPS.enabled = false;
    }

    public void OpenInstagram()
    {
        Application.OpenURL(_artist._instagramLink);
        Debug.Log("Open Instagram is not implemented yet!");
    }
    public void AddToCart()
    {
        Application.OpenURL(artPiece._buyLink);
        Debug.Log("Entered Shop.");
    }


    public void CloseArtInfo()
    {
        fullscreenView.SetActive(false);

        Debug.Log("Close Info Button clicked");
        artInfo.gameObject.SetActive(false);
        playerFPS.enabled = true;

        //reenable art button
        transform.parent.Find("artInfoButton").GetComponent<InfoButtonClickTrigger>().OnTriggerEnter(GameObject.Find("Player").GetComponent<CapsuleCollider>());

        Debug.Log("Closed");
    }

    private void AddToWishlist()
    {
        // send to server

        // change button

        // heart menu
    }

    public void ShowFullscreen()
    {
        // activate fs view
        fullscreenView.SetActive(true);
        var rootVisualElementFS = fullscreenView.GetComponent<UIDocument>()
                .rootVisualElement;
        //add the image to the fullscreen view
        rootVisualElementFS.Q<VisualElement>("unity-content-container").style.backgroundImage =
            Background.FromSprite(roomBuilder._artObjects[thisWorkIndex]
                                             .transform
                                             .Find("art")
                                             .GetComponent<SpriteRenderer>()
                                             .sprite);
        //add the callback to the close button
        rootVisualElementFS.Q<Button>("close").RegisterCallback<ClickEvent>(
                ev => fullscreenView.SetActive(false)
            );


        rootVisualElementFS.Q<VisualElement>("unity-content-container").style.width = Screen.width * zoomFactor;
        rootVisualElementFS.Q<VisualElement>("unity-content-container").style.height = Screen.height * zoomFactor;
        
        Debug.Log("Show fullscreen called.");
    }

    private void CreateContent()
    {
        var rootVisualElement = artInfo.GetComponent<UIDocument>().rootVisualElement;
        rootVisualElement.Q<Label>("title").text = artPiece._name;
        rootVisualElement.Q<Label>("year").text = artPiece._year;
        rootVisualElement.Q<Label>("description").text = artPiece._description;
        rootVisualElement.Q<Label>("artist").text = _artist._name;
        rootVisualElement.Q<VisualElement>("artwork").style.backgroundImage =  Background.FromSprite(roomBuilder._artObjects[thisWorkIndex].transform.Find("art").GetComponent<SpriteRenderer>().sprite);
    }
}


