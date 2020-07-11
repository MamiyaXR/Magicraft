using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemAsset : ScriptableObject
{
    public ItemID id;
    public Sprite image;
    /*****************************************************************************************
     *
     *
     *
     *****************************************************************************************/
    public override bool Equals(object other)
    {
        Type type = other.GetType();
        if (type.Name != "ItemAsset")
            return false;
        else
            return id == ((ItemAsset)other).id;
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public enum ItemID
{
    Dirt = 0,
    Grass,
    Stone,
    Trunk,
    Leaves
}