using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    private Button marble;
    private Button tiles;
    private Button scifi;
    private Button fiona;
    private Button fionaPromo;
    // private var playerMovement;


    private void OnEnable()
    {
        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        marble = rootVisualElement.Q<Button>("Marmorsaal");
        scifi = rootVisualElement.Q<Button>("Raumschiff");

        marble.RegisterCallback<ClickEvent>(ev => MarbleFunc());
        scifi.RegisterCallback<ClickEvent>(ev => scifiFunc());
        //menuButton 
        tiles = rootVisualElement.Q<Button>("Fliesenhalle");
        tiles.RegisterCallback<ClickEvent>(ev => TilesFunc());

        fiona = rootVisualElement.Q<Button>("FionasRaum");
        fiona.RegisterCallback<ClickEvent>(ev => FionasFunc());

        fionaPromo = rootVisualElement.Q<Button>("FionasRaumPromo");
        fionaPromo.RegisterCallback<ClickEvent>(ev => FionasPromoFunc());
    }

    public void MarbleFunc()
    {
        SceneManager.LoadSceneAsync("marble");
        Debug.Log("Pointer down to marble.");
    }
    public void scifiFunc()
    {
        SceneManager.LoadSceneAsync("scifi");
        Debug.Log("Pointer down to scifi.");
    }
 
    public void TilesFunc()
    {
        SceneManager.LoadSceneAsync("tiles");
        Debug.Log("Pointer down to tiles.");
    }

    public void FionasFunc()
    {
        SceneManager.LoadSceneAsync("fionasRoom");
        Debug.Log("Pointer down to Fionas Room.");
    }
    public void FionasPromoFunc()
    {
        SceneManager.LoadSceneAsync("fionasRoomPromo");
        Debug.Log("Pointer down to Fionas Promo Room.");
    }
}

