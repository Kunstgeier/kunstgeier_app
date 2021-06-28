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


    MenuUtils menuUtils;
    

    //check if Rooms are loaded
    private void Start()
    {
        menuUtils = new MenuUtils();
        menuUtils._roomsUpstream = menuUtils.GetRoomsFromServer();
        menuUtils._roomsLocal = menuUtils.GetLoadedSceneNames();
        menuUtils._roomObjects = menuUtils.GetRoomObjects(menuUtils._roomsUpstream);

        // Fill the Menu with buttons
        menuUtils.CreateMenuButtons(menuUtils._roomObjects, transform);

        // https://forum.unity.com/threads/uielements-for-runtime-and-the-ui-builder-unite-copenhagen-2019-talk.758681/

        // settings button

        var rootVisualElement = transform.GetComponent<UIDocument>().rootVisualElement;

        rootVisualElement.Q<Button>("settings").RegisterCallback<ClickEvent>(
            ev => SceneManager.LoadSceneAsync("settings"));

    }

}


// milestone 1 api
// rooms over the air
// user-user interactionen
// gallerie-user interaction

// wettbewerb
// gallerie classico
// 3d modelle 