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
    MenuUtils menuUtils;
    // Start is called before the first frame update
    void Start()
    {
        menuUtils = new MenuUtils();
        var rootVisualElement = transform.GetComponent<UIDocument>().rootVisualElement;
     
        rootVisualElement.Q<Button>("menu").RegisterCallback<ClickEvent>(
            ev => SceneManager.LoadSceneAsync("menu"));


        // refactor Menu class to not be a monobehavior 
        //var menuManager = new MenuUtils();

        rootVisualElement.Q<Button>("clearCache").RegisterCallback<ClickEvent>(
            ev => menuUtils.DeleteCache());
    }

}
