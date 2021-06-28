using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

using System.Net;
using System;
using System.IO;

[Serializable]
public class RoomList
{
    public Rooms rooms;

    public RoomList(int count, List<Room> roomsList)
    {
        rooms = new Rooms(count, roomsList);
    }
    public RoomList()
    {

    }
    //public static RoomList CreateFromJSON(string json)
    //{
    //    return JsonUtility.FromJson<RoomList>(json);
    //}
}
