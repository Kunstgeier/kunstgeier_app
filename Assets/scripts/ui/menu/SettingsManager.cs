using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

using System.Net;
using System;
using System.IO;
public class SettingsManager : MonoBehaviour
{
    VisualElement rootVisualElement;
    // Start is called before the first frame update
    void OnEnable()
    {
        rootVisualElement = transform.GetComponent<UIDocument>().rootVisualElement;

        // refactor Menu class to not be a monobehavior 
        //var menuManager = new MenuUtils();
        rootVisualElement.Q<Button>("usernameDisplay").text = "Angemeldet als " + PlayerPrefs.GetString("username", "not found");
        rootVisualElement.Q<Label>("labelExample").text = "Email: " + PlayerPrefs.GetString("email", "not found");



        rootVisualElement.Q<Button>("clearCache").RegisterCallback<ClickEvent>(ev => DeleteCache());
        rootVisualElement.Q<Button>("logout").RegisterCallback<ClickEvent>(ev => Logout());

    }

    private void DeleteCache()
    {
        bool success = Caching.ClearCache();
        if (success)
        {
            rootVisualElement.Q<Button>("clearCache").text = "Speicher bereinigt.";
            Debug.Log("Cache cleaned");
        }
        else
        {
            rootVisualElement.Q<Button>("clearCache").text = "Bereinigung fehlgeschlagen.";
            Debug.Log("Cache NOT cleaned");
        }
    }

    private void Logout()
    {
        Debug.Log("Logout called from settings.");
        PlayerPrefs.DeleteKey ("token");
        PlayerPrefs.DeleteKey("username");
        PlayerPrefs.DeleteKey("email");
        PlayerPrefs.DeleteKey("userID");
        SceneManager.LoadScene("auth");
    }

}
