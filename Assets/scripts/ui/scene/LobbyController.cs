using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class LobbyController : MonoBehaviour
{
    private Button instagramButton;
    private Button enterButton;
    private Button menuButton;
    private Button heartButton;
    Artists _artists;
    Exhibition _exhibition;
    [SerializeField]
    GameObject _player;

    fps playerFPS;

    public void Activate()
    {
        var roomBuilder = GameObject.Find("RoomBuilder").GetComponent<RoomBuilder>();
        _artists = roomBuilder._artists;
        _exhibition = roomBuilder._exhibition;

        // Get artist information here for the links and so on

        CreateContent();

        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        instagramButton = rootVisualElement.Q<Button>("instagramButton");
        enterButton = rootVisualElement.Q<Button>("enterButton");

        instagramButton.RegisterCallback<ClickEvent>(ev => OpenInstagram());
        enterButton.RegisterCallback<ClickEvent>(ev => EnterExhibition());
        //exitButton 
        menuButton = rootVisualElement.Q<Button>("menuButton");
        menuButton.RegisterCallback<ClickEvent>(ev => ToMenu());

        //instaButton 
        instagramButton = rootVisualElement.Q<Button>("instagramButton");
        instagramButton.RegisterCallback<ClickEvent>(ev => OpenInstagram());
        // heart button
        heartButton = rootVisualElement.Q<Button>("heartButton");
        heartButton.RegisterCallback<ClickEvent>(ev => AddToWishlist());

        playerFPS = _player.GetComponent<fps>();
        playerFPS.enabled = false;

    }

    public void OpenInstagram()
    {

        Application.OpenURL(_artists._artists[0]._instagramLink);
        Debug.Log("Open Instagram called.");
    }
    public void EnterExhibition()
    {
        playerFPS.enabled = true;
        transform.gameObject.SetActive(false);
        Debug.Log("Entered Exhibition.");
    }


    public void ToMenu()
    {
        SceneManager.LoadScene("menu");
        Debug.Log("back to menu called");
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
        rootVisualElement.Q<Label>("title").text = _exhibition._name;
        rootVisualElement.Q<Label>("year").text = "";
        if (_exhibition._description != null && _exhibition._description != "")
        {
            rootVisualElement.Q<Label>("description").text = _exhibition._description;
        }
        else if (_artists._artists[0]._description != null && _artists._artists[0]._description != "")
        {
            rootVisualElement.Q<Label>("description").text = _artists._artists[0]._description;
        }
        else
        {
            rootVisualElement.Q<Label>("description").text = "No description available.";
        }
        //remove dummy name
        rootVisualElement.Q<Label>("artist").text = "";
        foreach (var artist in _artists._artists)
        { 
               rootVisualElement.Q<Label>("artist").text += artist._name + "  ";
        }
        
    }
}
