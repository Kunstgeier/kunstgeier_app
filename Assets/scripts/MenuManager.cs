using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

using System.Net;
using System;
using System.IO;

public class MenuManager : MonoBehaviour
{
    //get list of all rooms from server
    //... 

    private List<Room> _roomsUpstream; //imagine this comes from some server
    private List<Room> _roomsLocal; //rooms downloaded onto device

    private List<Room> _roomObjects;

    //check if Rooms are loaded
    private void Start()
    {
      
        _roomsUpstream = GetRoomsFromServer();
        _roomsLocal = GetLoadedSceneNames();
        _roomObjects = GetRoomObjects(_roomsUpstream);

        // Fill the Menu with buttons
        CreateMenuButtons(_roomObjects);

        // https://forum.unity.com/threads/uielements-for-runtime-and-the-ui-builder-unite-copenhagen-2019-talk.758681/

        //load on click coroutine

        //add download animation

        //add scene loading animation (on starting)

        //someway add the artPiece names and descriptions to the artInfo thing

        // if its a standard room, just download the art and art info

    }

    //returns nmes of all scenes that are in the build index
    // what if i download a scene? is it added to the buildindex?
    private List<Room> GetLoadedSceneNames()
    {
        // does the user file already exists?
        //if(File.Exists(Application.persistentDataPath + "/localRooms.json"))
        //{
        //    string json = File.ReadAllText(Application.persistentDataPath + "/localRooms.json");
        //    var roomsLocal = JsonUtility.FromJson<RoomList>(json);
        //    List<Room> returnRooms = new List<Room>();
        //    foreach (Room r in roomsLocal.rooms.objects)
        //    {
        //        returnRooms.Add(r);
        //    }
        //    return (returnRooms);
        //}
        //else
        {
            string json = Resources.Load<TextAsset>("Scenes/exampleResponse").text;
            var roomsLocal = JsonUtility.FromJson<RoomList>(json);
            List<Room> returnRooms = new List<Room>();
            foreach (Room r in roomsLocal.rooms.objects)
            {
                returnRooms.Add(r);
            }
            return (returnRooms);
        }
        //var sceneNames = new List<string>();
        //for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        //{
        //    sceneNames.Add(SceneUtility.GetScenePathByBuildIndex(i));
        //}
        //return (sceneNames);
    }

    //returns all rooms listed on the server
    private List<Room> GetRoomsFromServer()
    {

        // server model: room(_name, _gallery, _assetBundlePath, _location)
        // local model: room(_name, _gallery, _assetBundlePath, _location, _downloaded = false)

        //some server interaction here
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest
                .Create(String.Format("http://api.kunstgeier.de/rooms/getAll"));

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string jsonResponse = reader.ReadToEnd();
            RoomList rooms = new RoomList();
            rooms = JsonUtility.FromJson<RoomList>(jsonResponse);
            List<Room> returnRooms = new List<Room>();
            foreach(Room r in rooms.rooms.objects)
            {
                returnRooms.Add(r);
            }
            Debug.Log("server success!: " + jsonResponse);
            return (returnRooms);
        }
        catch(WebException e)
        {
            Debug.Log("Connection to Server failes with following message: " + e.Message);
            Debug.Log("Continuing with already downloaded rooms.");
            var rooms = new List<Room>();
            return(rooms);
        }


    }

    private List<Room> GetRoomObjects(List<Room> roomsUpstream)
    {
        var roomObjects = new List<Room>();
        //in case we have no connection to server
        if(roomsUpstream.Count < 1)
        {
            Debug.Log("No upstream rooms available. Using buildindex instead.");
            foreach(Room room in _roomsLocal)
            {
                //set downloaded to true, because room is locally available
                roomObjects.Add(new Room(room._id, room._name, room._gallery, room._assetBundlePath, room._location, room._downloaded));

            }
        }
        else
        {
            foreach (Room room in roomsUpstream)
            {
                //check if scene is loaded
                int localRoomIndex = _roomsLocal.FindIndex(localRoom => localRoom._id == room._id);
                if (localRoomIndex >= 0)
                {
                    roomObjects.Add(_roomsLocal[localRoomIndex]);
                }
                else
                {
                    //add room even if not downloaded yet.
                    // room._downloaded defaults to `false`
                    roomObjects.Add(room);
                }
            }
        }
        File.WriteAllText(Application.persistentDataPath + "/localRooms.json", JsonUtility.ToJson(roomObjects));
        return (roomObjects);
    }

    private void CreateMenuButtons(List<Room> rooms)
    {
        var menuButton = Resources.Load<VisualTreeAsset>("UI/menuButton");
        var rootVisualElement = transform.GetComponent<UIDocument>().rootVisualElement;
        foreach (Room room in rooms)
        {
            VisualElement tempButton = ModifyButton(menuButton.CloneTree(), room);
            
            rootVisualElement.hierarchy.ElementAt(0).hierarchy.ElementAt(1).Add(tempButton);
            tempButton.RegisterCallback<ClickEvent>(ev => EnterRoom(room));
        }

    }

    private VisualElement ModifyButton(VisualElement button, Room room)
    {
        //if room is not downloaded, grey the background
        if (!room._downloaded)
        {
            button.hierarchy.ElementAt(0).hierarchy.ElementAt(0).style.backgroundColor = new Color(.9f, .9f, .9f);
        }
        button.Q<Label>("name").text = room._name;
        button.Q<Label>("gallery").text = room._gallery;

        return (button);
    }

    public void EnterRoom(Room room)
    {
        if (room._downloaded)
        {
            SceneManager.LoadSceneAsync(room._name);

            Debug.Log("Loading: " + room._name);
        }
        else
        {
            Debug.Log("Downloading: " + room._name);
            Debug.Log("Download not implemented yet...");

            //if download completed:
            //update the room in the json and write
            //File.WriteAllText(Application.persistentDataPath + "/localRooms.json", JsonUtility.ToJson(roomObjects));
        }
    }
}


// milestone 1 api
// rooms over the air
// user-user interactionen
// gallerie-user interaction

// wettbewerb
// gallerie classico
// 3d modelle 