using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEditorInternal;
using UnityEngine;

public class SetupUnityCommon
{
    [MenuItem("Tools/Setup Unity Common")]
    public static void Setup()
    {
        UnityEditor.PlayerSettings.companyName = "yellowlime";
        UnityEditor.PlayerSettings.bundleVersion = "0.1";
        AddPackage("com.unity.cinemachine");
        AddPackage("com.unity.textmeshpro");
        // UnityEditor.AssetDatabase.ImportPackage("com.unity.cinemachine", interactive: false);
        // UnityEditor.EditorSettings.serializationMode = UnityEditor.SerializationMode.ForceText;
        // UnityEditor.EditorSettings.externalVersionControl = "Visible Meta Files";
    }

    static AddRequest Request;

    static void AddPackage(string packageName)
    {
        // Add a package to the project
        Request = Client.Add(packageName);
        EditorApplication.update += Progress;
    }

    static void Progress()
    {
        if (Request.IsCompleted)
        {
            if (Request.Status == StatusCode.Success)
                Debug.Log("Installed: " + Request.Result.packageId);
            else if (Request.Status >= StatusCode.Failure)
                Debug.Log(Request.Error.message);

            EditorApplication.update -= Progress;
        }
    }
    public static void CreateTag(string s){
        // Open tag manager
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        // For Unity 5 we need this too
        SerializedProperty layersProp = tagManager.FindProperty("layers");

        // First check if it is not already present
        bool found = false;
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(s)) { found = true; break; }
        }

        // if not found, add it
        if (!found)
        {
            tagsProp.InsertArrayElementAtIndex(0);
            SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
            n.stringValue = s;
        }

        // Setting a Layer (Let's set Layer 10)
        string layerName = "the_name_want_to_give_it";

        // --- Unity 4 ---
        // SerializedProperty sp = tagManager.FindProperty("User Layer 10");
        // if (sp != null) sp.stringValue = layerName;

        // // --- Unity 5 ---
        // SerializedProperty sp = layersProp.GetArrayElementAtIndex(10);
        // if (sp != null) sp.stringValue = layerName;
        // // and to save the changes
        // tagManager.ApplyModifiedProperties();
    }
}