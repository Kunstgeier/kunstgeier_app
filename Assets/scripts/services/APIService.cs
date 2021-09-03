using UnityEngine;
using UnityEditor;
using Proyecto26;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

//using UnityEngine.UI;

public class APIService : MonoBehaviour
{
    [SerializeField]
    private bool logger;
    public bool online = true;
    public List<AssetBundle> _loadedBundles;
    //private void Awake()
    //{
    //    DontDestroyOnLoad(this.gameObject);
    //}

    //public void Start()
    //{
    //    LogMessage("Application started", "now trying to connect to server.");
    //    LogMessage("", "");
    //    Get<Rooms>("rooms/getAll", callbackTest<Rooms>);

    //    // get with params
    //    var tempDict = new Dictionary<string, string> {
    //                                                        { "id","2" }
    //                                                  };
    //    Get<Rooms>("rooms/get", callbackTest<Rooms>,tempDict);
    //}
  

    private readonly string _baseURL = "https://api.kunstgeier.de/staging/";

    private void LogMessage(string title, string message)
    {
        if (logger)
        {
#if UNITY_EDITOR
            EditorUtility.DisplayDialog(title, message, "Ok");
#else
		Debug.Log(message);
#endif
        }
        else
        {
            Debug.Log(message);
        }

    }
        /// <summary>
        /// Get request
        /// </summary>
        /// <typeparam name="T">the type we expect from the api.</typeparam>
        /// <typeparam name="route">the sub route of the API.</typeparam>
    public void Get<T>(string route, Action<string> callback, string id = null, Dictionary<string, string> paramDict = null)
    {
        if (paramDict == null) paramDict = new Dictionary<string, string>();
        // We can add default request headers for all requests
        //LogMessage("Token is:", GetToken());
        if(GetToken() != "")
        {
            RestClient.DefaultRequestHeaders["Authorization"] = GetToken();
        }

        //edit URL for parameters
        if (id != null) route = route.Replace("#", id);
        RequestHelper currentRequest = new RequestHelper
        {
            Uri = _baseURL + route,
            Params = paramDict
        };
        RestClient.Get(currentRequest)
            .Then(res => {
            Debug.Log("GetRequest got: " + JsonUtility.ToJson(res.Text, false));
            //var parsed_return = JsonUtility.FromJson<APIReturnParser<T>>(res.Text);
            return res.Text;
        }).Then( res => callback(res));
    }

    /// <summary>
    /// Post request
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="route"></param>
    /// <param name="body">the request body as dictionary.</param>
    public void Post(string route, Dictionary<string, string> body)
    {
        RestClient.DefaultRequestHeaders["Authorization"] = GetToken();

        // need a way of stripping the underscores in front of variables
        RestClient.Post(_baseURL + route , CargoToJson(body))
        .Then(res => this.LogMessage("Success", JsonUtility.ToJson(res, true)))
        .Catch(err => this.LogMessage("Error", err.Message));
    }

    public void Put(string route, Dictionary<string, string> body)
    {
        RestClient.DefaultRequestHeaders["Authorization"] = GetToken();

        RestClient.Put(_baseURL + route, CargoToJson(body))
            .Then(res => this.LogMessage("Success", JsonUtility.ToJson(res, true)))
            .Catch(err => this.LogMessage("Error", err.Message));
    }

    //public void Delete(string route, Dictionary<string, string> body)
    //{
    //    RestClient.DefaultRequestHeaders["Authorization"] = GetToken();


    //    RestClient.Delete(_baseURL + route, (err, res) => {
    //        if (err != null)
    //        {
    //            this.LogMessage("Error", err.Message);
    //        }
    //        else
    //        {
    //            this.LogMessage("Success", "Status: " + res.StatusCode.ToString());
    //        }
    //    });
    //}

    public IEnumerator GetButtonThumbnail(string link, UnityEngine.UIElements.VisualElement button)
    {
        Debug.Log("Gettexture at: " + link);
        UnityWebRequest  www = UnityWebRequestTexture.GetTexture(link, true);
        // Progress bar here !!

        // Debug.Log("Download: " + www.downloadProgress);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            button.Q<Button>("roomCard").style.backgroundImage = Background.FromTexture2D(DownloadHandlerTexture.GetContent(www));
            //button.style.unityBackgroundScaleMode = ScaleMode.ScaleAndCrop;
            //button.style.backgroundImage = Background.FromTexture2D(DownloadHandlerTexture.GetContent(www));
        }
    }

    public IEnumerator GetArtPieceFile(ArtPiece artpiece, Action<ArtPiece,Texture2D, Boolean> callback, Boolean last = false)
    {
        Debug.Log("Get Artworkfile at: " + artpiece._filePath);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(artpiece._filePath, true);
        //Progress bar here !!

        //Debug.Log("Download: " + www.downloadProgress);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("call callback now?: " + DownloadHandlerTexture.GetContent(www));
            callback(artpiece, DownloadHandlerTexture.GetContent(www), last);
            //button.style.unityBackgroundScaleMode = ScaleMode.ScaleAndCrop;
            //button.style.backgroundImage = Background.FromTexture2D(DownloadHandlerTexture.GetContent(www));
        }
    }

    public void GetRoomModel(Exhibition exhibition)
    {
        Debug.Log("Attempting to load: " + exhibition._roomModelLink);
        Addressables.LoadSceneAsync(exhibition._roomModelLink, LoadSceneMode.Single);
    }


    public void GetArtWorksOfExhibition(Exhibition room, Action<string> callback)
    {
        Debug.Log("Get Artworks of Exhibition: " + room._name);
        Get<ArtPieces>(Routes.GetArtworksFromRoom, callback, room._id);
    }

    public void GetArtistsOfExhibition(Exhibition exhibition, Action<string> callback)
    {
        Debug.Log("Get Artworks of Exhibition: " + exhibition._name);
        Get<Artists>(Routes.GetArtistFromRoom, callback, exhibition._id);
    }
    private string GetToken()
    {
        //decrypt here
        string token = PlayerPrefs.GetString("token");
        return token;
    }

    private void SetToken(string token)
    {
        // encrypt here 
        // safe token to playerPrefs
        PlayerPrefs.SetString("token", token);
    }

    public string CargoToJson<T>(T cargo)
    {
        
        string ret = JsonUtility.ToJson(cargo);
        return ret;
    }

    public void callbackTest<T>(string response)
    {
        //rerender menu
        LogMessage("Callback response", response);
        //safe data
        var parsedResponse = JsonUtility.FromJson<APIReturnParser<T>>(response);
        LogMessage("Parsed resonse meta msg: ", parsedResponse.meta.msg);
        //
    }

    public void IsOnline()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            online = false;
            Debug.Log("Error. Check internet connection!");
        }
    }
}

public struct Routes
{
    public static string GetAllRooms = "exhibitions";
    public static string GetRooms = "exhibitions/#";
    public static string GetArtistFromRoom = "exhibitions/#/artists";
    public static string GetArtworksFromRoom = "exhibitions/#/artworks";
    public static string Register = "auth/register";
    public static string Login = "auth/login";
    public static string CheckToken = "auth/checkToken";
    //public static string GetAllRooms = "rooms/getAll";
}