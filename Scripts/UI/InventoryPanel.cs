using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryPanel : MonoBehaviour
{
    public string panelTitle;
    public TMP_Text panelTitleText;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject slotsContainer;
    [SerializeField] private int numberOfSlots;
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<TMP_Text>().text = panelTitle;
        for(int i = 0; i < numberOfSlots; i++){
            Instantiate(slotPrefab, new Vector3(), Quaternion.identity, slotsContainer.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
