using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemOnGUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image itemImage = null;
    [SerializeField] private Text itemNum = null;
    public InventoryID inventoryID;
    public SlotData slotData;
    public IItemFunction itemFunction;
    private Transform originalParent;
    private RectTransform rTransform;
    /*****************************************************************************************
     *
     *
     *
     *****************************************************************************************/
    private void Start()
    {
        rTransform = GetComponent<RectTransform>();
    }
    public void Refresh()
    {
        if (slotData == null)
            return;

        if (slotData.item == null)
        {
            itemImage.gameObject.SetActive(false);
            itemNum.gameObject.SetActive(false);
        }
        else
        {
            itemImage.sprite = slotData.item.image;
            itemNum.text = slotData.itemNum.ToString();
            if (itemFunction == null)
            {
                Type t = Type.GetType(slotData.item.typeName);
                itemFunction = Activator.CreateInstance(t) as IItemFunction;
            }
            itemImage.gameObject.SetActive(true);
            itemNum.gameObject.SetActive(true);
        }
    }
    public void Refresh(InventoryID inventoryID, SlotData slotData)
    {
        this.inventoryID = inventoryID;
        this.slotData = slotData;
        Refresh();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = rTransform.parent;
        rTransform.SetParent(GameManager.instance.inventoryDict[inventoryID].gameObject.transform);
        rTransform.position = eventData.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        rTransform.position = eventData.position;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        bool processed = false;
        //拖到UI之外，丢出item（暂：归位）
        if (!processed && eventData.pointerCurrentRaycast.gameObject == null)
        {
            rTransform.SetParent(originalParent);
            rTransform.localPosition = Vector3.zero;
            processed = true;
        }
        //拖到格子，堆叠，交换（暂：只交换）
        if (!processed && eventData.pointerCurrentRaycast.gameObject.name == "Item")
        {
            //交换位置
            rTransform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent);
            rTransform.localPosition = Vector3.zero;
            eventData.pointerCurrentRaycast.gameObject.transform.SetParent(originalParent);
            eventData.pointerCurrentRaycast.gameObject.transform.localPosition = Vector3.zero;
            //交换数据层
            ItemOnGUI target = eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemOnGUI>();
            int targetIndex = GameManager.instance.inventoryDict[target.inventoryID].inventoryAsset.inventory.IndexOf(target.slotData);
            int index = GameManager.instance.inventoryDict[inventoryID].inventoryAsset.inventory.IndexOf(slotData);
            GameManager.instance.inventoryDict[target.inventoryID].inventoryAsset.inventory[targetIndex] = slotData;
            GameManager.instance.inventoryDict[inventoryID].inventoryAsset.inventory[index] = target.slotData;
            //交换显示层
            int targetSlotIndex = GameManager.instance.inventoryDict[target.inventoryID].slotList.IndexOf(target);
            int slotIndex = GameManager.instance.inventoryDict[inventoryID].slotList.IndexOf(this);
            GameManager.instance.inventoryDict[target.inventoryID].slotList[targetSlotIndex] = this;
            GameManager.instance.inventoryDict[inventoryID].slotList[slotIndex] = target;
            //交换库ID
            InventoryID tempID = inventoryID;
            inventoryID = target.inventoryID;
            target.inventoryID = tempID;

            processed = true;
        }
        //其他情况，归位
        if (!processed)
        {
            rTransform.SetParent(originalParent);
            rTransform.localPosition = Vector3.zero;
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
