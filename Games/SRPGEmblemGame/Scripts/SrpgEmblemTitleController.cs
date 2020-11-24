using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SrpgEmblemTitleController : MonoBehaviour
{
    [SerializeField] private TMP_Text textPreview;
    [SerializeField] private Button buttonPlay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetLevel(TextAsset mapJsonFile){
        SrpgMap.data = JsonUtility.FromJson<SrpgMapData>(mapJsonFile.text);
        textPreview.text = string.Join("\n", SrpgMap.data.map);
        buttonPlay.interactable = true;
    }
}
