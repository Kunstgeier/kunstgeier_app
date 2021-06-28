using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

using System.Net;
using System;

[Serializable]
public class ArtInfo
{
    public int _id;
    public string _buyLink;
    public string _fileLink;
    public int _artistID;
    public Artist _artist;
    public string _title;
    public int _year;
    public string _description;
    public int _width;
    public int _height;
    public string _material;
    public int _price;
}