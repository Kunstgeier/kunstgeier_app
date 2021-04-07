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

        //nextButton.RegisterCallback<ClickEvent>(ev => next());
        //previousButton.RegisterCallback<ClickEvent>(ev => previous());
        //menuButton 
        menuButton = rootVisualElement.Q<Button>("menuButton");
        menuButton.RegisterCallback<ClickEvent>(ev => menu());
    }

    //public void next()
    //{
    //    if (tourIndex >= (artWorks.Count - 1))
    //    {
    //        Debug.Log("End of art list reached");
    //        return;
    //    }
    //    var playerTransform = player.GetComponent<Transform>();
    //    tourIndex += 1;
    //    playerTransform.position = artWorks[tourIndex].transform.position;
    //}
    //public void previous()
    //{
    //    if (tourIndex <= 0)
    //    {
    //      Debug.Log("Start of art list reached");
    //        return;
    //    }
    //    var playerTransform = player.GetComponent<Transform>();
    //    tourIndex -= 1;
    //    playerTransform.position = artWorks[tourIndex].transform.position;
    //}
    public void setTourIndex(GameObject targetObject)
    {
        tourIndex = artWorks.IndexOf(targetObject);
    }

    public void menu()
    {
        SceneManager.LoadSceneAsync("menu");
        Debug.Log("Pointer down to menu.");
    }
}

