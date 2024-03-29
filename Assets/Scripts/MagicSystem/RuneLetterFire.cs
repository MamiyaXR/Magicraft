﻿using GestureRecognizer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneLetterFire : RuneLetter
{
    private const string path = "AssetBundles/fire.unity3D";
    private const string assetName = "Assets/Prefabs/Fire.prefab";
    private GameObject fireBase;
    private static RuneLetterFire _instance;
    /*****************************************************************************************
     *
     *
     *
     *****************************************************************************************/
    public static RuneLetterFire Instance
    {
        get
        {
            if (_instance == null)
                _instance = new RuneLetterFire();
            return _instance;
        }
    }
    private RuneLetterFire()
    {
        id = RuneLetterLibary.fire;
        pattern = LoadGesturePatterById(id);
        fireBase = AssetBundle.LoadFromFile(path).LoadAsset<GameObject>(assetName);
    }
    /// <summary>
    /// 接口实现
    /// </summary>
    public override void MagicLaunch(GameObject caster, MagicEventArgs e)
    {
        GameObject obj = GameObject.Instantiate(GameManager.instance.firePrefab, e.origin + e.direction * e.distance, Quaternion.identity);
        obj.transform.localScale *= e.magicPower / 100;
    }
}
