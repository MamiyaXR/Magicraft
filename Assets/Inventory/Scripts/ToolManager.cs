using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToolManager : InventoryManager
{
    public RectTransform highLight;
    private int highLightIndex = 0;
    private Vector2 highLightOffset = new Vector2(0, -5);
    private static ToolManager _instance;
    public static ToolManager instance { get => _instance; }
    /*****************************************************************************************
     *
     *
     *
     *****************************************************************************************/
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
    protected override void Start()
    {
        base.Start();
        MoveHighLight();
    }
    private void Update()
    {
        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        if (scroll > 0)
        {
            highLightIndex--;
            if (highLightIndex < 0)
                highLightIndex = slotList.Count - 1;
            MoveHighLight();
        }
        else if (scroll < 0)
        {
            highLightIndex++;
            if (highLightIndex >= slotList.Count)
                highLightIndex = 0;
            MoveHighLight();
        }
    }
    /// <summary>
    /// 改变高亮位置
    /// </summary>
    private void MoveHighLight()
    {
        highLight.transform.SetParent(slotList[highLightIndex].transform.parent);
        highLight.anchoredPosition = highLightOffset;
        highLight.SetAsFirstSibling();
    }
    public bool CurrentItemFunctionFirst()
    {
        ItemOnGUI curItem = highLight.parent.GetComponentInChildren<ItemOnGUI>();
        if (curItem == null ||
            curItem.itemFunction == null)
            return false;
        else
            return curItem.itemFunction.FunctionFirst();
    }
    public bool CurrentItemFunctionSecond()
    {
        ItemOnGUI curItem = highLight.parent.GetComponentInChildren<ItemOnGUI>();
        if (curItem == null ||
            curItem.itemFunction == null)
            return false;
        else
            return curItem.itemFunction.FunctionSecond();
    }
    public void SubItem()
    {
        ItemOnGUI curItem = highLight.parent.GetComponentInChildren<ItemOnGUI>();
        if (curItem == null ||
            curItem.slotData == null)
            return;

        curItem.slotData.itemNum -= 1;
        if (curItem.slotData.itemNum <= 0)
        {
            curItem.slotData.item = null;
            curItem.itemFunction = null;
        }
        curItem.Refresh();
    }
}
