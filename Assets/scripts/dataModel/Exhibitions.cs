using System.Collections.Generic;
using System;

[Serializable]
public class Exhibitions
{
    public Exhibition[] _exhibitions;


    public Exhibitions(List<Exhibition> rooms)
    {
        this._exhibitions = new Exhibition[rooms.Count];
        for(int i = 0; i < rooms.Count; i++)
        {
            this._exhibitions[i] = rooms[i];
        }
    }
}
