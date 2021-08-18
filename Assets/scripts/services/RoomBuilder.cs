using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.AI;

public class RoomBuilder : MonoBehaviour
{
    Exhibition _exhibition;
    ArtPieces _artPieces;

    List<GameObject> _artObjects;
    [SerializeField]
    GameObject _player;
    [SerializeField]
    GameObject _sceneUI;
    //References
    APIService apiService;

    // Use this for initialization
    void Start()
    {
        this._artObjects = new List<GameObject>();
        Debug.Log("RoomBuilder started.");
        apiService = GameObject.Find("APIService").GetComponent<APIService>();
        // load information from json
        _exhibition = JsonUtility.FromJson<Exhibition>(File.ReadAllText(Application.persistentDataPath + "/RoomToOpen.json"));
        // download room object if not existing and instantiate
        // download artworks and instantiate as callback
        StartCoroutine(apiService.GetRoomModel(_exhibition, DownloadAndPlaceArtworks));

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DownloadAndPlaceArtworks(string artPieces)
    {
        Debug.Log("Download and place artworks called with arg: " + artPieces);
        ArtPieces artworks = JsonUtility.FromJson<APIReturnParser<ArtPieces>>(artPieces).data;
        Debug.Log("Artworks: " + artworks._artworks[0]._name);
        foreach (ArtPiece artpiece in artworks._artworks)
        {
            Debug.Log("Download and place Artwork: " + artpiece._name);
            StartCoroutine(apiService.GetArtPieceFile(artpiece, PlaceArtworks));
        }

        // place our player at first pic


        //hide everything else
        // NAVMESH ????!?!?

        //NavMeshHit closestHit;
        //if (NavMesh.SamplePosition(_player.transform.position, out closestHit, 500, 1))
        //{
        //    _player.transform.position = closestHit.position;
        //    
        //}

        Debug.Log(" Set Player active");

        _player.SetActive(true);

        Debug.Log("Player active");


        //_player.transform.position = GameObject.Find(artworks._artworks[0]._roomPosition.ToString()).transform.position;


        // NEVER CALLED..... WHY?

        GameObject.Find("loadingScreen").SetActive(false);
        _sceneUI.SetActive(true);
        //hurrrray
        Debug.Log("scene UI active?");
    }

    public void PlaceArtworks(ArtPiece artpiece, Texture2D artwork)
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

                spriteRenderer.sprite = Sprite.Create(artwork , new Rect(0.0f, 0.0f, artwork.width, artwork.height), new Vector2(0.5f, 0.5f), 100.0f);
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
    }
}
