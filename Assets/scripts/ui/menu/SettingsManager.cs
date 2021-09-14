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
    void Start()
    {
        rootVisualElement = transform.GetComponent<UIDocument>().rootVisualElement;

        // refactor Menu class to not be a monobehavior 
        //var menuManager = new MenuUtils();

        rootVisualElement.Q<Button>("clearCache").RegisterCallback<ClickEvent>(ev => DeleteCache());
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

}
