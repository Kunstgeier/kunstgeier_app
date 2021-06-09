using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artist : MonoBehaviour
{
    string _name;
    string _gallery;
    string _instagramLink;
    string _location;

    public Artist(string name, string gallery, string instagramLink, string location)
    {
        _name = name;
        _gallery = gallery;
        _instagramLink = instagramLink;
        _location = location;
    }
    
}
