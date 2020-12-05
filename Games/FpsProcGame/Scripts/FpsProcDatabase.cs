
using UnityEngine;

[System.Serializable]
public class DummyJsonArray {
    [SerializeField] public string[] values;
}

public class FpsProcDatabase : MonoBehaviour {
    
    public static Texture2D[] icons;
    public static Texture2D[] faces;
    public TextAsset firstNamesJsonFile, lastNamesJsonFile, streetNamesJsonFile;
    public static string[] firstNames, lastNames, streetNames;

    private void Start() {
        LoadImgsFromFolder(icons, folder: "lorc");
        LoadImgsFromFolder(faces, folder: "faces");
        firstNames = JsonUtility.FromJson<DummyJsonArray>(firstNamesJsonFile.text).values;
        lastNames = JsonUtility.FromJson<DummyJsonArray>(lastNamesJsonFile.text).values;
        streetNames = JsonUtility.FromJson<DummyJsonArray>(streetNamesJsonFile.text).values;
    }

    void LoadImgsFromFolder(Texture2D[] imagesAry, string folder){
        object[] loadedIcons = Resources.LoadAll (folder, typeof(Texture2D)) ;
        imagesAry = new Texture2D[loadedIcons.Length];
        loadedIcons.CopyTo(imagesAry, 0);
    }

    public int GetRandomFaceIndex() => Random.Range(0, faces.Length);
    public int GetRandomIconIndex() => Random.Range(0, icons.Length);
}