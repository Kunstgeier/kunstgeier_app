using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ArtInteractionController : MonoBehaviour
{
    private Button instagramButton;
    private Button orderButton;
    private Button closeInfoButton;
    public int tourIndex;
    private GameObject player;

    private TourManager tourManager;
    private int thisWorkIndex;
    // private var playerMovement;

    private void Start()
    {
        //get art index of this table
        tourManager = GameObject.Find("sceneUI").GetComponent<TourManager>();
        thisWorkIndex = tourManager.GetTourIndex(transform.parent.gameObject.transform.Find("snapTarget").gameObject);
    }
    // Start is called before the first frame update
    void Update()
    {
        //close artInfo if player to far away
        if (thisWorkIndex != tourManager.tourIndex)
        {
            Debug.Log("This Work Index" + thisWorkIndex);
            Debug.Log("Current Art Index" + tourManager.tourIndex);

            CloseArtInfo();
        }

    }


    private void OnEnable()
    {
        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        instagramButton = rootVisualElement.Q<Button>("instagramButton");
        orderButton = rootVisualElement.Q<Button>("orderButton");

        instagramButton.RegisterCallback<ClickEvent>(ev => OpenInstagram());
        orderButton.RegisterCallback<ClickEvent>(ev => AddToCart());
        //menuButton 
        closeInfoButton = rootVisualElement.Q<Button>("exitButton");
        closeInfoButton.RegisterCallback<ClickEvent>(ev => CloseArtInfo());
    }

    public void OpenInstagram()
    {
        Debug.Log("Open Instagram is not implemented yet!");
    }
    public void AddToCart()
    {
        Debug.Log("Add to cart is not implemented yet!");
    }


    public void CloseArtInfo()
    {
        Debug.Log("Close Info Button clicked");
        transform.gameObject.SetActive(false);
        Debug.Log("Closed");
    }
}
