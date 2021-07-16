using UnityEngine;
using UnityEditor;
using Proyecto26;
using System.Collections.Generic;
using System;

public class APIService : MonoBehaviour
{
    [SerializeField]
    private bool logger;
    public bool online = true;
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
    public void Get<T>(string route, Action<string> callback, Dictionary<string, string> paramDict = null)
    {
        if (paramDict == null) paramDict = new Dictionary<string, string>();
        // We can add default request headers for all requests
        LogMessage("Token is:", GetToken());
        if(GetToken() != "")
        {
            RestClient.DefaultRequestHeaders["Authorization"] = GetToken();
        }

        //edit URL for parameters
        RequestHelper currentRequest = new RequestHelper
        {
            Uri = _baseURL + route,
            Params = paramDict
        };
        RestClient.Get(currentRequest)
            .Then(res => {
            this.LogMessage("GetRequest got: ", JsonUtility.ToJson(res, true));
            var parsed_return = JsonUtility.FromJson<APIReturnParser<T>>(res.Text);
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


    /// <summary>
    /// General class to start an api call.
    /// </summary>
    /// <param name="route">route without basen url</param>
    /// <param name="method">Get, post, put, delete</param>
    /// <param name="body">body string as json.</param>
    /// <returns></returns>
    //private string MakeAPICall(string route, string method, string body)
    //{
    //    string ret = "";
    //    string url = _baseURL + route;
    //    int status= -1;

    //    switch (method)
    //    {
    //        case "GET":
    //            StartCoroutine(GetRequest(url, "", ret, status) => {
    //                if(status == 0) {
    //                    return ret;
    //                }
    //            });
    //            break;
    //        case "POST":
    //            StartCoroutine(PostRequest(url, body, ret, status) => {
    //                if(status == 0) {
    //                    return ret;
    //                }
    //            });
    //            break;
    //        case "PUT":
    //            StartCoroutine(PutRequest(url, body, ret, status) => {
    //                if(status == 0) {
    //                    return ret;
    //                }
    //            });
    //            break;
    //        default:
    //            return "error";
    //            break;
    //    }
    //}

    ///// <summary>
    ///// send Get Request to server.
    ///// </summary>
    ///// <param name="url">url to Call with base url.</param>
    ///// <param name="body">body in json. if none, empty string</param>
    ///// <param name="ret">the variable to safe the return string in.</param>
    ///// <param name="status">status</param>
    ///// <returns></returns>
    //IEnumerator GetRequest(string url, string body, string ret, int status)
    //{
    //    // decrypt token and build url

    //    url = url + "token=" + GetToken();
    //    using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
    //    {
    //        // Request and wait for the desired page.
    //        yield return webRequest.SendWebRequest();

    //        string[] pages = url.Split('/');
    //        int page = pages.Length - 1;

    //        switch (webRequest.result)
    //        {
    //            case UnityWebRequest.Result.ConnectionError:
    //            case UnityWebRequest.Result.DataProcessingError:
    //                Debug.LogError(pages[page] + ": Error: " + webRequest.error);
    //                status = 1;
    //                break;
    //            case UnityWebRequest.Result.ProtocolError:
    //                Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
    //                status = 2;
    //                break;
    //            case UnityWebRequest.Result.Success:
    //                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
    //                // set the return value
    //                ret = webRequest.downloadHandler.text;
    //                status = 0;
    //                break;
    //        }
    //    }
    //}

    //IEnumerator PostRequest(string url, string body, string ret, int status)
    //{
    //    // decrypt token and build url

    //    url = url + "token=" + GetToken();
    //    using (UnityWebRequest webRequest = UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
    //    {
    //        bytes[] bytes = System.Text.Encoding.UTF8.GetBytes(body);
    //        UploadHandlerRaw uH = new UploadHandlerRaw(bytes);
    //        webRequest.uploadHandler = uH;
    //        // Request and wait for the desired page.
    //        yield return webRequest.SendWebRequest();

    //        string[] pages = url.Split('/');
    //        int page = pages.Length - 1;

    //        switch (webRequest.result)
    //        {
    //            case UnityWebRequest.Result.ConnectionError:
    //            case UnityWebRequest.Result.DataProcessingError:
    //                Debug.LogError(pages[page] + ": Error: " + webRequest.error);
    //                status = 1;
    //                break;
    //            case UnityWebRequest.Result.ProtocolError:
    //                Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
    //                status = 2;
    //                break;
    //            case UnityWebRequest.Result.Success:
    //                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
    //                // set the return value
    //                ret = webRequest.downloadHandler.text;
    //                status = 0;
    //                break;
    //        }
    //    }
    //}

    //IEnumerator PutRequest(string url, string body, string ret, int status)
    //{
    //    // decrypt token and build url

    //    url = url + "token=" + GetToken();

    //    bytes[] bytes = System.Text.Encoding.UTF8.GetBytes(body);
    //    using (UnityWebRequest webRequest = UnityWebRequest.Put(url, bytes))
    //    {
    //        // Request and wait for the desired page.
    //        yield return webRequest.SendWebRequest();

    //        string[] pages = url.Split('/');
    //        int page = pages.Length - 1;

    //        switch (webRequest.result)
    //        {
    //            case UnityWebRequest.Result.ConnectionError:
    //            case UnityWebRequest.Result.DataProcessingError:
    //                Debug.LogError(pages[page] + ": Error: " + webRequest.error);
    //                status = 1;
    //                break;
    //            case UnityWebRequest.Result.ProtocolError:
    //                Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
    //                status = 2;
    //                break;
    //            case UnityWebRequest.Result.Success:
    //                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
    //                // set the return value
    //                ret = webRequest.downloadHandler.text;
    //                status = 0;
    //                break;
    //        }
    //    }
    //}
}

public struct Routes
{
    public static string GetAllRooms = "rooms/getAll";
    public static string GetRooms = "rooms/get";
    public static string GetArtistFromRoom = "rooms/getArtist";
    public static string GetArtworksFromRoom = "rooms/getArtworks";
    public static string Register = "auth/register";
    public static string Login = "auth/login";
    public static string CheckToken = "auth/checkToken";
    //public static string GetAllRooms = "rooms/getAll";
}