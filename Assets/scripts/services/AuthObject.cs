using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class AuthObject
{
    public string msg;
    public UserData user;
    public string token;
    //public AuthObject(string msg, string user, string token)
    //{
    //    _msg = msg;
    //    _user = JsonUtility.FromJson<UserData>(user);
    //    _token = token;
    //}
}
