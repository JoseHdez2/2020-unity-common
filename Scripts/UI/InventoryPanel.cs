using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class InventoryPanel : MonoBehaviour
{
    public string panelTitle;
    public TMP_Text panelTitleText;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject slotsContainer;
    [SerializeField] private int numberOfSlots;
    private DragDropSlot[] slots;
    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<TMP_Text>().text = panelTitle;
        slots = new DragDropSlot[numberOfSlots];
        for(int i = 0; i < numberOfSlots; i++){
            GameObject gameObj = Instantiate(slotPrefab, new Vector3(), Quaternion.identity, slotsContainer.transform);
            slots[i] = gameObj.GetComponent<DragDropSlot>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(DragDropItem item){
        // DragDropSlot slot = slots.FirstOrDefault(slot => slot.HasItemOfType(item.typeId));
        DragDropSlot firstEmptySlot = slots.FirstOrDefault(slot => slot.itemInSlot == null);
        firstEmptySlot.SetObjectInSlot(item.gameObject);
    }
}
