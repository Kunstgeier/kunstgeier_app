using System;
[Serializable]
public class UserData
{

    string _id;
    string _nicename;
    string _email;
    string _url;
    string _registerDate;
    string _activationKey;
    string _status;
    string _displayName;


    public UserData(string ID,
                    string user_login,
                    string user_email,
                    string user_url,
                    string user_registered,
                    string user_activatino_key,
                    string user_status,
                    string display_name)
    {
        _id = ID;
        _nicename = user_login;
        _email = user_email;
        _url = user_url;
        _registerDate = user_registered;
        _activationKey = user_activatino_key;
        _status = user_status;
        _displayName = display_name;
    }
}
