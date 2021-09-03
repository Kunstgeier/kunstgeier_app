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

    public int tourIndex;
    private fps playerFPS;

    private TourManager tourManager;
    private int thisWorkIndex;

    private ArtPiece artPiece;

    private Artist _artist;
    // private var playerMovement;

    // Start is called before the first frame update
    void Update()
    {
        //close artInfo if player to far away
        if (thisWorkIndex != tourManager.tourIndex)
        {
            Debug.Log("This Work Index" + thisWorkIndex);
            Debug.Log("Current Art Index" + tourManager.tourIndex);

            CloseArtInfo();
        }

    }


    private void OnEnable()
    {
        //get art index of this table
        tourManager = GameObject.Find("sceneUI").GetComponent<TourManager>();
        thisWorkIndex = tourManager.GetTourIndex(transform.parent.gameObject.transform.Find("snapTarget").gameObject);

        var roomBuilder = GameObject.Find("RoomBuilder").GetComponent<RoomBuilder>();
        artPiece = roomBuilder.artworks._artworks[thisWorkIndex];

        // Get artist information here for the links and so on
        _artist = Array.Find(roomBuilder._artists._artists, a => a._id.ToString() == artPiece._artistID);

        CreateContent();

        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
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
        Debug.Log("Close Info Button clicked");
        transform.gameObject.SetActive(false);
        playerFPS.enabled = true;
        Debug.Log("Closed");
    }

    private void AddToWishlist()
    {
        // send to server

        // change button

        // heart menu
    }

    private void CreateContent()
    {
        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        rootVisualElement.Q<Label>("title").text = artPiece._name;
        rootVisualElement.Q<Label>("year").text = artPiece._year;
        rootVisualElement.Q<Label>("description").text = artPiece._description;
        rootVisualElement.Q<Label>("artist").text = _artist._name;
    }
}


