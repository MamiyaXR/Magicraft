using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBlockBase : IItemFunction
{
    protected BlockType type;
    public ItemBlockBase()
    {
        type = BlockType.Air;
    }
    public virtual bool FunctionFirst()
    {
        return false;
    }
    public virtual bool FunctionSecond()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//创建一条从主相机到鼠标点的射线
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, PlayerController.instance.maxDist, PlayerController.instance.layer))
        {
            if (hitInfo.transform.tag == "Chunk")
            {
                Vector3 pointInTargetBlock;
                pointInTargetBlock = hitInfo.point - ray.direction.normalized * .01f;

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
                BlockType targetType = tc.blocks[bix, biy, biz];
                if (targetType != BlockType.Air)
                    return false;
                else
                {
                    ToolManager.instance.SubItem();
                    tc.blocks[bix, biy, biz] = type;
                    tc.BuildMesh();
                    return true;
                }
            }
        }
        return false;
    }
}
