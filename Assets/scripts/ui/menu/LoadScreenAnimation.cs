using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class LoadScreenAnimation : MonoBehaviour
{
    VisualElement rootVisualElement;
    Button logo;
    // Start is called before the first frame update
    void OnEnable()
    {
        rootVisualElement = transform.GetComponent<UIDocument>().rootVisualElement;
        logo = rootVisualElement.Q<Button>("LOGO");
        DOTween.To(x => logo.style.opacity = x, 0, 1, .5f).SetLoops(-1, LoopType.Yoyo);
    }
}
