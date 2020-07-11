using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemOnGUI : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private Text itemNum;
    private InventoryID inventoryID;
    private SlotData slotData;
    /*****************************************************************************************
     *
     *
     *
     *****************************************************************************************/
    public void Refresh(InventoryID inventoryID, SlotData slotData)
    {
        if (slotData == null)
            return;

        this.inventoryID = inventoryID;
        this.slotData = slotData;

        if (this.slotData.item == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            itemImage.sprite = slotData.item.image;
            itemNum.text = slotData.itemNum.ToString();
            gameObject.SetActive(true);
        }
    }
}
