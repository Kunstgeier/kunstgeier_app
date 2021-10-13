using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;
using System.Text.RegularExpressions;

public class RegisterPageManager : MonoBehaviour
{
    APIService apiService;
    [SerializeField]
    private GameObject LoginPage;

    VisualElement registerRootVisualElement;

    AuthManager authManager;

    private void OnEnable()
    {
        TouchScreenKeyboard.hideInput = true;

        authManager = GameObject.Find("UI").GetComponent<AuthManager>();
        authManager.Loading = true;

        apiService = GameObject.Find("UI").GetComponent<APIService>();

        registerRootVisualElement = transform.GetComponent<UIDocument>().rootVisualElement;

        //fade in menu and tabbar
        DOTween.To(x => registerRootVisualElement.style.opacity = x, 0, 1, .5f);

        //callbacks for buttons
        registerRootVisualElement.Q<Button>("toLoginPage").RegisterCallback<ClickEvent>(
            ev => ToLoginPage());

        registerRootVisualElement.Q<Button>("registerButton").RegisterCallback<ClickEvent>(
            ev => PrepareRegister());

        authManager.Loading = false;
        
    }


    private void PrepareRegister()
    {
        authManager.Loading = true;
        var username = registerRootVisualElement.Q<TextField>("username").value;
        var email = registerRootVisualElement.Q<TextField>("email").value;
        var password1 = registerRootVisualElement.Q<TextField>("password1").value;
        var password2 = registerRootVisualElement.Q<TextField>("password2").value;

        string MatchEmailPattern =
            @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
            + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
              + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
            + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

        if (username == null || username == "")
        {
            authManager.DisplayAuthMessage("Bitte einen Username eingeben.");
            return;
        }
        else if (!Regex.IsMatch(email, MatchEmailPattern))
        {
            authManager.DisplayAuthMessage("Bitte eine valide Email eingeben.");
            return;
        }
        else if(password1 == "" || password2 == "")
        {
            authManager.DisplayAuthMessage("Bitte beide Passwörter eingeben.");
            return;

        }
        else if (password1 != password2)
        {
            authManager.DisplayAuthMessage("Passwörter stimmen nicht überein.");
            return;
        }
        
        apiService.RegisterUser(email, password1, username, RegisterCallback);
    }
    public void ToLoginPage()
    {
        LoginPage.SetActive(true);
        transform.gameObject.SetActive(false);
    }

    public void CheckUser(string username)
    {
        apiService.CheckUser(username, FeedbackUserCheckResult);
    }

    public void FeedbackUserCheckResult(string result)
    {

    }

    public void RegisterCallback(string response)
    {
        try
        {
            var parsed = JsonUtility.FromJson<APIReturnParser<AuthReturnParser>>(response);

            var auth = parsed.data._auth;

            if (auth.msg == "Registration was successful")
            {
                apiService.SetToken(auth.token);
                //safe user data
                authManager.OpenMenu(response);
            }
            else if (parsed.error != null )
            {
                authManager.DisplayAuthMessage(parsed.error.message);
                authManager.Loading = false;
                return;
            }
            return;
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
        }

        Debug.Log("Registration failed due to unknown reason.");
        authManager.DisplayAuthMessage("Registrierung fehlgeschlagen.");

        Debug.Log(response);
        authManager.Loading = false;
        return;
    }
}
