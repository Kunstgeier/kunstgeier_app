using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TourManager : MonoBehaviour
{
    public List<GameObject> artWorks;
    public GameObject artParent;
    private Button nextButton;
    private Button previousButton;
    private int tourIndex;
    private GameObject player;
    // private var playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        
        tourIndex = -1;
        artParent = GameObject.Find("ArtWork");
         foreach (Transform child in artParent.transform)
         {
             foreach(Transform grandchild in child)
             {
                if (grandchild.tag == "artCam")
                    {
                        artWorks.Add(grandchild.gameObject);
                    }
             }
         }
         Debug.Log("Size of artWorks list: " + artWorks.Count);
    }


    private void OnEnable() 
    {
        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        nextButton = rootVisualElement.Q<Button>("nextButton");
        previousButton = rootVisualElement.Q<Button>("previousButton");

        nextButton.RegisterCallback<ClickEvent>(ev => next());
        previousButton.RegisterCallback<ClickEvent>(ev => previous());
    }

    void next(){
        if (tourIndex >= (artWorks.Count-1))
        {
            Debug.Log("End of art list reached");
            return;
        }
        var playerMovement = player.GetComponent<PlayerMovement>();
        tourIndex += 1;
        playerMovement.snapToObject(artWorks[tourIndex]);
    }
    void previous(){
        if (tourIndex <= 0)
        {
            Debug.Log("Start of art list reached");
            return;
        }
        var playerMovement = player.GetComponent<PlayerMovement>();
        tourIndex -= 1;
        playerMovement.snapToObject(artWorks[tourIndex]);
    }
    public void setTourIndex(GameObject targetObject)
    {
        tourIndex = artWorks.IndexOf(targetObject);
    }
}

