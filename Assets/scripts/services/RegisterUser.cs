using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RegisterUser 
{

    public string username;
    public string password;
    public string email;


    public RegisterUser(string username, string password, string email)
    {
        this.username = username;
        this.password = password;
        this.email = email;
    }
}