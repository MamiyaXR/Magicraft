using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotData
{
    public const int itemNumMax = 99;
    public ItemAsset item;
    [Range(0, itemNumMax)]
    public int itemNum;
}
