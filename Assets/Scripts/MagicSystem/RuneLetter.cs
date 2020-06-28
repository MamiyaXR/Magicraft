using GestureRecognizer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RuneLetter
{
    /// <summary>
    /// 属性
    /// </summary>
    protected string id;
    protected GesturePattern pattern;
    /// <summary>
    /// 属性接口
    /// </summary>
    public string Id { get => id; }
    public GesturePattern Pattern { get => pattern; }
    /// <summary>
    /// 卢恩文字库
    /// </summary>
    public struct RuneLetterLibary
    {
        public const string force = "force";
        public const string fire = "fire";
    }
    private const string assetBundlePath = "AssetBundles/runeletters.unity3d";
    private const string assetPathBase = "Assets/RuneLetters/";
    private static AssetBundle gesturePatternAB;
    protected static GesturePattern LoadGesturePatterById(string id)
    {
        if(gesturePatternAB == null)
            gesturePatternAB = AssetBundle.LoadFromFile(assetBundlePath);
        return gesturePatternAB.LoadAsset<GesturePattern>(assetPathBase + id + ".asset");
    }
    public static Dictionary<string, RuneLetter> runeLetterDict = new Dictionary<string, RuneLetter>()
    {
        { RuneLetterLibary.force, RuneLetterForce.Instance},
        { RuneLetterLibary.fire, RuneLetterFire.Instance}
    };
    /// <summary>
    /// 卢恩文字效果函数
    /// </summary>
    public abstract void MagicLaunch(GameObject caster, MagicEventArgs e);
}
