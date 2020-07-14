using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLeaves : Block
{
    public BlockLeaves()
    {
        _type = BlockType.BlockLeaves;
        _durability = 1;
        _top = new Surface(TextureType.Leaves);
        _bottom = new Surface(TextureType.Leaves);
        _north = new Surface(TextureType.Leaves);
        _south = new Surface(TextureType.Leaves);
        _west = new Surface(TextureType.Leaves);
        _east = new Surface(TextureType.Leaves);
    }
}
