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
using UnityEngine.UIElements.Experimental;
using DG.Tweening;
public class MenuManager : MonoBehaviour
{
    //get list of all rooms from server

    APIService apiService;
    public GameObject StartPage;
    public GameObject SearchPage;
    public GameObject SafedPage;
    public GameObject SettingsPage;
    public GameObject OfflinePage;
    public GameObject loadingPage;

    //visualelements
    VisualElement tabBarRootVisualElement;

    VisualElement loadingRootVisualElement;

    //array of UIdocuments

    private UIDocument[] uiDocuments;
    //check if Rooms are loaded
    private void Start()
    {
        
        apiService = transform.GetComponent<APIService>();
        //repeatedly check online status
        //InvokeRepeating("apiService.IsOnline", 0f, 2f);

        //apiService.Get<Rooms>(Routes.GetAllRooms, )
        //menuUtils._roomsUpstream = menuUtils.GetRoomsFromServer();
        //menuUtils._roomsLocal = menuUtils.GetLoadedSceneNames();
        //menuUtils._roomObjects = menuUtils.GetRoomObjects(menuUtils._roomsUpstream);

        //// Fill the Menu with buttons
        //menuUtils.CreateMenuButtons(menuUtils._roomObjects, transform);

        // https://forum.unity.com/threads/uielements-for-runtime-and-the-ui-builder-unite-copenhagen-2019-talk.758681/

        // settings button

        tabBarRootVisualElement = transform.GetComponent<UIDocument>().rootVisualElement;

        //fade in menu and tabbar
        DOTween.To(x => tabBarRootVisualElement.style.marginTop = x, 600, 0, .5f);
        //DOTween.To(x => tabBarRootVisualElement.style.opacity = x, 600, 0, .5f);
        //loading page stuff

        loadingRootVisualElement = loadingPage.GetComponent<UIDocument>()
                                        .rootVisualElement;

        //callbacks for buttons
        tabBarRootVisualElement.Q<Button>("settings").RegisterCallback<ClickEvent>(
            ev => SetSettingsPage());

        tabBarRootVisualElement.Q<Button>("start").RegisterCallback<ClickEvent>(
            ev => SetStartPage());
        tabBarRootVisualElement.Q<Button>("search").RegisterCallback<ClickEvent>(
            ev => SetSearchPage());
        tabBarRootVisualElement.Q<Button>("safed").RegisterCallback<ClickEvent>(
            ev => SetSafedPage());
        SetStartPage();

        // populate uiDocuments

        PopulateUIDocuments();
    }


    public void SetStartPage()
    {
        StartPage.SetActive(true);
        SearchPage.SetActive(false);
        SafedPage.SetActive(false);
        SettingsPage.SetActive(false);
        SetSortingOrder("start");
    }
    public void SetSearchPage()
    {
        StartPage.SetActive(false);
        SearchPage.SetActive(true);
        SafedPage.SetActive(false);
        SettingsPage.SetActive(false);
        SetSortingOrder("search");
    }
    public void SetSafedPage()
    {
        StartPage.SetActive(false);
        SearchPage.SetActive(false);
        SafedPage.SetActive(true);
        SettingsPage.SetActive(false);
        SetSortingOrder("safed");
    }
    public void SetSettingsPage()
    {
        StartPage.SetActive(false);
        SearchPage.SetActive(false);
        SafedPage.SetActive(false);
        SettingsPage.SetActive(true);
        SetSortingOrder("settings");
    }

    // open Room
    public void EnterExhibition(Exhibition exhibition)
    {
        // show loading screen
        ShowLoadingScreen("loading " + exhibition._name);
        ///safe room name or so to playerprefs or json?
        ExhibitionToJson(exhibition);
        //download and open room scene assetbundle
        StartCoroutine(apiService.GetRoomModel(exhibition));
    }

    private void ExhibitionToJson(Exhibition room)
    {
        File.WriteAllText(Application.persistentDataPath + "/RoomToOpen.json", JsonUtility.ToJson(room));
    }


    public void ShowLoadingScreen(string message = null )
    {
        loadingPage.SetActive(true);
        loadingRootVisualElement = loadingPage.GetComponent<UIDocument>()
                                        .rootVisualElement;

        //loadingRootVisualElement.style.opacity = 0;

        if (message != null)
        {
            //change loading screen message
            loadingRootVisualElement.Q<Label>("message").text = message;
        }
        // fade in loading screen
        DOTween.To(x => loadingRootVisualElement.style.opacity = x, 0f, 1f, 0.5f);

        Debug.Log("Show Loading Scene Screen");
    }

    private void PopulateUIDocuments()
    {
        uiDocuments = new UIDocument[] {
                            StartPage.GetComponent<UIDocument>(),
                            SearchPage.GetComponent<UIDocument>(),
                            SafedPage.GetComponent<UIDocument>(),
                            SettingsPage.GetComponent<UIDocument>(),
                            OfflinePage.GetComponent<UIDocument>()
                      };
    }

    private void SetSortingOrder(string page)
    {
        switch (page)
        {
            case "start":
                StartPage.GetComponent<UIDocument>().sortingOrder = 3;
                SettingsPage.GetComponent<UIDocument>().sortingOrder = 2;
                SafedPage.GetComponent<UIDocument>().sortingOrder = 2;
                SearchPage.GetComponent<UIDocument>().sortingOrder = 2;
                OfflinePage.GetComponent<UIDocument>().sortingOrder = 2;
                break;
            case "search":
                StartPage.GetComponent<UIDocument>().sortingOrder = 2;
                SettingsPage.GetComponent<UIDocument>().sortingOrder = 2;
                SafedPage.GetComponent<UIDocument>().sortingOrder = 2;
                SearchPage.GetComponent<UIDocument>().sortingOrder = 3;
                OfflinePage.GetComponent<UIDocument>().sortingOrder = 2;
                break;
            case "safed":
                StartPage.GetComponent<UIDocument>().sortingOrder = 2;
                SettingsPage.GetComponent<UIDocument>().sortingOrder = 2;
                SafedPage.GetComponent<UIDocument>().sortingOrder = 3;
                SearchPage.GetComponent<UIDocument>().sortingOrder = 2;
                OfflinePage.GetComponent<UIDocument>().sortingOrder = 2;
                break;
            case "settings":
                StartPage.GetComponent<UIDocument>().sortingOrder = 2;
                SettingsPage.GetComponent<UIDocument>().sortingOrder = 3;
                SafedPage.GetComponent<UIDocument>().sortingOrder = 2;
                SearchPage.GetComponent<UIDocument>().sortingOrder = 2;
                OfflinePage.GetComponent<UIDocument>().sortingOrder = 2;
                break;
            case "offline":
                StartPage.GetComponent<UIDocument>().sortingOrder = 2;
                SettingsPage.GetComponent<UIDocument>().sortingOrder = 2;
                SafedPage.GetComponent<UIDocument>().sortingOrder = 2;
                SearchPage.GetComponent<UIDocument>().sortingOrder = 2;
                OfflinePage.GetComponent<UIDocument>().sortingOrder = 3;
                break;
            default:
                break;
        }
    }
}