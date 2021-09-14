using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class TourManager : MonoBehaviour
{
    public List<GameObject> artWorks;
    public GameObject artParent;
    private Button nextButton;
    private Button previousButton;
    private Button menuButton;
    public int tourIndex;
    public GameObject player;
    // private var playerMovement;
    APIService apiService;
    string targetRoomModel;
    string exibition;

    [SerializeField]
    GameObject artistInfo;

    void Start()
    {

        apiService = GameObject.Find("APIService").GetComponent<APIService>();
        /////WHAAAAT ?? hier bin ich
        /// download rooms in menu or in empty scene
        /// get data across??
        /// ref: menumanager
        /// ref: APIService
        /// SOLUTION: Safe Artworks list and Room object to json ie MenuManager.enterroom and reload here
        //download image

        //combine

        //instantiate player prefab at spawn

        //remove loading screen

        // calculate navigation
        player.SetActive(true);

        tourIndex = -1;
        artParent = GameObject.Find("artPositions");
        Debug.Log("Starting to grab artworks.");
        foreach (Transform child in artParent.transform)
        {
            foreach (Transform grandchild in child)
            {
                foreach (Transform greatgrandchild in grandchild)
                {
                    if (greatgrandchild.name == "snapTarget")
                    {
                        artWorks.Add(greatgrandchild.gameObject);
                        Debug.Log("Artwork added " + grandchild.transform.name);
                    }
                }
            }
        }
        //artWorks = new List<GameObject> { GameObject.FindGameObjectsWithTag("snapTarget"));
        Debug.Log("Size of artWorks list: " + artWorks.Count);


        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        nextButton = rootVisualElement.Q<Button>("nextButton");
        previousButton = rootVisualElement.Q<Button>("previousButton");

        nextButton.RegisterCallback<ClickEvent>(ev => Next());
        previousButton.RegisterCallback<ClickEvent>(ev => Previous());
        //menuButton 
        menuButton = rootVisualElement.Q<Button>("menuButton");
        menuButton.RegisterCallback<ClickEvent>(ev => Menu());
    }

    public void Next()
    {
        if (tourIndex >= (artWorks.Count - 1))
        {
            Debug.Log("End of art list reached");
            //set to first picture
            tourIndex = 0;
        }
        else
        {
            tourIndex += 1;
        }
        //player.transform.position = artWorks[tourIndex].transform.position;
        var fps = player.GetComponent<fps>();
        fps.ActivateMoveTo(artWorks[tourIndex].transform);
    }
    public void Previous()
    {
        if (tourIndex <= 0)
        {
            Debug.Log("Start of art list reached");
            //set to last picture
            tourIndex = artWorks.Count - 1;
        }
        else
        {
            tourIndex -= 1;
        }
        //player.transform.position = artWorks[tourIndex].transform.position;
        var fps = player.GetComponent<fps>();

        fps.ActivateMoveTo(artWorks[tourIndex].transform);
    }

    public void SetTourIndex(GameObject targetObject)
    {
        tourIndex = artWorks.IndexOf(targetObject);
    }

    public int GetTourIndex(GameObject targetObject)
    {
        return artWorks.IndexOf(targetObject);
    }

    public void Menu()
    {
        artistInfo.SetActive(true);
        artistInfo.GetComponent<LobbyController>().Activate();
        Debug.Log("Lobby enter called.");
    }
}
