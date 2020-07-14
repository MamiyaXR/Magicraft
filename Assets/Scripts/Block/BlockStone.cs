using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockStone : Block
{
    public BlockStone()
    {
        _type = BlockType.BlockStone;
        _durability = 100;
        _top = new Surface(TextureType.Stone);
        _bottom = new Surface(TextureType.Stone);
        _north = new Surface(TextureType.Stone);
        _south = new Surface(TextureType.Stone);
        _west = new Surface(TextureType.Stone);
        _east = new Surface(TextureType.Stone);
    }
}
