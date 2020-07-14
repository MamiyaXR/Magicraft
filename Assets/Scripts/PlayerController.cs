using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool active = true;
    public float maxDist = 30;
    public LayerMask layer;
    private static PlayerController m_instance;
    public static PlayerController instance { get => m_instance; }
    /*****************************************************************************************
     *
     *
     *
     *****************************************************************************************/
    private void Awake()
    {
        if (m_instance == null)
            m_instance = this;
    }
    private void Update()
    {
        if (!active)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (!ToolManager.instance.CurrentItemFunctionFirst())
                FunctionFirst();
        }
        else if (Input.GetMouseButtonDown(1))
            ToolManager.instance.CurrentItemFunctionSecond();
    }
    /// <summary>
    /// 空手时摧毁方块
    /// </summary>
    public void FunctionFirst()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//创建一条从主相机到鼠标点的射线
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDist, layer))
        {
            if(hit.collider.tag == "Chunk")
            {
                Vector3 pointInTargetBlock;
                pointInTargetBlock = hit.point + ray.direction.normalized * .01f;
                int x, y, z;
                TerrainChunk tc = TerrainManager.instance.GetBlockByPosition(pointInTargetBlock, out x, out y, out z);

                //处理
                if (tc == null)
                    return;
                float itemPosX = Mathf.FloorToInt(pointInTargetBlock.x) + 0.5f;
                float itemPosY = Mathf.FloorToInt(pointInTargetBlock.y) + 0.5f;
                float itemPosZ = Mathf.FloorToInt(pointInTargetBlock.z) + 0.5f;
                BlockType itemType = tc.blocks[x, y, z].type;
                tc.blocks[x, y, z] = null;
                tc.BuildMesh();
                ItemManager.instance.CreateBlockItem(itemType, new Vector3(itemPosX, itemPosY, itemPosZ), Quaternion.identity);
            }
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//创建一条从主相机到鼠标点的射线
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, maxDist, layer))
        {
            Vector3 pointInTargetBlock;
            pointInTargetBlock = hitInfo.point + ray.direction.normalized * 0.1f;
            Gizmos.DrawSphere(pointInTargetBlock, 0.1f);

            float bix = Mathf.FloorToInt(pointInTargetBlock.x) + 0.5f;
            float biy = Mathf.FloorToInt(pointInTargetBlock.y) + 0.5f;
            float biz = Mathf.FloorToInt(pointInTargetBlock.z) + 0.5f;
            Gizmos.DrawCube(new Vector3(bix, biy, biz), new Vector3(1, 1, 1));
        }
    }
#endif
}
