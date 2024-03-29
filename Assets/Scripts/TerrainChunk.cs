﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunk : MonoBehaviour
{
    //区块范围
    public const int chunkWidth = 16;
    public const int chunkLength = 16;
    public const int chunkHeight = 64;
    public Block[,,] blocks = new Block[chunkWidth + 2, chunkHeight, chunkLength + 2];
    /*****************************************************************************************
     *
     *
     *
     *****************************************************************************************/
    public void BuildMesh()
    {
        Mesh mesh = new Mesh();
        //顶点坐标list，三角形list，uv坐标list
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        
        for (int x = 1; x < chunkWidth + 1; ++x)
        {
            for(int z = 1; z < chunkLength + 1; ++z)
            {
                for(int y = 0; y < chunkHeight; ++y)
                {
                    if(blocks[x,y,z] != null)
                    {
                        int faceNum = 0;
                        Vector3 blockPos = new Vector3(x - 1, y, z - 1);
                        //top
                        if(y == chunkHeight - 1 || blocks[x, y + 1, z] == null)
                        {
                            vertices.Add(blockPos + new Vector3(0, 1, 0));
                            vertices.Add(blockPos + new Vector3(0, 1, 1));
                            vertices.Add(blockPos + new Vector3(1, 1, 1));
                            vertices.Add(blockPos + new Vector3(1, 1, 0));
                            uvs.AddRange(blocks[x, y, z].top.textureData.uvs);
                            faceNum += 1;
                        }
                        else
                            blocks[x, y, z].top.linkBlock = blocks[x, y + 1, z];
                        //bottom
                        if (y == 0 || blocks[x, y - 1, z] == null)
                        {
                            vertices.Add(blockPos + new Vector3(0, 0, 0));
                            vertices.Add(blockPos + new Vector3(1, 0, 0));
                            vertices.Add(blockPos + new Vector3(1, 0, 1));
                            vertices.Add(blockPos + new Vector3(0, 0, 1));
                            uvs.AddRange(blocks[x, y, z].bottom.textureData.uvs);
                            faceNum += 1;
                        }
                        else
                            blocks[x, y, z].bottom.linkBlock = blocks[x, y - 1, z];
                        //north
                        if (blocks[x, y, z + 1] == null)
                        {
                            vertices.Add(blockPos + new Vector3(1, 0, 1));
                            vertices.Add(blockPos + new Vector3(1, 1, 1));
                            vertices.Add(blockPos + new Vector3(0, 1, 1));
                            vertices.Add(blockPos + new Vector3(0, 0, 1));
                            uvs.AddRange(blocks[x, y, z].north.textureData.uvs);
                            faceNum += 1;
                        }
                        else
                            blocks[x, y, z].north.linkBlock = blocks[x, y, z + 1];
                        //south
                        if (blocks[x, y, z - 1] == null)
                        {
                            vertices.Add(blockPos + new Vector3(0, 0, 0));
                            vertices.Add(blockPos + new Vector3(0, 1, 0));
                            vertices.Add(blockPos + new Vector3(1, 1, 0));
                            vertices.Add(blockPos + new Vector3(1, 0, 0));
                            uvs.AddRange(blocks[x, y, z].south.textureData.uvs);
                            faceNum += 1;
                        }
                        else
                            blocks[x, y, z].south.linkBlock = blocks[x, y, z - 1];
                        //west
                        if (blocks[x - 1, y, z] == null)
                        {
                            vertices.Add(blockPos + new Vector3(0, 0, 1));
                            vertices.Add(blockPos + new Vector3(0, 1, 1));
                            vertices.Add(blockPos + new Vector3(0, 1, 0));
                            vertices.Add(blockPos + new Vector3(0, 0, 0));
                            uvs.AddRange(blocks[x, y, z].west.textureData.uvs);
                            faceNum += 1;
                        }
                        else
                            blocks[x, y, z].west.linkBlock = blocks[x - 1, y, z];
                        //east
                        if (blocks[x + 1, y, z] == null)
                        {
                            vertices.Add(blockPos + new Vector3(1, 0, 0));
                            vertices.Add(blockPos + new Vector3(1, 1, 0));
                            vertices.Add(blockPos + new Vector3(1, 1, 1));
                            vertices.Add(blockPos + new Vector3(1, 0, 1));
                            uvs.AddRange(blocks[x, y, z].east.textureData.uvs);
                            faceNum += 1;
                        }
                        else
                            blocks[x, y, z].east.linkBlock = blocks[x + 1, y, z];

                        int tl = vertices.Count - faceNum * 4;
                        for(int i = 0; i < faceNum; ++i)
                        {
                            triangles.AddRange(new int[] { tl + i * 4, tl + i * 4 + 1, tl + i * 4 + 2, tl + i * 4, tl + i * 4 + 2, tl + i * 4 + 3});
                        }
                    }
                }
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        //重新计算法线
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        GetComponent<MeshRenderer>().material.mainTexture = WorldTexture.textureAtlas;
    }
}
