using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using DG.Tweening;



public class LobbyController : MonoBehaviour
{
    VisualElement rootVisualElement;
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

        rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        rootVisualElement.style.opacity = 0;
        CreateContent();

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

        foreach (var item in GameObject.FindGameObjectsWithTag("artInfoButton"))
        {
            item.GetComponent<UIDocument>().enabled = false;
        }

    }

    public void OpenInstagram()
    {

        Application.OpenURL(_artists._artists[0]._instagramLink);
        Debug.Log("Open Instagram called.");
    }
    public void EnterExhibition()
    {
        playerFPS.enabled = true;
        // fade out
        DOTween.To(x => rootVisualElement.style.opacity = x, 1, 0, 0.5f).From(true).OnComplete(() => transform.gameObject.SetActive(false));
        //move down
        DOTween.To(x => rootVisualElement.style.marginTop = x, 0, 400, 0.5f);
        //transform.gameObject.SetActive(false);
        Debug.Log("Entered Exhibition.");

        // activate art info button
        foreach (var item in GameObject.FindGameObjectsWithTag("artInfoButton"))
        {
            item.GetComponent<BoxCollider>().enabled = false;
            item.GetComponent<BoxCollider>().enabled = true;

        }
    }


    public void ToMenu()
    {
        //fade out
        DOTween.To(x => rootVisualElement.style.opacity = x, 1, 0, 0.3f).From(true).OnComplete(() => SceneManager.LoadScene("menu"));
        //move down
        DOTween.To(x => rootVisualElement.style.marginTop = x, 0, 400, 0.5f);
        DOTween.To(x => _player.transform.Find("playerCamera").GetComponent<Camera>().focalLength
                            = x, _player.transform.Find("playerCamera")
                                .GetComponent<Camera>().focalLength, 50, 1.5f)
                                .From(true);

        //_player.transform.Find("playerCamera").GetComponent<Camera>().focalLength
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
        //var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
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
        //fade in
        DOTween.To(x => rootVisualElement.style.opacity = x, 0, 1, 0.5f).From(true);
        //move up
        DOTween.To(x => rootVisualElement.style.marginTop = x, 400, 0, 0.5f).From(true);
    }
}
