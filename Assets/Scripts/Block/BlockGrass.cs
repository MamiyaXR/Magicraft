using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGrass : Block
{
    public BlockGrass()
    {
        _type = BlockType.Grass;
        _durability = 10;
        _top = new Surface(TextureType.Grass);
        _bottom = new Surface(TextureType.Dirt);
        _north = new Surface(TextureType.GrassSide);
        _south = new Surface(TextureType.GrassSide);
        _west = new Surface(TextureType.GrassSide);
        _east = new Surface(TextureType.GrassSide);
    }
    public BlockGrass(Vector3 position) : this()
    {
        this.position = position;
    }
}
