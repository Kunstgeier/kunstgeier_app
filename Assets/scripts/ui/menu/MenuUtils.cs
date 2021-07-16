//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using UnityEngine.UIElements;
//using UnityEngine.SceneManagement;
//using UnityEngine.Networking;

//using System.Net;
//using System;
//using System.IO;

//public class MenuUtils
//{
//    //get list of all rooms from server
//    //... 

//    public List<Room> _roomsUpstream; //imagine this comes from some server
//    public List<Room> _roomsLocal; //rooms downloaded onto device

//    public List<Room> _roomObjects; 

//    //returns nmes of all scenes that are in the build index
//    // what if i download a scene? is it added to the buildindex?
//    public List<Room> GetLoadedSceneNames()
//    {
//        // does the user file already exists?
//        if (File.Exists(Application.persistentDataPath + "/localRooms.json"))
//        {
//            string json = File.ReadAllText(Application.persistentDataPath + "/localRooms.json");
//            var roomsLocal = JsonUtility.FromJson<RoomList>(json);
//            List<Room> returnRooms = new List<Room>();
//            foreach (Room r in roomsLocal.rooms._rooms)
//            {
//                returnRooms.Add(r);
//            }
//            return (returnRooms);
//        }
//        else
//        {
//            string json = Resources.Load<TextAsset>("Scenes/exampleResponse").text;
//            var roomsLocal = JsonUtility.FromJson<RoomList>(json);
//            List<Room> returnRooms = new List<Room>();
//            foreach (Room r in roomsLocal.rooms._rooms)
//            {
//                returnRooms.Add(r);
//            }
//            return (returnRooms);
//        }
//        //var sceneNames = new List<string>();
//        //for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
//        //{
//        //    sceneNames.Add(SceneUtility.GetScenePathByBuildIndex(i));
//        //}
//        //return (sceneNames);
//    }

//    //returns all rooms listed on the server
//    public List<Room> GetRoomsFromServer()
//    {

//        // server model: room(_name, _gallery, _assetBundlePath, _location)
//        // local model: room(_name, _gallery, _assetBundlePath, _location, _downloaded = false)

//        //some server interaction here
//        try
//        {
//            HttpWebRequest request = (HttpWebRequest)WebRequest
//                .Create(String.Format("https://api.kunstgeier.de/rooms/getAll"));

//            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
//            StreamReader reader = new StreamReader(response.GetResponseStream());
//            string jsonResponse = reader.ReadToEnd();
//            RoomList rooms = new RoomList();
//            rooms = JsonUtility.FromJson<RoomList>(jsonResponse);
//            List<Room> returnRooms = new List<Room>();
//            foreach (Room r in rooms.rooms._rooms) 
//            {
//                returnRooms.Add(r);
//            }
//            Debug.Log("server success!: " + jsonResponse);
//            return (returnRooms);
//        }
//        catch (WebException e)
//        {
//            Debug.Log("Connection to Server failes with following message: " + e.Message);
//            Debug.Log("Continuing with already downloaded rooms.");
//            var rooms = new List<Room>();
//            return (rooms);
//        }
//    }

//    public List<Room> GetRoomObjects(List<Room> roomsUpstream)
//    {
//        var roomObjects = new List<Room>();
//        //in case we have no connection to server
//        if (roomsUpstream.Count < 1)
//        {
//            Debug.Log("No upstream rooms available. Using buildindex instead.");
//            foreach (Room room in _roomsLocal)
//            {
//                //set downloaded to true, because room is locally available
//                roomObjects.Add(new Room(room._id, room._name, room._gallery, room._assetBundlePath, room._location, room._downloaded));

//            }
//        }
//        else
//        {
//            foreach (Room room in roomsUpstream)
//            {
//                //check if scene is loaded
//                int localRoomIndex = _roomsLocal.FindIndex(localRoom => localRoom._id == room._id);
//                if (localRoomIndex >= 0)
//                {
//                    roomObjects.Add(_roomsLocal[localRoomIndex]);
//                }
//                else
//                {
//                    //add room even if not downloaded yet.
//                    // room._downloaded defaults to `false`
//                    roomObjects.Add(room);
//                }
//            }
//        }
//        File.WriteAllText(Application.persistentDataPath + "/localRooms.json", JsonUtility.ToJson(new RoomList(roomObjects.Count, roomObjects)).ToString());
//        return (roomObjects);
//    }

//    public void CreateMenuButtons(List<Room> rooms, Transform transform)
//    {
//        var menuButton = Resources.Load<VisualTreeAsset>("UI/menuButton");
//        var rootVisualElement = transform.GetComponent<UIDocument>().rootVisualElement;
//        foreach (Room room in rooms)
//        {
//            VisualElement tempButton = ModifyButton(menuButton.CloneTree(), room);

//            rootVisualElement.Q<VisualElement>("unity-content-container").Add(tempButton);
//            //rootVisualElement.hierarchy.ElementAt(0).hierarchy.ElementAt(1).Add(tempButton);
//            tempButton.RegisterCallback<ClickEvent>(ev => EnterRoom(room, transform, tempButton));
//        }

//    }

//    public VisualElement ModifyButton(VisualElement button, Room room)
//    {
//        //if room is not downloaded, grey the background
//        if (!room._downloaded)
//        {
//            button.hierarchy.ElementAt(0).hierarchy.ElementAt(0).style.backgroundColor = new Color(.9f, .9f, .9f);
//        }
//        button.Q<Label>("name").text = room._name;
//        button.Q<Label>("gallery").text = room._gallery;

//        return (button);
//    }

//    public void EnterRoom(Room room, Transform transform, VisualElement button)
//    {
//        //if (room._bundle && room._bundle.)
//        //{
//        //    //check if vundle exists
//        //    string[] paths = room._bundle.GetAllScenePaths();
//        //    SceneManager.LoadSceneAsync(paths[0]);
//        //    ShowLoadingScreen("Load");
//        //}
//        if (room._downloaded)
//        {
//            transform.gameObject.GetComponent<MenuManager>().StartCoroutine(GetAssetBundle(room, transform));
//            Debug.Log("Loading: " + room._name);
//        }
//        else
//        {
//            Debug.Log("Downloading: " + room._name);
//            transform.gameObject.GetComponent<MenuManager>().StartCoroutine(GetAssetBundle(room, transform));
//            //change button
//            button.Q<Label>("name").text = "downloading";

//            //if download completed:
//            //update the room in the json and write
//            File.WriteAllText(Application.persistentDataPath + "/localRooms.json", JsonUtility.ToJson(new RoomList(_roomObjects.Count, _roomObjects)).ToString());
//        }
//    }

//    public void ShowLoadingScreen(string type, Transform transform)
//    {
//        transform.GetComponent<UIDocument>().sortingOrder = 0;
//        Debug.Log("Show Loading Scene Screen");
//    }

//    IEnumerator GetAssetBundle(Room room, Transform transform)
//    {
//        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(room._assetBundlePath, 1, 0);
//        //Progress bar here !!

//        Debug.Log("Download: " + www.downloadProgress);

//        yield return www.SendWebRequest();

//        if (www.result != UnityWebRequest.Result.Success)
//        {
//            Debug.Log(www.error);
//        }
//        else
//        {
//            room._bundle = DownloadHandlerAssetBundle.GetContent(www);
//            File.WriteAllText(Application.persistentDataPath + "/localRooms.json", JsonUtility.ToJson(new RoomList(_roomObjects.Count, _roomObjects)).ToString());

//            //Experimental !!
//            //SceneManager.LoadSceneAsync(room._name);
//        }

//        //show popup that scene is donwloaded with open and close option
//        // when other scene is open

//        if (room._downloaded)
//        {
//            ShowLoadingScreen("Load", transform);
//            SceneManager.LoadSceneAsync(room._name);
//        }
//        else
//        {
//            room._downloaded = true;
//            Debug.Log("Scene Downloaded, Menu reloading.");
//            File.WriteAllText(Application.persistentDataPath + "/localRooms.json", JsonUtility.ToJson(new RoomList(_roomObjects.Count, _roomObjects)).ToString());
//            SceneManager.LoadSceneAsync("menu");
//        }

//    }

//    public void DeleteCache()
//    {
//        var tempRooms = GetLoadedSceneNames();
//        bool success = Caching.ClearCache();
//        if (success)
//        {
//            foreach (Room room in tempRooms)
//            {
//                room._downloaded = false;
//            }
//            File.WriteAllText(Application.persistentDataPath + "/localRooms.json", JsonUtility.ToJson(new RoomList(tempRooms.Count, tempRooms)).ToString());
//        }
//        else
//        {
//            Debug.Log("Cache not cleanable!!!");
//        }
//    }
//}


//// milestone 1 api
//// rooms over the air
//// user-user interactionen
//// gallerie-user interaction

//// wettbewerb
//// gallerie classico
//// 3d modelle 