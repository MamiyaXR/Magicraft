using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDirt : Block
{
    public BlockDirt()
    {
        _type = BlockType.Dirt;
        _durability = 10;
        _top = new Surface(TextureType.Dirt);
        _bottom = new Surface(TextureType.Dirt);
        _north = new Surface(TextureType.Dirt);
        _south = new Surface(TextureType.Dirt);
        _west = new Surface(TextureType.Dirt);
        _east = new Surface(TextureType.Dirt);
    }
    public BlockDirt(Vector3 position) : this()
    {
        this.position = position;
    }
}
