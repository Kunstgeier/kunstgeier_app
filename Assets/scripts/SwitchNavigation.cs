using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SwitchNavigation : MonoBehaviour
{
    private Button navModeButton;


    private void OnEnable() 
    {
        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        navModeButton = rootVisualElement.Q<Button>("navModeButton");
        GameObject player = GameObject.FindWithTag("Player");
        
        var playerMovement = player.GetComponent<PlayerMovement>();

        navModeButton.RegisterCallback<ClickEvent>(ev => playerMovement.SwitchNavMode());
    }
}


