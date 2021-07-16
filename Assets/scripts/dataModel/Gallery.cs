using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gallery : MonoBehaviour
{
    string _name;
    string[] _artists;
    string _instagramLink;
    string _webLink;
    string _location;

    public Gallery(string name, string[] artists, string webLink, string location)
    {
        _name = name;
        _artists = artists;
        _webLink = webLink;
        _location = location;
    }
}
