using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStarter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Scene Starter started!");
        Instantiate(Resources.Load<GameObject>("Prefabs/sceneStart"));
    }

}
