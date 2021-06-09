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

        marbleButton.RegisterCallback<PointerUpEvent>(ev => marble());
        tilesButton.RegisterCallback<PointerUpEvent>(ev => tiles());
        scifiButton.RegisterCallback<PointerUpEvent>(ev => scifi());
    }

    public void marble()
    {
        SceneManager.LoadSceneAsync("dark");
    }
    public void tiles()
    {
        SceneManager.LoadSceneAsync("tiles");
    }
    public void scifi()
    {
        SceneManager.LoadSceneAsync("scifi");
    }
}
