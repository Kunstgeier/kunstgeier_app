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
    public Exhibitions rooms;

    public RoomList(int count, List<Exhibition> roomsList)
    {
        rooms = new Exhibitions(roomsList);
    }
    public RoomList()
    {

    }
    //public static RoomList CreateFromJSON(string json)
    //{
    //    return JsonUtility.FromJson<RoomList>(json);
    //}
}
