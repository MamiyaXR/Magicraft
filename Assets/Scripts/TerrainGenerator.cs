﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ChunkPos
{
    public int xPos;
    public int zPos;
    public ChunkPos(int xPos, int zPos)
    {
        this.xPos = xPos;
        this.zPos = zPos;
    }
}

public class TerrainGenerator : MonoBehaviour
{
    public Vector3 originalPos = Vector3.zero;
    //区块模板
    public GameObject terrainChunk;
    public static Dictionary<ChunkPos, TerrainChunk> chunkDict = new Dictionary<ChunkPos, TerrainChunk>();
    private List<TerrainChunk> chunks = new List<TerrainChunk>();
    private List<ChunkPos> chunkPoses = new List<ChunkPos>();
    //噪声
    FastNoise noise = new FastNoise();
    int chunkDist = 5;

    public void Start()
    {
        LoadChunks(true);
    }
    public void Update()
    {
        LoadChunks();
    }
    public void BuildChunk(int xPos, int zPos)
    {
        TerrainChunk chunk;
        if(chunks.Count > 0)
        {
            chunk = chunks[0];
            chunk.gameObject.SetActive(true);
            chunks.RemoveAt(0);
            chunk.transform.position = new Vector3(xPos, 0, zPos);
        } else
        {
            GameObject chunkGo = Instantiate(terrainChunk, new Vector3(xPos, 0, zPos), Quaternion.identity);
            chunk = chunkGo.GetComponent<TerrainChunk>();
        }

        for(int x = 0; x < TerrainChunk.chunkWidth + 2; ++x)
        {
            for(int z = 0; z < TerrainChunk.chunkLength + 2; ++z)
            {
                for(int y = 0; y < TerrainChunk.chunkHeight; ++y)
                {
                    chunk.blocks[x, y, z] = GetBlockType(xPos + x - 1, y, zPos + z - 1);
                }
            }
        }

        GenerateTrees(chunk.blocks, xPos, zPos);
        chunk.BuildMesh();

        chunkDict.Add(new ChunkPos(xPos, zPos), chunk);
    }
    private BlockType GetBlockType(int x, int y, int z)
    {
        //基础层
        float simplex1 = noise.GetSimplex(x * .8f, z * .8f) * 10;
        float simplex2 = noise.GetSimplex(x * 3f, z * 3f) * 10 * (noise.GetSimplex(x * .3f, z * .3f) + .5f);
        float heightMap = simplex1 + simplex2;
        float baseLandHeight = TerrainChunk.chunkHeight * .5f + heightMap;

        //洞窟
        float caveNoise = noise.GetPerlinFractal(x * 5f, y * 10f, z * 5f);
        float caveMask = noise.GetSimplex(x * .3f, z * .3f) + .3f;

        //岩石层
        float simplexStone1 = noise.GetSimplex(x * 1f, z * 1f) * 10;
        float simplexStone2 = (noise.GetSimplex(x * 5f, z * 5f) + .5f) * 20 * (noise.GetSimplex(x * .3f, z * .3f) + .5f);
        float stoneHeightMap = simplexStone1 + simplexStone2;
        float baseStoneHeight = TerrainChunk.chunkHeight * .25f + stoneHeightMap;

        BlockType blockType = BlockType.Air;
        if(y <= baseLandHeight)
        {
            blockType = BlockType.Dirt;
            if (y > baseLandHeight - 1)
                blockType = BlockType.Grass;
            if (y <= baseStoneHeight)
                blockType = BlockType.Stone;
        }

        if (caveNoise > Mathf.Max(caveMask, 0.2f))
            blockType = BlockType.Air;

        return blockType;
    }
    private void GenerateTrees(BlockType[,,] blocks, int x, int z)
    {
        System.Random rand = new System.Random(x * 10000 + z);
        float simplex = noise.GetSimplex(x * .8f, z * .8f);
        if(simplex > 0)
        {
            simplex *= 2f;
            int treeCount = Mathf.FloorToInt((float)rand.NextDouble() * 5 * simplex);

            for(int i = 0; i < treeCount; ++i)
            {
                int xPos = (int)(rand.NextDouble() * 14) + 1;
                int zPos = (int)(rand.NextDouble() * 14) + 1;
                int y = TerrainChunk.chunkHeight - 1;
                //找到地面
                while (y > 0 && blocks[xPos, y, zPos] == BlockType.Air)
                    --y;
                ++y;

                int treeHeight = 4 + (int)(rand.NextDouble() * 4);

                for (int j = 0; j < treeHeight; ++j)
                    if (y + j < TerrainChunk.chunkHeight)
                        blocks[xPos, y + j, zPos] = BlockType.Trunk;

                int leavesWidth = 1 + (int)(rand.NextDouble() * 6);
                int leavesHeight = (int)(rand.NextDouble() * 3);

                int iter = 0;
                for (int m = y + treeHeight - 1; m <= y + treeHeight - 1 + treeHeight; m++)
                {
                    for (int k = xPos - (int)(leavesWidth * .5) + iter / 2; k <= xPos + (int)(leavesWidth * .5) - iter / 2; k++)
                        for (int l = zPos - (int)(leavesWidth * .5) + iter / 2; l <= zPos + (int)(leavesWidth * .5) - iter / 2; l++)
                            if (k >= 0 && k < 16 && l >= 0 && l < 16 && m >= 0 && m < 64 && rand.NextDouble() < .8f)
                                blocks[k, m, l] = BlockType.Leaves;

                    iter++;
                }
            }
        }
    }
    ChunkPos curChunk = new ChunkPos(-1, -1);
    void LoadChunks(bool instant = false)
    {
        int curChunkPosX = Mathf.FloorToInt(originalPos.x / 16) * 16;
        int curChunkPosZ = Mathf.FloorToInt(originalPos.z / 16) * 16;

        if (curChunk.xPos != curChunkPosX || curChunk.zPos != curChunkPosZ)
        {
            curChunk.xPos = curChunkPosX;
            curChunk.zPos = curChunkPosZ;

            for (int i = curChunkPosX - 16 * chunkDist; i <= curChunkPosX + 16 * chunkDist; i += 16)
                for (int j = curChunkPosZ - 16 * chunkDist; j <= curChunkPosZ + 16 * chunkDist; j += 16)
                {
                    ChunkPos cp = new ChunkPos(i, j);

                    if (!chunkDict.ContainsKey(cp) && !chunkPoses.Contains(cp))
                    {
                        if (instant)
                            BuildChunk(i, j);
                        else
                            chunkPoses.Add(cp);
                    }
                }
            //remove chunks that are too far away
            List<ChunkPos> toDestroy = new List<ChunkPos>();
            //unload chunks
            foreach (KeyValuePair<ChunkPos, TerrainChunk> c in chunkDict)
            {
                ChunkPos cp = c.Key;
                if (Mathf.Abs(curChunkPosX - cp.xPos) > 16 * (chunkDist + 3) ||
                    Mathf.Abs(curChunkPosZ - cp.zPos) > 16 * (chunkDist + 3))
                {
                    toDestroy.Add(c.Key);
                }
            }

            //remove any up for generation
            foreach (ChunkPos cp in chunkPoses)
            {
                if (Mathf.Abs(curChunkPosX - cp.xPos) > 16 * (chunkDist + 1) ||
                    Mathf.Abs(curChunkPosZ - cp.zPos) > 16 * (chunkDist + 1))
                    chunkPoses.Remove(cp);
            }

            foreach (ChunkPos cp in toDestroy)
            {
                chunkDict[cp].gameObject.SetActive(false);
                chunks.Add(chunkDict[cp]);
                chunkDict.Remove(cp);
            }

            StartCoroutine(DelayBuildChunks());
        }
    }
    IEnumerator DelayBuildChunks()
    {
        while (chunkPoses.Count > 0)
        {
            BuildChunk(chunkPoses[0].xPos, chunkPoses[0].zPos);
            chunkPoses.RemoveAt(0);

            yield return new WaitForSeconds(.2f);
        }
    }
}
