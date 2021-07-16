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
public class StartPageManager : MonoBehaviour
{
    APIService apiService;
    System.Action<string> displayRooms;
    Rooms rooms;

    // Start is called before the first frame update
    public void OnEnable()
    {
        Debug.Log("Start page enabled");
        displayRooms = new System.Action<string>(DisplayRooms);
        apiService = GameObject.Find("tabbar").GetComponent<APIService>();
        var rootVisualElement = transform.GetComponent<UIDocument>().rootVisualElement;
        ReloadRoomList();
    }


    public void ReloadRoomList()
    {
        Debug.Log("Reload rooms now.");
        apiService.Get<Rooms>(Routes.GetAllRooms,  displayRooms);
    }

    public void CreateMenuButtons(List<Room> rooms, Transform transform)
    {
        var menuButton = Resources.Load<VisualTreeAsset>("UI/menuButton");
        var rootVisualElement = transform.GetComponent<UIDocument>().rootVisualElement;
        foreach (Room room in rooms)
        {
            VisualElement tempButton = ModifyButton(menuButton.CloneTree(), room);

            rootVisualElement.Q<VisualElement>("unity-content-container").Add(tempButton);
            //rootVisualElement.hierarchy.ElementAt(0).hierarchy.ElementAt(1).Add(tempButton);
            tempButton.RegisterCallback<ClickEvent>(ev => EnterRoom(room, transform, tempButton));
        }

    }

    public VisualElement ModifyButton(VisualElement button, Room room)
    {
        //if room is not downloaded, grey the background
        //if (!room._downloaded)
        //{
        //    button.hierarchy.ElementAt(0).hierarchy.ElementAt(0).style.backgroundColor = new Color(.9f, .9f, .9f);
        //}
        button.Q<Label>("name").text = room._name;
        button.Q<Label>("artist").text = room._id;

        return (button);
    }

    public void EnterRoom(Room room, Transform transform, VisualElement button)
    {
        //if (room._bundle && room._bundle.)
        //{
        //    //check if vundle exists
        //    string[] paths = room._bundle.GetAllScenePaths();
        //    SceneManager.LoadSceneAsync(paths[0]);
        //    ShowLoadingScreen("Load");
        //}

        Debug.Log("Enter Room not possible right now.");
        //if (room._downloaded)
        //{
        //    transform.gameObject.GetComponent<MenuManager>().StartCoroutine(GetAssetBundle(room, transform));
        //    Debug.Log("Loading: " + room._name);
        //}
        //else
        //{
        //    Debug.Log("Downloading: " + room._name);
        //    transform.gameObject.GetComponent<MenuManager>().StartCoroutine(GetAssetBundle(room, transform));
        //    //change button
        //    button.Q<Label>("name").text = "downloading";

        //    //if download completed:
        //    //update the room in the json and write
        //    File.WriteAllText(Application.persistentDataPath + "/localRooms.json", JsonUtility.ToJson(new RoomList(_roomObjects.Count, _roomObjects)).ToString());
        //}
    }

    public void ShowLoadingScreen(string type, Transform transform)
    {
        transform.GetComponent<UIDocument>().sortingOrder = 0;
        Debug.Log("Show Loading Scene Screen");
    }

    void DisplayRooms(string s)
    {
        Debug.Log("Callback function DisplayRooms called with string: " + s);
        //Parsed object
        APIReturnParser<Rooms> parsed = JsonUtility.FromJson<APIReturnParser<Rooms>>(s);
        //error handling
        if (!parsed.error)
        {
            //get rooms out of json
            rooms = parsed.data;
        }
        else
        {
            Debug.Log("Could not get room list from server due to error: " + parsed.meta.msg);
        }
        var roomCard = Resources.Load<VisualTreeAsset>("UI/roomCard");
        var rootVisualElement = transform.GetComponent<UIDocument>().rootVisualElement;
        foreach(Room r in rooms._rooms)
        {
            VisualElement tempButton = ModifyButton(roomCard.CloneTree().ElementAt(0), r);

            rootVisualElement.Q<VisualElement>("unity-content-container").Add(tempButton);
            //rootVisualElement.hierarchy.ElementAt(0).hierarchy.ElementAt(1).Add(tempButton);
            tempButton.RegisterCallback<ClickEvent>(ev => EnterRoom(r, transform, tempButton));
        }


    }

}

