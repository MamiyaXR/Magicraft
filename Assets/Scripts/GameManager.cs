using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Slider slider = null;
    [SerializeField] private GameObject cursor = null;
    [SerializeField] private GameObject bag = null;
    private bool canOpenUI = true;
    public Dictionary<InventoryID, InventoryManager> inventoryDict;
    public GameObject firePrefab;
    private GameState _gameState = GameState.unInit;
    public GameState gameState { get => _gameState; }
    private static GameManager _instance;
    public static GameManager instance { get => _instance; }
    /*****************************************************************************************
     *
     *
     *
     *****************************************************************************************/
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        WorldTexture.InitTextureAtlas();
    }
    private void Start()
    {
        bag.SetActive(false);
        ChangeGameState(GameState.Normal);
        slider.maxValue = MagicController.instance.magicPowerMax;
        inventoryDict = new Dictionary<InventoryID, InventoryManager>()
        {
            { InventoryID.Tool, ToolManager.instance},
            { InventoryID.Bag, BagManager.instance }
        };
    }
    private void Update()
    {
        // 待删除，应由UIManager操作
        slider.value = MagicController.instance.magicPower;

        if (canOpenUI)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                bag.SetActive(!bag.activeSelf);
                SetCursorVisible(bag.activeSelf);
                if (bag.activeSelf)
                    ChangeGameState(GameState.OpenUI);
                else
                    ChangeGameState(GameState.Normal);
            }
        }
    }
    /// <summary>
    /// 鼠标的显示与隐藏
    /// </summary>
    /// <param name="visible">为true时显示，为false时隐藏</param>
    public void SetCursorVisible(bool visible)
    {
        if (Cursor.visible == visible)
            return;

        Cursor.visible = visible;
        cursor.SetActive(!visible);
        if(visible)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }
    public void ChangeGameState(GameState state)
    {
        if (_gameState == state)
            return;

        switch (state)
        {
            case GameState.Normal:
                SetSignals(true, false, true, true, true, false, true);
                SetCursorVisible(false);
                break;
            case GameState.ThirdPerson:
                SetSignals(true, false, false, false, true, false, false);
                SetCursorVisible(false);
                cursor.SetActive(false);
                break;
            case GameState.UsingMagic:
                SetSignals(true, true, false, true, false, true, false);
                SetCursorVisible(true);
                break;
            case GameState.OpenUI:
                SetSignals(true, true, false, false, false, true, true);
                SetCursorVisible(true);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 统一设置标志位
    /// </summary>
    /// <param name="moveCtrlActive"> MoveController.active，为true能移动，为false不能移动 </param>
    /// <param name="moveCtrlBlock"> MoveController.blockPlayerRotation，为true不能旋转，为false能旋转 </param>
    /// <param name="playerCtrlActive"> PlayerController.active，为true能动作，为false不能动作 </param>
    /// <param name="magicCtrlActive"> MagicController.active，为true能动作，为false不能动作 </param>
    /// <param name="cameraCtrlActive"> CameraController.active，为true能切换相机，为false不能切换相机 </param>
    /// <param name="cameraCtrlBlock"> CameraController.blockCameraRotation，为true不能旋转，为false能旋转 </param>
    /// <param name="canOpenUI"> GameManager.canOpenUI，为true能打开UI，为false不能打开UI </param>
    private void SetSignals(bool moveCtrlActive, bool moveCtrlBlock, bool playerCtrlActive,
                            bool magicCtrlActive, bool cameraCtrlActive, bool cameraCtrlBlock, bool canOpenUI)
    {
        MoveController.instance.active = moveCtrlActive;
        MoveController.instance.blockPlayerRotation = moveCtrlBlock;
        PlayerController.instance.active = playerCtrlActive;
        MagicController.instance.active = magicCtrlActive;
        CameraController.instance.active = cameraCtrlActive;
        CameraController.instance.blockCameraRotation = cameraCtrlBlock;
        this.canOpenUI = canOpenUI;
    }
    /*****************************************************************************************
     *
     *
     *
     *****************************************************************************************/
    
}
public enum GameState
{
    unInit = 0,
    Normal,
    ThirdPerson,
    UsingMagic,
    OpenUI
}