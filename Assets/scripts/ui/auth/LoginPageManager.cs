using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LoginPageManager : MonoBehaviour
{
    APIService apiService;
    [SerializeField]
    private GameObject RegisterPage;
    AuthManager authManager;

    VisualElement loginRootVisualElement;


    private void OnEnable()
    {
        authManager = GameObject.Find("UI").GetComponent<AuthManager>();
        apiService = GameObject.Find("UI").GetComponent<APIService>();

        loginRootVisualElement = transform.GetComponent<UIDocument>().rootVisualElement;

        //fade in menu and tabbar
        DOTween.To(x => loginRootVisualElement.style.opacity = x, 0, 1, .5f);

        //callbacks for buttons
        loginRootVisualElement.Q<Button>("toRegisterPage").RegisterCallback<ClickEvent>(
            ev => ToRegisterPage());

        loginRootVisualElement.Q<Button>("loginButton").RegisterCallback<ClickEvent>(
            ev => PrepareLogin());

    }
    private void PrepareLogin()
    {
        var usernameOrEmail = loginRootVisualElement.Q<TextField>("usernameOrEmail").value;
        var password = loginRootVisualElement.Q<TextField>("password").value;

        Debug.Log(usernameOrEmail + " with " + password);

        apiService.LoginUser(usernameOrEmail, password, LoginCallback);
    }


    public void ToRegisterPage()
    {
        RegisterPage.SetActive(true);
        transform.gameObject.SetActive(false);
    }

    public void LoginCallback(string response)
    {
        var auth = JsonUtility.FromJson<APIReturnParser<AuthReturnParser>>(response).data;
        if (auth._auth.msg == "Authentication was good")
        {
            apiService.SetToken(auth._auth.token);
            //safe user data
            authManager.OpenMenu(response);
        }
        else
        {
            Debug.Log("Login failed due to unknown reason.");
            ToRegisterPage();
        }
    }
}
