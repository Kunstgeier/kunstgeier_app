using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.AI;
using System;

public class RoomBuilder : MonoBehaviour
{
    public Exhibition _exhibition;
    public ArtPieces artworks;
    public Artists _artists;

    List<GameObject> _artObjects;
    [SerializeField]
    GameObject _player;
    [SerializeField]
    GameObject _sceneUI;
    [SerializeField]
    GameObject artistInfo;
    //References
    APIService apiService;

    // Use this for initialization
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        this._artObjects = new List<GameObject>();
        Debug.Log("RoomBuilder started.");
        apiService = GameObject.Find("APIService").GetComponent<APIService>();
        // load information from json
        _exhibition = JsonUtility.FromJson<Exhibition>(File.ReadAllText(Application.persistentDataPath + "/RoomToOpen.json"));
        // download room object if not existing and instantiate
        // download artworks and instantiate as callback
        apiService.GetArtWorksOfExhibition(_exhibition, DownloadAndPlaceArtworks);
        //get artist information
        apiService.GetArtistsOfExhibition(_exhibition, StoreArtists);
    }

    public void StoreArtists(string json)
    {
        _artists = JsonUtility.FromJson<APIReturnParser<Artists>>(json).data;
        Debug.Log("Received arists:" + json);
    }

    public void DownloadAndPlaceArtworks(string artPieces)
    {
        Debug.Log("Download and place artworks called with arg: " + artPieces);
        artworks = JsonUtility.FromJson<APIReturnParser<ArtPieces>>(artPieces).data;
        Debug.Log("Artworks: " + artworks._artworks[0]._name);

        for(int i = 0; i < artworks._artworks.Length; i++)
        {
            Debug.Log("Download and place Artwork: " + artworks._artworks[i]._name);
            if(i == artworks._artworks.Length - 1)
            {
                //last artpiece
                StartCoroutine(apiService.GetArtPieceFile(artworks._artworks[i], PlaceArtworks, true));
            }
            else
            {
                StartCoroutine(apiService.GetArtPieceFile(artworks._artworks[i], PlaceArtworks, false));
            }
        }

        
        // place our player at first pic
        _player.transform.position = GameObject.Find("1").transform.position;
        //find some position on navmesh
        NavMeshHit targetPoint;
        NavMesh.SamplePosition(_player.transform.position, out targetPoint, 2f, NavMesh.AllAreas);
        _player.transform.position = targetPoint.position;
        Debug.Log("Player moved to position one.");
        _player.SetActive(true);
        //activate Lobby functionality
        artistInfo.SetActive(true);
        artistInfo.GetComponent<LobbyController>().Activate();
    }

    public void PlaceArtworks(ArtPiece artpiece, Texture2D artwork, Boolean last)
    {
        // find the position to spawn the artpiece at.
        Debug.Log("Place artworks called.");
        if (artpiece._roomPosition != null)
        {
            var positionObject = GameObject.Find(artpiece._roomPosition);
            if (positionObject != null)
            {
                var artworkObject = Instantiate(Resources.Load("Prefabs/art") as GameObject, positionObject.transform);
                var spriteRenderer = artworkObject.transform.Find("art").GetComponent<SpriteRenderer>();

                spriteRenderer.sprite = Sprite.Create(artwork, new Rect(0.0f, 0.0f, artwork.width, artwork.height), new Vector2(0.5f, 0.5f), 100.0f);
                // scale up the sprite
                spriteRenderer.size *= 10;
                this._artObjects.Add(artworkObject);
                //create artinfo here !!
            }
            else
            {
                Debug.Log("No matching position object found for:" + artpiece._roomPosition);
            }

        }
        else
        {
            Debug.Log("Artwork has no position and will be skipped.");
        }

        //if all art objets are spawned
        if(last)
        {
            _sceneUI.SetActive(true);
        }

    }
}
