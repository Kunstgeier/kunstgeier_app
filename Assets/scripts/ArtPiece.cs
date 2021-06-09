using System.Collections;
using System.Collections.Generic;
using System.IO; //for path class

using UnityEngine;

public class ArtPiece : MonoBehaviour
{

    private string _name;
    private Artist _artist;
    private Gallery _gallery;
    private string _description;
    private string _imagePath;

    // constructor
    public ArtPiece(string name, Artist artist, Gallery gallery, string description, string imagePath)
    {
          _name = name;
          _artist = artist;
          _gallery = gallery;
          _description = description;
          _imagePath = imagePath;
    }

}
