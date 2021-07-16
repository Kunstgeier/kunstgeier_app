using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class AuthObject : MonoBehaviour
{
    string _msg;
    UserData _user;
    string _token;
    public AuthObject(string msg, string user, string token)
    {
        _msg = msg;
        _user = JsonUtility.FromJson<UserData>(user);
    }
}
