using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;
using TMPro;

[System.Serializable]
public class FpsProcNpcData {
    public string uuid, fullName, greeting, bldgName, groupId;
    public int faceIndex, bldgFloor;

    private static List<string> greetings = new List<string>(){"Hello.", "Hi!", "What's up?", "Yes?"};

    public FpsProcNpcData(FpsProcDatabase db){
        fullName = db.GetRandomFullName();
        uuid = $"{fullName}_{Random.Range(0, 100_000_000)}";
        faceIndex = FpsProcDatabase.GetRandomFaceIndex();
        greeting = greetings.RandomItem();
    }
}

public class FpsProcNpc : MonoBehaviour
{
    public FpsProcNpcData data;
    public Texture2D face;
    public MeshRenderer faceRenderer;
    [SerializeField] private TMP_Text textName, textJob;

    private void Awake() {
        textName.gameObject.SetActive(false);
        textJob.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start(){
        FpsProcDatabase db = FindObjectOfType<FpsProcDatabase>();

        name = data.fullName;
        textName.text = data.fullName;
        // faceRenderer.material = new Material(faceShader);
        faceRenderer.material.mainTexture = FpsProcDatabase.faces[data.faceIndex];
        faceRenderer.material.color = Color.white;
    }

    public void ClickedOn(){
        FindObjectOfType<FpsProcGameMgr>().ClickNpc(this, data.greeting);
    }

    public void ToggleName(bool show){
        textName.gameObject.SetActive(show);
    }

    public void ToggleJob(bool show, string jobName){
        textJob.text = jobName;
        textJob.gameObject.SetActive(show);
    }
}
