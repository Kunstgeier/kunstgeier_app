using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.UI;


public class CreateThumbnailBundles : Editor
{

    string path;
    string dataType;

    void Start()
    {

        path = GetArg("-folder");
        dataType = GetArg("-dataType");

        BuildAssetBundles(path);
    }

    void BuildAssetBundles(string folder)
    {

        foreach (string file in System.IO.Directory.GetFiles(folder))
        {
            if (!file.Contains(".meta"))
            {
                AssetImporter importer = AssetImporter.GetAtPath("Assets/models/" + file);
                if (importer != null)
                {
                    importer.assetBundleName = file;
                    Debug.Log("assetBundlesAssigned");

                }
                else
                {
                    Debug.Log("No asset selected for File : " + file);
                }
            }
        }
    }

    public static void ExecBuildAssetBundles()
    {
        Debug.Log("Building bundle");
        BuildPipeline.BuildAssetBundles("bundles", BuildAssetBundleOptions.None, BuildTarget.iOS);
    }

    private static string GetArg(string name)
    {
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == name && args.Length > i + 1)
            {
                return args[i + 1];
            }
        }
        return null;
    }
}
