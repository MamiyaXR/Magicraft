using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    Air,
    Dirt,
    Grass,
    Stone,
    Trunk,
    Leaves
}

public class Block
{
    //位置
    public Vector3 position;
    //类型
    protected BlockType _type;
    public BlockType type { get => _type; }
    //耐久度
    protected int _durability;
    public int durability { get => _durability; }
    //六个面
    protected Surface _top;
    protected Surface _bottom;
    protected Surface _north;
    protected Surface _south;
    protected Surface _west;
    protected Surface _east;
    public Surface top { get => _top; }
    public Surface bottom { get => _bottom; }
    public Surface north { get => _north; }
    public Surface south { get => _south; }
    public Surface west { get => _west; }
    public Surface east { get => _east; }

    public static Dictionary<BlockType, Block> blockDict = new Dictionary<BlockType, Block>()
    {
        { BlockType.Dirt, new BlockDirt() },
        { BlockType.Grass, new BlockGrass() },
        { BlockType.Stone, new BlockStone() },
        { BlockType.Trunk, new BlockTrunk() },
        { BlockType.Leaves, new BlockLeaves() }
    };
}
