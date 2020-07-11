
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject bloclItemPrefab;
    //注意顺序
    public List<ItemAsset> itemAssetList;
    private const int itemNumMax = 4096;
    private List<GameObject> items = new List<GameObject>();
    private static ItemManager _instance;
    public static ItemManager instance { get => _instance; }
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
    private void Update()
    {
        if(items.Count > itemNumMax)
        {
            GameObject temp = items[0];
            items.RemoveAt(0);
            Destroy(temp);
        }
    }
    public GameObject CreateBlockItem(BlockType type, Vector3 position, Quaternion rotation)
    {
        GameObject result = Instantiate(bloclItemPrefab, position, rotation);
        BlockItem blockItem = result.GetComponent<BlockItem>();
        blockItem.type = type;
        blockItem.itemAsset = itemAssetList[(int)type - 1];
        blockItem.InitBlockItem();
        blockItem.AddForce();
        items.Add(result);
        return result;
    }
}
