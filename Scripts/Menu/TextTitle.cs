using UnityEngine;
using TMPro;
using ExtensionMethods;

public class TextTitle : MonoBehaviour {
    
    public ECase titleCase;

    void Start()
    {
        GetComponent<TMP_Text>().text = Application.productName.FormatCase(titleCase);
    }
}