using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private Button marbleButton;
    private Button tilesButton;
    private Button scifiButton;


    private void OnEnable()
    {

        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        marbleButton = rootVisualElement.Q<Button>("marbleButton");
        tilesButton = rootVisualElement.Q<Button>("tilesButton");
        scifiButton = rootVisualElement.Q<Button>("scifiButton");

        marbleButton.RegisterCallback<ClickEvent>(ev => marble());
        tilesButton.RegisterCallback<ClickEvent>(ev => tiles());
        scifiButton.RegisterCallback<ClickEvent>(ev => scifi());

    }

    void marble()
    {
        SceneManager.LoadSceneAsync("dark");
    }
    void tiles()
    {
        SceneManager.LoadSceneAsync("tiles");
    }
    void scifi()
    {
        SceneManager.LoadSceneAsync("scifi");
    }
}
