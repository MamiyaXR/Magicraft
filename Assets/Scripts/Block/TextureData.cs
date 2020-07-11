using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureData
{
    private Vector2[] _uvs;
    public Vector2[] uvs { get => _uvs; }
    public TextureData(TextureType type)
    {
        if (WorldTexture.textureAtlas == null)
            return;

        _uvs = new Vector2[]
        {
            new Vector2(WorldTexture.packResult[(int)type].xMin, WorldTexture.packResult[(int)type].yMin),
            new Vector2(WorldTexture.packResult[(int)type].xMin, WorldTexture.packResult[(int)type].yMax),
            new Vector2(WorldTexture.packResult[(int)type].xMax, WorldTexture.packResult[(int)type].yMax),
            new Vector2(WorldTexture.packResult[(int)type].xMax, WorldTexture.packResult[(int)type].yMin)
        };
    }

    public static Dictionary<TextureType, TextureData> textureDict = new Dictionary<TextureType, TextureData>
    {
        { TextureType.Dirt, new TextureData(TextureType.Dirt) },
        { TextureType.GrassSide, new TextureData(TextureType.GrassSide) },
        { TextureType.Grass, new TextureData(TextureType.Grass) },
        { TextureType.Leaves, new TextureData(TextureType.Leaves) },
        { TextureType.TreeSide, new TextureData(TextureType.TreeSide) },
        { TextureType.TreeCX, new TextureData(TextureType.TreeCX) },
        { TextureType.Stone, new TextureData(TextureType.Stone) }
    };
}
