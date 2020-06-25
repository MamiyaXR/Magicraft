using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbTest : MonoBehaviour
{
    void Start()
    {
        AssetBundle texturesAB = AssetBundle.LoadFromFile("AssetBundles/textures.unity3d");
        string[] strArr = texturesAB.GetAllAssetNames();
        Material material = GetComponent<MeshRenderer>().material;
        MeshFilter mesh = GetComponent<MeshFilter>();

        List<Texture2D> textureList = new List<Texture2D>();
        foreach(string str in strArr)
        {
            textureList.Add(texturesAB.LoadAsset<Texture2D>(str));
        }

        Texture2D textureAtlas = new Texture2D(64, 7 * 64);
        
        Rect[] packResult = textureAtlas.PackTextures(textureList.ToArray(), 0);
        for(int i = 0; i < packResult.Length; ++i)
        {
            Debug.Log("i=" + i + " xMin=" + packResult[i].xMin + " xMax=" + packResult[i].xMax + " yMin=" + packResult[i].yMin + " yMax=" + packResult[i].yMax);
        }
        Debug.Log(mesh.mesh.triangles.Length);
        Debug.Log(mesh.mesh.uv.Length);

        Vector2[] newUV = new Vector2[]
        {
            new Vector2(packResult[2].xMin, packResult[2].yMin),
            new Vector2(packResult[2].xMin, packResult[2].yMax),
            new Vector2(packResult[2].xMax, packResult[2].yMax),
            new Vector2(packResult[2].xMax, packResult[2].yMin)
        };
        for(int i = 0, j = 0; i < mesh.mesh.uv.Length; ++i)
        {
            if (j > 3)
                j = 0;

            mesh.sharedMesh.uv[i].x = 0;//newUV[j].x;
            mesh.sharedMesh.uv[i].y = 0;// newUV[j].y;
            ++j;
        }

        for (int i = 0; i < mesh.mesh.uv.Length; ++i)
            Debug.Log(mesh.sharedMesh.uv[i]);
        material.mainTexture = textureAtlas;
    }
}
