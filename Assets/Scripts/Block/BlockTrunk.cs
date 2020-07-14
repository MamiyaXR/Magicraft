using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTrunk : Block
{
    public BlockTrunk()
    {
        _type = BlockType.BlockTrunk;
        _durability = 50;
        _top = new Surface(TextureType.TreeCX);
        _bottom = new Surface(TextureType.TreeCX);
        _north = new Surface(TextureType.TreeSide);
        _south = new Surface(TextureType.TreeSide);
        _west = new Surface(TextureType.TreeSide);
        _east = new Surface(TextureType.TreeSide);
    }
}
