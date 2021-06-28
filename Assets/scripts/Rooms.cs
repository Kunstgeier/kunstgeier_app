using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

using System.Net;
using System;

[Serializable]
public class Rooms
{
    public int count;
    public Room[] objects;


    public Rooms(int count, List<Room> rooms)
    {
        this.count = count;
        this.objects = new Room[rooms.Count];
        for(int i = 0; i < rooms.Count; i++)
        {
            this.objects[i] = rooms[i];
        }
    }
    //public static RoomList CreateFromJSON(string json)
    //{
    //    return JsonUtility.FromJson<RoomList>(json);
    //}
}
