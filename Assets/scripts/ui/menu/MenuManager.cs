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

public class MenuManager : MonoBehaviour
{
    //get list of all rooms from server

    APIService apiService;
    public GameObject StartPage;
    public GameObject SearchPage;
    public GameObject SafedPage;
    public GameObject SettingsPage;
    public GameObject OfflinePage;




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

        var rootVisualElement = transform.GetComponent<UIDocument>().rootVisualElement;

        rootVisualElement.Q<Button>("settings").RegisterCallback<ClickEvent>(
            ev => SetSettingsPage());

        rootVisualElement.Q<Button>("start").RegisterCallback<ClickEvent>(
            ev => SetStartPage());
        rootVisualElement.Q<Button>("search").RegisterCallback<ClickEvent>(
            ev => SetSearchPage());
        rootVisualElement.Q<Button>("safed").RegisterCallback<ClickEvent>(
            ev => SetSafedPage());
        SetStartPage();

    }

    //public void Update()
    //{
    //    if (!apiService.online)
    //    {
    //        //activate offline mode
    //        OfflinePage.SetActive(true);
    //    }
    //    else
    //    {
    //        //inactivate offline mode
    //        OfflinePage.SetActive(false);
    //    }
    //}

    public void SetStartPage()
    {
        StartPage.SetActive(true);
        SearchPage.SetActive(false);
        SafedPage.SetActive(false);
        SettingsPage.SetActive(false);
    }
    public void SetSearchPage()
    {
        StartPage.SetActive(false);
        SearchPage.SetActive(true);
        SafedPage.SetActive(false);
        SettingsPage.SetActive(false);
    }
    public void SetSafedPage()
    {
        StartPage.SetActive(false);
        SearchPage.SetActive(false);
        SafedPage.SetActive(true);
        SettingsPage.SetActive(false);
    }
    public void SetSettingsPage()
    {
        StartPage.SetActive(false);
        SearchPage.SetActive(false);
        SafedPage.SetActive(false);
        SettingsPage.SetActive(true);
    }
}
