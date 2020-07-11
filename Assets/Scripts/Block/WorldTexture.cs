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
public class WorldTexture
{
    //纹理资源路径
    private static string path = "AssetBundles/textures.unity3d";
    //合成区块纹理
    private static Texture2D _textureAtlas = null;
    public static Texture2D textureAtlas { get => _textureAtlas; }
    private static Rect[] _packResult = null;
    public static Rect[] packResult { get => _packResult; }
    public static void InitTextureAtlas()
    {
        if (_textureAtlas == null)
        {
            _textureAtlas = new Texture2D(1024, 1024);
            AssetBundle texturesAB = AssetBundle.LoadFromFile(path);
            string[] strArray = texturesAB.GetAllAssetNames();
            List<Texture2D> textureList = new List<Texture2D>();
            foreach (string str in strArray)
                textureList.Add(texturesAB.LoadAsset<Texture2D>(str));
            _packResult = _textureAtlas.PackTextures(textureList.ToArray(), 0);
        }
    }
}
