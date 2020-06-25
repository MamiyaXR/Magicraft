using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TextureType
{
    Dirt,
    GrassSide,
    Grass,
    Leaves,
    TreeSide,
    TreeCX,
    Stone
}
public class MyTexture
{
    //纹理资源路径
    private static string path = "AssetBundles/textures.unity3d";
    //合成区块纹理
    private static Texture2D _textureAtlas = null;
    public static Texture2D textureAtlas { get => _textureAtlas; }
    private static Rect[] packResult = null;
    private static void InitTextureAtlas()
    {
        _textureAtlas = new Texture2D(1024, 1024);
        AssetBundle texturesAB = AssetBundle.LoadFromFile(path);
        string[] strArray = texturesAB.GetAllAssetNames();
        List<Texture2D> textureList = new List<Texture2D>();
        foreach (string str in strArray)
            textureList.Add(texturesAB.LoadAsset<Texture2D>(str));
        packResult = _textureAtlas.PackTextures(textureList.ToArray(), 0);
    }

    private Vector2[] _uvs;
    public Vector2[] uvs { get => _uvs; }
    public MyTexture(TextureType type)
    {
        if(textureAtlas == null || packResult == null)
            InitTextureAtlas();
        _uvs = new Vector2[]
        {
            new Vector2(packResult[(int)type].xMin, packResult[(int)type].yMin),
            new Vector2(packResult[(int)type].xMin, packResult[(int)type].yMax),
            new Vector2(packResult[(int)type].xMax, packResult[(int)type].yMax),
            new Vector2(packResult[(int)type].xMax, packResult[(int)type].yMin)
        };
    }
    public static Dictionary<TextureType, MyTexture> textureDict = new Dictionary<TextureType, MyTexture>
    {
        { TextureType.Dirt, new MyTexture(TextureType.Dirt) },
        { TextureType.GrassSide, new MyTexture(TextureType.GrassSide) },
        { TextureType.Grass, new MyTexture(TextureType.Grass) },
        { TextureType.Leaves, new MyTexture(TextureType.Leaves) },
        { TextureType.TreeSide, new MyTexture(TextureType.TreeSide) },
        { TextureType.TreeCX, new MyTexture(TextureType.TreeCX) },
        { TextureType.Stone, new MyTexture(TextureType.Stone) }
    };
}
