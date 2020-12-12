
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

[System.Serializable]
public class DummyJsonArray {
    [SerializeField] public List<string> values;
}

[System.Serializable]
public class FpsProcQuest {
    string questgiverId, targetId;
    int targetAmount = 1, count = 0;
    bool started = false, finished = false;
}

public class FpsProcDatabase : MonoBehaviour {
    
    public static List<Texture2D> icons, faces;
    public TextAsset firstNamesJsonFile, lastNamesJsonFile, streetNamesJsonFile;
    public static List<string> firstNames, lastNames, streetNames;

    public void Initialize() {
        icons = LoadImgsFromFolder(folder: "lorc");
        faces = LoadImgsFromFolder(folder: "faces");
        firstNames = JsonUtility.FromJson<DummyJsonArray>(firstNamesJsonFile.text).values;
        lastNames = JsonUtility.FromJson<DummyJsonArray>(lastNamesJsonFile.text).values;
        streetNames = JsonUtility.FromJson<DummyJsonArray>(streetNamesJsonFile.text).values;
    }

    private List<Texture2D> LoadImgsFromFolder(string folder){
        return new List<Texture2D>(Resources.LoadAll<Texture2D>(folder));
    }

    public static int GetRandomFaceIndex() => Random.Range(0, faces.Count);
    public static int GetRandomIconIndex() => Random.Range(0, icons.Count);

    public string GetRandomFullName() {
        string lastName = lastNames.RandomItem() + (Random.Range(0,10) == 9 ? $"-{lastNames.RandomItem()}" : "");
        return $"{firstNames.RandomItem()} {lastName}";
    }
}