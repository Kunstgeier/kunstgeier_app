using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CheckUser : ApiDataModelBase
{

    public string username;


    public CheckUser(string username)
    {
        this.username = username;
    }

    public override string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
