using System;
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
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, PlayerController.instance.maxDist, PlayerController.instance.layer))
        {
            if (hit.collider.tag == "Chunk")
            {
                Vector3 pointInTargetBlock;
                pointInTargetBlock = hit.point - ray.direction.normalized * .01f;
                int x, y, z;
                TerrainChunk tc = TerrainManager.instance.GetBlockByPosition(pointInTargetBlock, out x, out y, out z);

                //处理
                if (tc == null || tc.blocks[x, y, z] != null)
                    return false;
                else
                {
                    ToolManager.instance.SubItem();
                    Type t = Type.GetType(Enum.GetName(typeof(BlockType), type));
                    tc.blocks[x, y, z] = Activator.CreateInstance(t) as Block;
                    tc.BuildMesh();
                    return true;
                }
            }
        }
        return false;
    }
}
