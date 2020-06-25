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
    private TextureType top;
    private TextureType side;
    private TextureType bottom;

    private Texture2D textureAtlas;
    private MyTexture _topTexture;
    private MyTexture _sideTexture;
    private MyTexture _bottomTexture;

    public MyTexture topTexture { get => _topTexture; }
    public MyTexture sideTexture { get => _sideTexture; }
    public MyTexture bottomTexture { get => _bottomTexture; }

    public Block(TextureType type)
    {
        top = type;
        side = type;
        bottom = type;
        GetMyTexture();
    }

    public Block(TextureType top, TextureType side, TextureType bottom)
    {
        this.top = top;
        this.side = side;
        this.bottom = bottom;
        GetMyTexture();
    }

    private void GetMyTexture()
    {
        _topTexture = MyTexture.textureDict[top];
        _sideTexture = MyTexture.textureDict[side];
        _bottomTexture = MyTexture.textureDict[bottom];
    }

    public static Dictionary<BlockType, Block> blockDict = new Dictionary<BlockType, Block>()
    {
        { BlockType.Dirt, new Block(TextureType.Dirt) },
        { BlockType.Grass, new Block(TextureType.Grass, TextureType.GrassSide, TextureType.Dirt) },
        { BlockType.Stone, new Block(TextureType.Stone) },
        { BlockType.Trunk, new Block(TextureType.TreeCX, TextureType.TreeSide, TextureType.TreeCX) },
        { BlockType.Leaves, new Block(TextureType.Leaves) }
    };
}
