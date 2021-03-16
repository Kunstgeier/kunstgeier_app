using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SwitchNavigation : MonoBehaviour
{
    private Button switchTFButton;


    private void OnEnable() 
    {
        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        switchTFButton = rootVisualElement.Q<Button>("navModeButton");
        GameObject player = GameObject.FindWithTag("Player");
        
        var playerMovement = player.GetComponent<PlayerMovement>();

        switchTFButton.RegisterCallback<ClickEvent>(ev => playerMovement.switchNavMode());
        // playerMovement.switchNavMode();
    }
}


