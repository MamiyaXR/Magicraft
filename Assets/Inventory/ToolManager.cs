using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : InventoryManager
{
    public RectTransform highLight;
    private static ToolManager _instance;
    public static ToolManager instance { get => _instance; }
    /*****************************************************************************************
     *
     *
     *
     *****************************************************************************************/
    private void Awake()
    {
        if(_instance == null)
            _instance = this;
    }
    protected override void Start()
    {
        base.Start();
        highLight.transform.SetParent(slotList[0].transform);
        highLight.anchoredPosition = new Vector2(0, -16);
    }
}
