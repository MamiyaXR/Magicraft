using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGroup : MonoBehaviour
{
    public List<Block> blocks = new List<Block>();
    private int faceNum = 0;
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
        faceNum = 0;
        BuildMeshByBlock(blocks[0], vertices, triangles, uvs, new Vector3(0, 0, 0), null);
        for (int i = 0; i < faceNum; ++i)
            triangles.AddRange(new int[] { i * 4, i * 4 + 1, i * 4 + 2, i * 4, i * 4 + 2, i * 4 + 3});

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        GetComponent<MeshRenderer>().material.mainTexture = WorldTexture.textureAtlas;
    }
    private void BuildMeshByBlock(Block block, List<Vector3> vertices, List<int> triangles, List<Vector2> uvs, Vector3 offset, Block from)
    {
        if (block.type == BlockType.Air)
            return;

        //top
        if(block.top.linkBlock == null)
        {
            vertices.Add(offset + new Vector3(-0.5f, 0.5f, -0.5f));
            vertices.Add(offset + new Vector3(-0.5f, 0.5f, 0.5f));
            vertices.Add(offset + new Vector3(0.5f, 0.5f, 0.5f));
            vertices.Add(offset + new Vector3(0.5f, 0.5f, -0.5f));
            uvs.AddRange(block.top.textureData.uvs);
            faceNum++;
        } else if(block.top.linkBlock != from)
            BuildMeshByBlock(block.top.linkBlock, vertices, triangles, uvs, offset + new Vector3(0, 1, 0), block);
        //bottom
        if(block.bottom.linkBlock == null)
        {
            vertices.Add(offset + new Vector3(-0.5f, -0.5f, -0.5f));
            vertices.Add(offset + new Vector3(0.5f, -0.5f, -0.5f));
            vertices.Add(offset + new Vector3(0.5f, -0.5f, 0.5f));
            vertices.Add(offset + new Vector3(-0.5f, -0.5f, 0.5f));
            uvs.AddRange(block.bottom.textureData.uvs);
            faceNum++;
        } else if(block.bottom.linkBlock != from)
            BuildMeshByBlock(block.bottom.linkBlock, vertices, triangles, uvs, offset + new Vector3(0, -1, 0), block);
        //north
        if(block.north.linkBlock == null)
        {
            vertices.Add(offset + new Vector3(0.5f, -0.5f, 0.5f));
            vertices.Add(offset + new Vector3(0.5f, 0.5f, 0.5f));
            vertices.Add(offset + new Vector3(-0.5f, 0.5f, 0.5f));
            vertices.Add(offset + new Vector3(-0.5f, -0.5f, 0.5f));
            uvs.AddRange(block.north.textureData.uvs);
            faceNum++;
        } else if(block.north.linkBlock != from)
            BuildMeshByBlock(block.north.linkBlock, vertices, triangles, uvs, offset + new Vector3(0, 0, 1), block);
        //south
        if(block.south.linkBlock == null)
        {
            vertices.Add(offset + new Vector3(-0.5f, -0.5f, -0.5f));
            vertices.Add(offset + new Vector3(-0.5f, 0.5f, -0.5f));
            vertices.Add(offset + new Vector3(0.5f, 0.5f, -0.5f));
            vertices.Add(offset + new Vector3(0.5f, -0.5f, -0.5f));
            uvs.AddRange(block.south.textureData.uvs);
            faceNum++;
        } else if(block.south.linkBlock != from)
            BuildMeshByBlock(block.south.linkBlock, vertices, triangles, uvs, offset + new Vector3(0, 0, -1), block);
        //west
        if(block.west.linkBlock == null)
        {
            vertices.Add(offset + new Vector3(-0.5f, -0.5f, 0.5f));
            vertices.Add(offset + new Vector3(-0.5f, 0.5f, 0.5f));
            vertices.Add(offset + new Vector3(-0.5f, 0.5f, -0.5f));
            vertices.Add(offset + new Vector3(-0.5f, -0.5f, -0.5f));
            uvs.AddRange(block.west.textureData.uvs);
            faceNum++;
        } else if(block.west.linkBlock != from)
            BuildMeshByBlock(block.west.linkBlock, vertices, triangles, uvs, offset + new Vector3(-1, 0, 0), block);
        //east
        if(block.east.linkBlock == null)
        {
            vertices.Add(offset + new Vector3(0.5f, -0.5f, -0.5f));
            vertices.Add(offset + new Vector3(0.5f, 0.5f, -0.5f));
            vertices.Add(offset + new Vector3(0.5f, 0.5f, 0.5f));
            vertices.Add(offset + new Vector3(0.5f, -0.5f, 0.5f));
            uvs.AddRange(block.east.textureData.uvs);
            faceNum++;
        } else if(block.east.linkBlock != from)
            BuildMeshByBlock(block.east.linkBlock, vertices, triangles, uvs, offset + new Vector3(1, 0, 0), block);
    }
}
