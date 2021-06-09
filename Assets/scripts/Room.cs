using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

using System.Net;
using System;
using System.IO;

[Serializable]
public class Room
{
    public string _id;
    public string _name;
    public string _gallery;
    public string _assetBundlePath;
    public string _location;
    public bool _downloaded;


    //use if not downloaded, probably coming in from server
    public Room(string id, string name, string gallery, string assetBundlePath, string location)
    {
        _id = id;
        _name = name;
        _gallery = gallery;
        _assetBundlePath = Path.GetFullPath(assetBundlePath);
        _location = location;
        _downloaded = false;
        // more to come here


    }

    //use if already downloaded
    public Room(string id, string name, string gallery, string assetBundlePath, string location, bool downloaded)
    {
        _id = id;
        _name = name;
        _gallery = gallery;
        _assetBundlePath = Path.GetFullPath(assetBundlePath);
        _location = location;
        _downloaded = downloaded;
        // more to come here
    }
}