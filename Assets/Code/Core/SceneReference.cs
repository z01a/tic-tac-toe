using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class SceneReference
{
#if UNITY_EDITOR
    [Tooltip("Reference to the scene asset")]
    [SerializeField]
    public SceneAsset sceneAsset;
#endif

    [HideInInspector]
    public string SceneName;

#if UNITY_EDITOR
    public void OnValidate()
    {
        if (sceneAsset != null)
        {
            string path = AssetDatabase.GetAssetPath(sceneAsset);
            SceneName = System.IO.Path.GetFileNameWithoutExtension(path);
        }
    }
#endif
}