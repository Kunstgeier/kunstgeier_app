using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ToMenu : MonoBehaviour
{
    private Button menuButton;


    private void OnEnable()
    {
        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        menuButton = rootVisualElement.Q<Button>("menuButton");

        menuButton.RegisterCallback<ClickEvent>(ev => menu());

    }

    void menu()
    {
        SceneManager.LoadSceneAsync("menu");
    }
}
