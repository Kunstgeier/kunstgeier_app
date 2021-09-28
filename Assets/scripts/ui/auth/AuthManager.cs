using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    [SerializeField]
    private GameObject loginPage;
    [SerializeField]
    private GameObject registerPage;

    APIService apiService;

    // Start is called before the first frame update
    void Start()
    {
        apiService = transform.GetComponent<APIService>();

        string token = apiService.GetToken();

        if(token == "" || token == null)
        {
            registerPage.SetActive(true);
        }
        else
        {
            apiService.CheckToken(token, OpenMenu);
        }
        
        
    }

    public void OpenMenu(string result)
    {
        Debug.Log("Open Menu called with: " + result);
        if (result != null)
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
            else if (authResult.error == false)
            {
                SceneManager.LoadScene("menu");
            }
            else
            {
                Debug.Log(authResult.error.ToString());
                loginPage.SetActive(true);
            }
        }
    }
       
}
