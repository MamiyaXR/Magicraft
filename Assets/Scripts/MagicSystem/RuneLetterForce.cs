using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneLetterForce : RuneLetter
{
    private static RuneLetterForce _instance;
    /*****************************************************************************************
     *
     *
     *
     *****************************************************************************************/
    public static RuneLetterForce Instance
    {
        get
        {
            if (_instance == null)
                _instance = new RuneLetterForce();
            return _instance;
        }
    }
    private RuneLetterForce()
    {
        id = RuneLetterLibary.force;
        pattern = LoadGesturePatterById(id);
    }
    /// <summary>
    /// 接口实现
    /// </summary>
    public override void MagicLaunch(GameObject caster, MagicEventArgs e)
    {
        RaycastHit hit;
        Rigidbody rBody;
        if (Physics.Raycast(e.origin, e.direction, out hit, e.distance * 5f))
        {
            if(hit.collider.tag == "Chunk")
            {
                Vector3 pointInTargetBlock;
                pointInTargetBlock = hit.point + e.direction.normalized * .01f;
                int x, y, z;
                TerrainChunk tc = TerrainManager.instance.GetBlockByPosition(pointInTargetBlock, out x, out y, out z);

                if (tc == null)
                    return;
                float blockPosX = Mathf.FloorToInt(pointInTargetBlock.x) + 0.5f;
                float blockPosY = Mathf.FloorToInt(pointInTargetBlock.y) + 0.5f;
                float blockPosZ = Mathf.FloorToInt(pointInTargetBlock.z) + 0.5f;
                Block block = tc.blocks[x, y, z];
                tc.blocks[x, y, z] = null;
                tc.BuildMesh();

                block.top.linkBlock = null;
                block.bottom.linkBlock = null;
                block.north.linkBlock = null;
                block.south.linkBlock = null;
                block.west.linkBlock = null;
                block.east.linkBlock = null;
                BlockGroup bg = GameObject.Instantiate(GameManager.instance.blockGroupPrefab, 
                                                        new Vector3(blockPosX, blockPosY, blockPosZ), 
                                                        Quaternion.identity).GetComponent<BlockGroup>();
                bg.blocks.Add(block);
                bg.BuildMesh();
                rBody = bg.GetComponent<Rigidbody>();
                if (rBody != null)
                    rBody.AddForce(e.direction * e.magicPower);
                return;
            }
            rBody = hit.collider.GetComponent<Rigidbody>();
            if (rBody != null)
                rBody.AddForce(e.direction * e.magicPower);
        }
    }
}
