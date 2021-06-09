using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class CreateAssetBundles : Editor
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles("/Users/simsalabim/Development/kunstgeier/bundles", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.iOS);
    }
}
