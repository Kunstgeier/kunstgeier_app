using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TokenObject
{

    public string token;


    public TokenObject(string token)
    {
        this.token = token;
    }

    public  string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
