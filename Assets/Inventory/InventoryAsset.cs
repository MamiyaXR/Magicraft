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
        Type type = other.GetType();
        if (type.Name != "InventoryAsset")
            return false;
        else
            return id == ((InventoryAsset)other).id;
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
