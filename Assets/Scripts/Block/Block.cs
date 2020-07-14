using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全种类Block的枚举，名称与类名对应
/// </summary>
public enum BlockType
{
    Air = 0,
    BlockDirt,
    BlockGrass,
    BlockStone,
    BlockTrunk,
    BlockLeaves
}
/// <summary>
/// Block基类
/// </summary>
public abstract class Block
{
    public bool active;
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
        { BlockType.Air, null },
        { BlockType.BlockDirt, new BlockDirt() },
        { BlockType.BlockGrass, new BlockGrass() },
        { BlockType.BlockStone, new BlockStone() },
        { BlockType.BlockTrunk, new BlockTrunk() },
        { BlockType.BlockLeaves, new BlockLeaves() },
    };
}
