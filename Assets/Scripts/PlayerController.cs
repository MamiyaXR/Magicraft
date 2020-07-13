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
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, maxDist, layer))
        {
            if(hitInfo.transform.tag == "Chunk")
            {
                Vector3 pointInTargetBlock;
                pointInTargetBlock = hitInfo.point + ray.direction.normalized * .01f;

                //获得指向的区块chunk (不能用碰撞collider找)
                int chunkPosX = Mathf.FloorToInt(pointInTargetBlock.x / TerrainChunk.chunkWidth) * TerrainChunk.chunkWidth;
                int chunkPosZ = Mathf.FloorToInt(pointInTargetBlock.z / TerrainChunk.chunkLength) * TerrainChunk.chunkLength;
                ChunkPos cp = new ChunkPos(chunkPosX, chunkPosZ);
                TerrainChunk tc = TerrainManager.instance.chunkDict[cp];

                //获得区块内block的下标
                int bix = Mathf.FloorToInt(pointInTargetBlock.x) - chunkPosX + 1;
                int biy = Mathf.FloorToInt(pointInTargetBlock.y);
                int biz = Mathf.FloorToInt(pointInTargetBlock.z) - chunkPosZ + 1;

                //处理
                float itemPosX = Mathf.FloorToInt(pointInTargetBlock.x) + 0.5f;
                float itemPosY = Mathf.FloorToInt(pointInTargetBlock.y) + 0.5f;
                float itemPosZ = Mathf.FloorToInt(pointInTargetBlock.z) + 0.5f;
                BlockType itemType = tc.blocks[bix, biy, biz];
                tc.blocks[bix, biy, biz] = BlockType.Air;
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
