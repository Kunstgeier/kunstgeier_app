using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;

public class AuthManager : MonoBehaviour
{
    [SerializeField]
    private GameObject loginPage;
    [SerializeField]
    private GameObject registerPage;

    [SerializeField]
    private GameObject loadingScreen;
    [SerializeField]
    private GameObject msgScreen;
    
    APIService apiService;

    private bool loading = true;

    // Start is called before the first frame update
    void Start()
    {
        //TouchScreenKeyboard.hideInput = true;

        apiService = transform.GetComponent<APIService>();

        string token = apiService.GetToken();

        if (token == "" || token == null)
        {
            registerPage.SetActive(true);
        }
        else
        {
            apiService.CheckToken(token, OpenMenu);
        }

        Loading = false;
    }

    public void OpenMenu(string result)
    {
        loading = true;
        Debug.Log("Open Menu called with: " + result);
        if (result != null)
        {
            try
            {
                var authResult = JsonUtility.FromJson<APIReturnParser<AuthReturnParser>>(result);

                if (authResult.data._auth.user.display_name != null)
                //if we received a username and so one, we login and not only checked the token.
                {
                    var user = authResult.data._auth.user;
                    PlayerPrefs.SetString("username", user.display_name);
                    PlayerPrefs.SetString("email", user.user_email);
                    PlayerPrefs.SetString("userID", user.ID);
                    SceneManager.LoadScene("menu");
                }
                else if (authResult.meta.msg == "success")
                {
                    SceneManager.LoadScene("menu");
                }
                else
                {
                    Debug.Log(authResult.error.message);
                    loginPage.SetActive(true);
                }
                return;
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                loginPage.SetActive(true);
                return;
            }
        }
        else
        {
            registerPage.SetActive(true);
            return;
        }
    }

    public bool Loading
    {
        get { return loading; }
        set
        {
            if (value == loading)
                return;

            loading = value;
            if (loading)
            {
                loadingScreen.SetActive(true);
            }
            else
            {
                loadingScreen.SetActive(false);
            }

        }
    }

    public void DisplayAuthMessage(string msg = null)
    {
        if(msg == null || msg == "")
        {
            return;
        }
        msgScreen.SetActive(true);
        var rootVisualElement = msgScreen.GetComponent<UIDocument>()
                                            .rootVisualElement;
        rootVisualElement.Q<Label>("msg").text = msg;
        Loading = false;
    }
}

