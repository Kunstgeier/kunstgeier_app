using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LoginUser : ApiDataModelBase
{

    public string username;
    public string password;


    public LoginUser(string username, string password)
    {
        this.username = username;
        this.password = password;
    }

    public override string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
