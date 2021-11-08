﻿using UnityEngine;
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
using DG.Tweening;

//using UnityEngine.UI;

public class APIService : MonoBehaviour
{
    [SerializeField]
    private bool logger;
    public bool online = true;
    public List<AssetBundle> _loadedBundles;
    


    private readonly string _baseURL = "https://api.kunstgeier.de/dev/";

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
        if (GetToken() != "")
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
            .Then(res =>
            {
                Debug.Log("GetRequest got: " + JsonUtility.ToJson(res.Text, false));
                callback(res.Text);
            })
            .Catch(err => {
                var error = err as RequestException;
                callback(error.Response);
            });
    }

        /// <summary>
        /// Post request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="route"></param>
        /// <param name="body">the request body as dictionary.</param>
        public void Post<T>(string route, T postObject, Action<string> callback) 
    {
        Debug.Log("Post called.");
        //if (paramDict == null) paramDict = new Dictionary<string, string>();
        // We can add default request headers for all requests
        //LogMessage("Token is:", GetToken());

        if (GetToken() != "" && !route.Contains("checkToken"))
        {
            RestClient.DefaultRequestHeaders["Authorization"] = GetToken();
        }

        Debug.Log(JsonUtility.ToJson(postObject));
        //edit URL for parameters
        RequestHelper currentRequest = new RequestHelper
        {
            Uri = _baseURL + route,
            Body = postObject
        };
        Debug.Log(currentRequest.Uri);
        Debug.Log(currentRequest.Body);

        RestClient.Post(currentRequest)
            .Then(res => {
                Debug.Log("PostRequest got: " + JsonUtility.ToJson(res.Text, false));
                callback(res.Text);
            })
            .Catch(err => {
                var error = err as RequestException;
                callback(error.Response);
            });
    }

    
    public void Put<T>(string route, T putObject, Action<string> callback = null) 
    {
        Debug.Log("Put called.");

        //if (paramDict == null) paramDict = new Dictionary<string, string>();
        // We can add default request headers for all requests
        //LogMessage("Token is:", GetToken());
        if (GetToken() != "" && !route.Contains("register"))
        {
            RestClient.DefaultRequestHeaders["Authorization"] = GetToken();
        }
        Debug.Log(JsonUtility.ToJson(putObject));


        //edit URL for parameters
        RequestHelper currentRequest = new RequestHelper
        {
            Uri = _baseURL + route,
            Body = putObject
        };
        Debug.Log(currentRequest.Uri);
        Debug.Log(currentRequest.Body);

        RestClient.Put(currentRequest)
            .Then(res => {
                Debug.Log("PutRequest put: " + JsonUtility.ToJson(res.Text, false));
                callback(res.Text);

            })
            .Catch(err => {
                var error = err as RequestException;
                callback(error.Response);
                });
    }

    public IEnumerator GetButtonThumbnail(string link, UnityEngine.UIElements.VisualElement button)
    {
        Debug.Log("Gettexture at: " + link);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(link, true);
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

            //fade in background image
            DOTween.To(x => button.style.opacity = x, 0, 1, 0.5f);


        }
    }

    public IEnumerator GetArtPieceFile(ArtPiece artpiece, Action<ArtPiece, Texture2D> callback)
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
            callback(artpiece, DownloadHandlerTexture.GetContent(www));
        }
    }

    public IEnumerator GetRoomModel(Exhibition exhibition)
    {
        // Reference to load UI
        UIDocument loadUI = GameObject.Find("loadUI").GetComponent<UIDocument>();
        Label progressLabel = loadUI.rootVisualElement.Q<Label>("progress");
        Debug.Log("Attempting to load: " + exhibition._roomModelLink);
        var downloadScene = Addressables.LoadSceneAsync(exhibition._roomModelLink, LoadSceneMode.Single);

        while (!downloadScene.IsDone)
        {
            var status = downloadScene.GetDownloadStatus();
            var progress = status.Percent * 100;
            progressLabel.text = ((int)(progress)).ToString() + "%";
            Debug.Log("Downloadstatus: " + progress.ToString() +"%");
            yield return null;
        }
        Debug.Log("Download completed.");
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

    public void CheckToken(string token, Action<string> callback)
    {
        Debug.Log("Going to check token.");
        var cargo = new TokenObject(token);
        Post<TokenObject>(Routes.CheckToken, cargo, callback);
    }

    public void CheckUser(string username, Action<string> callback)
    {
        var cargo = new CheckUser(username);
        Post<CheckUser>(Routes.CheckUser, cargo, callback);
    }

    public void LoginUser(string email, string password, Action<string> callback)
    {
        Debug.Log("attempting to log in: " + email);
        var cargo = new LoginUser(email, password);
        Post<LoginUser>(Routes.Login, cargo, callback);
    }

    public void RegisterUser(string email, string password, string username, Action<string> callback)
    {
        Debug.Log("attempting to Register: " + email);
        var cargo = new RegisterUser(username, password, email);
        Post<RegisterUser>(Routes.Register, cargo, callback);
    }

    public string GetToken()
    {
        //decrypt here
        string token = PlayerPrefs.GetString("token", "");
        return token;
    }

    public void SetToken(string token)
    {
        // encrypt here 
        // safe token to playerPrefs
        PlayerPrefs.SetString("token",
                               token);//.Replace(":", (char)34 + ":" + (char)34)) ;
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
    public static string GetAllRooms = "exhibitions.json";
    public static string GetRooms = "exhibitions/#.json";
    public static string GetArtistFromRoom = "exhibitions/#/artists.json";
    public static string GetArtworksFromRoom = "exhibitions/#/artworks.json";
    public static string Register = "auth/register.json";
    public static string Login = "auth/login.json";
    public static string CheckToken = "auth/checkToken.json";
    public static string CheckUser = "auth/checkUser.json";
    public static string CheckEmail = "auth/checkEmail.json";
}
