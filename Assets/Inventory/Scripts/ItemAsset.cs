using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemAsset : ScriptableObject
{
    public ItemID id;
    public Sprite image;
    public string typeName;
    /*****************************************************************************************
     *
     *
     *
     *****************************************************************************************/
    public override bool Equals(object other)
    {
        if (other is ItemAsset)
            return id == ((ItemAsset)other).id;
        else
            return false;
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
public interface IItemFunction
{
    bool FunctionFirst();
    bool FunctionSecond();
}