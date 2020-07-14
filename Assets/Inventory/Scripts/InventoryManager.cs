using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public InventoryAsset inventoryAsset;
    public List<ItemOnGUI> slotList = new List<ItemOnGUI>();
    [SerializeField] protected Transform grid;
    [SerializeField] protected GameObject slotPrefab;
    /*****************************************************************************************
     *
     *
     *
     *****************************************************************************************/
    protected virtual void Start()
    {
        for (int i = 0; i < inventoryAsset.size; i++)
        {
            ItemOnGUI slot = Instantiate(slotPrefab, grid).GetComponentInChildren<ItemOnGUI>();
            slotList.Add(slot);
        }
        Refresh();
    }
    public void Refresh()
    {
        for (int i = 0; i < inventoryAsset.size; ++i)
            slotList[i].Refresh(inventoryAsset.id, inventoryAsset.inventory[i]);
    }
    public bool AddItem(ItemAsset itemAsset)
    {
        //锁住
        if (inventoryAsset.isLocked)
            return false;
        //有可堆叠的格子
        for(int i = 0; i < inventoryAsset.size; ++i)
        {
            if(inventoryAsset.inventory[i].item != null &&
                itemAsset.Equals(inventoryAsset.inventory[i].item) &&
                inventoryAsset.inventory[i].itemNum < SlotData.itemNumMax)
            {
                inventoryAsset.inventory[i].itemNum++;
                slotList[i].Refresh(inventoryAsset.id, inventoryAsset.inventory[i]);
                return true;
            }
        }
        //无可堆叠的格子，找空格子
        for(int i = 0; i < inventoryAsset.size; ++i)
        {
            if(inventoryAsset.inventory[i].item == null)
            {
                inventoryAsset.inventory[i].item = itemAsset;
                inventoryAsset.inventory[i].itemNum++;
                slotList[i].Refresh(inventoryAsset.id, inventoryAsset.inventory[i]);
                return true;
            }
        }
        //无处可放
        return false;
    }
}
