using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

using System.Net;
using System;
using System.IO;
using DG.Tweening;

public class StartPageManager : MonoBehaviour
{
    APIService apiService;
    //System.Action<string> displayExhibitions;
    Exhibitions exhibitions;
    MenuManager menuManager;

    // Start is called before the first frame update
    public void OnEnable()
    {
        Debug.Log("Start page enabled");
        //displayExhibitions = new System.Action<string>(DisplayExhibitions);
        apiService = GameObject.Find("tabbar").GetComponent<APIService>();
        menuManager = GameObject.Find("tabbar").GetComponent<MenuManager>();

        var rootVisualElement = transform.GetComponent<UIDocument>().rootVisualElement;
        ReloadExhibitionList();
        Debug.Log(PlayerPrefs.GetString("username", "username not found"));
    }
    public void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }


    public void ReloadExhibitionList()
    {
        Debug.Log("Reload Exhibitions now.");
        apiService.Get<Exhibitions>(Routes.GetAllRooms, DisplayExhibitions);
    }

    public VisualElement ModifyButton(VisualElement button, Exhibition exhibition)
    {
        //if exhibition is not downloaded, grey the background
        //if (!exhibition._downloaded)
        //{
        //    button.hierarchy.ElementAt(0).hierarchy.ElementAt(0).style.backgroundColor = new Color(.9f, .9f, .9f);
        //}
        button.Q<Label>("name").text = exhibition._name;
        button.Q<Label>("artist").text = exhibition._id;
        if (exhibition._thumbnailLink != null)
        {
            apiService.StartCoroutine(apiService.GetButtonThumbnail(exhibition._thumbnailLink, button));
        }
        return (button);
    }

    public void ShowLoadingScreen(string type, Transform transform)
    {
        transform.GetComponent<UIDocument>().sortingOrder = 0;
        Debug.Log("Show Loading Scene Screen");
    }

    void DisplayExhibitions(string s)
    {
        Debug.Log("Callback function DisplayExhibitions called with string: " + s);
        //Parsed object
        APIReturnParser<Exhibitions> parsed = JsonUtility.FromJson<APIReturnParser<Exhibitions>>(s);
        //error handling
        if (parsed.error != null)
        {
            //get exhibitions out of json
            exhibitions = parsed.data;
            Debug.Log(exhibitions._exhibitions[0]._name);
        }
        else
        {
            Debug.Log("Could not get exhibition list from server due to error: " + parsed.error.message);
        }
        var exhibitionCard = Resources.Load<VisualTreeAsset>("UI/exhibitionCard");
        //get start page ui
        var rootVisualElement = transform.GetComponent<UIDocument>().rootVisualElement;
        foreach (Exhibition r in exhibitions._exhibitions)
        {
            Debug.Log(r._name);
            VisualElement tempButton = ModifyButton(exhibitionCard.CloneTree().ElementAt(0), r);

            rootVisualElement.Q<VisualElement>("unity-content-container").Add(tempButton);
            //set opacity to zero
            tempButton.style.opacity = 0;
            //rootVisualElement.hierarchy.ElementAt(0).hierarchy.ElementAt(1).Add(tempButton);
            tempButton.RegisterCallback<ClickEvent>(ev => menuManager.EnterExhibition(r));
        }
    }

}
