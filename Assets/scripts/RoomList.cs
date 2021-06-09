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

    //public static RoomList CreateFromJSON(string json)
    //{
    //    return JsonUtility.FromJson<RoomList>(json);
    //}
}
[Serializable]
public class Rooms
{
    public int count;
    public Room[] objects;

    //public static RoomList CreateFromJSON(string json)
    //{
    //    return JsonUtility.FromJson<RoomList>(json);
    //}
}