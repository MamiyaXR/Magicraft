using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Inventory", fileName = "new Inventory")]
public class InventoryAsset : ScriptableObject
{
    public InventoryID id;
    public bool isLocked;
    public int size;
    public List<SlotData> inventory;
    /*****************************************************************************************
     *
     *
     *
     *****************************************************************************************/
    public override bool Equals(object other)
    {
        if (other is InventoryAsset)
            return id == ((InventoryAsset)other).id;
        else
            return false;
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
public enum InventoryID
{
    Tool = 0,
    Bag,
}
[System.Serializable]
public class SlotData
{
    public const int itemNumMax = 99;
    public ItemAsset item;
    [Range(0, itemNumMax)]
    public int itemNum;
}
