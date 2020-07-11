using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public MagicController magicController;
    [SerializeField] private Slider slider = null;
    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }
    /*****************************************************************************************
     *
     *
     *
     *****************************************************************************************/
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        SetCursorVisible(false);
        WorldTexture.InitTextureAtlas();
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
    //鼠标的显示与隐藏
    private void SetCursorVisible(bool visible)
    {
        Cursor.visible = visible;
        if(visible)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }
}
