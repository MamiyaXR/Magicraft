using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 单例模式构建
    /// </summary>
    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }

    public MagicController magicController;
    private void Awake()
    {
        _instance = this;
        SetCursorVisible(false);
    }

    [SerializeField]
    private Slider slider = null;
    /// <summary>
    /// 鼠标的显示与隐藏
    /// </summary>
    /// <param name="visible"></param>
    private void SetCursorVisible(bool visible)
    {
        Cursor.visible = visible;
        if(visible)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }
    private void Start()
    {
        slider.maxValue = magicController.magicPowerMax;
    }
    private void Update()
    {
        if (MagicController.isUsingMagic)
            SetCursorVisible(true);
        else
            SetCursorVisible(false);
        slider.value = magicController.magicPower;
    }
}
