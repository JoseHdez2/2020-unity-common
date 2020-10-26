using UnityEngine;
using TMPro;
using ExtensionMethods;

public class TextVersion : MonoBehaviour {

    void Start()
    {
        GetComponent<TMP_Text>().text = $"ver. {Application.version}";
    }
}