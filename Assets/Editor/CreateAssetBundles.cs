using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "AssetBundles";
        string filePath = Path.Combine(Application.streamingAssetsPath, assetBundleDirectory);
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        BuildPipeline.BuildAssetBundles(filePath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

        AssetDatabase.Refresh();
    }
}
