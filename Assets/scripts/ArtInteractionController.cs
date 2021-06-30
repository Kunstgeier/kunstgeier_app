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
    private GameObject player;

    private TourManager tourManager;
    private int thisWorkIndex;

    private ArtInfo _ArtInfo;
    private ArtInfoList _ArtInfoList;

    private Artists _Artists;
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

        //get Artinfo from json
        //read json
        // A LOT TO DOO HERE
        CreateContent();

        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        instagramButton = rootVisualElement.Q<Button>("instagramButton");
        orderButton = rootVisualElement.Q<Button>("orderButton");

        instagramButton.RegisterCallback<ClickEvent>(ev => OpenInstagram());
        orderButton.RegisterCallback<ClickEvent>(ev => AddToCart());
        //menuButton 
        closeInfoButton = rootVisualElement.Q<Button>("exitButton");
        closeInfoButton.RegisterCallback<ClickEvent>(ev => CloseArtInfo());

        // heart button
        heartButton = rootVisualElement.Q<Button>("heartButton");
        heartButton.RegisterCallback<ClickEvent>(ev => addToWishlist());
    }

    public void OpenInstagram()
    {
        Application.OpenURL(_ArtInfo._artist._instagramLink);
        Debug.Log("Open Instagram is not implemented yet!");
    }
    public void AddToCart()
    {
        Application.OpenURL(_ArtInfo._buyLink);
        Debug.Log("Entered Shop.");
    }


    public void CloseArtInfo()
    {
        Debug.Log("Close Info Button clicked");
        transform.gameObject.SetActive(false);
        Debug.Log("Closed");
    }

    private void CreateContent()
    {
        string jsonList = Resources.Load<TextAsset>("Scenes/artInfo").text;
        var _ArtInfoList = JsonUtility.FromJson<ArtInfoList>(jsonList);

        _ArtInfo = Array.Find(_ArtInfoList.artInfoList,
                                artInfo => transform.parent.name.
                                            EndsWith(artInfo._id.ToString()));

        string jsonArtist = Resources.Load<TextAsset>("Scenes/artists").text;
        _Artists = JsonUtility.FromJson<Artists>(jsonArtist);

        _ArtInfo._artist = Array.Find(_Artists.artists,
                                artist => artist._id == _ArtInfo._artistID);
        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        rootVisualElement.Q<Label>("title").text = _ArtInfo._title;
        rootVisualElement.Q<Label>("year").text = _ArtInfo._year.ToString();
        rootVisualElement.Q<Label>("description").text = _ArtInfo._description;
        rootVisualElement.Q<Label>("artist").text = _ArtInfo._artist._name;
    }
}


