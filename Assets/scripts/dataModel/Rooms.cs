using System.Collections.Generic;
using System;

[Serializable]
public class Rooms
{
    public Room[] _rooms;


    public Rooms(List<Room> rooms)
    {
        this._rooms = new Room[rooms.Count];
        for(int i = 0; i < rooms.Count; i++)
        {
            this._rooms[i] = rooms[i];
        }
    }
}
