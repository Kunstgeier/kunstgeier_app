using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class CacheTools : ScriptableObject
{

    [MenuItem("Tools/Cache/Clean")]
    public static void CleanCache()
    {
        if (Caching.ClearCache())
        {
            Debug.LogWarning("Successfully cleaned all caches.");
        }
        else
        {
            Debug.LogWarning("Cache was in use.");
        }
    }
}