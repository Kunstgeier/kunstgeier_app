using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class MsgScreenController : MonoBehaviour
{
    
    // Start is called before the first frame update
    void OnEnable()
    {
        var rootVisualElement = transform.GetComponent<UIDocument>().rootVisualElement;
        rootVisualElement.Q<Button>("okButton").RegisterCallback<ClickEvent>(
            ev => transform.gameObject.SetActive(false));
        DOTween.To(x => rootVisualElement.style.opacity = x, 0, 1, .5f);

    }

}
