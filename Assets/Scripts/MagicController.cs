using GestureRecognizer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MagicController : MonoBehaviour
{
    public static bool canUseMagic = true;
    public static bool isUsingMagic = false;
    [SerializeField] private Transform originPos;
    [SerializeField] private Transform startPos;
    [SerializeField] private GameObject magicGroup;
    [Space]
    public float magicPowerMax = 0;
    [SerializeField] private float magicRevertSpeed = 0;
    [SerializeField] private float magicExpenseSpeed = 0;
    private float magicExpense = 0;
    private bool isCalcMaigc;
    private float _magicPower;
    public float magicPower { get => _magicPower; }
    private MagicManager _magicManager;
    public MagicManager magicManager { get => _magicManager; }
    /*****************************************************************************************
     *
     *
     *
     *****************************************************************************************/
    private void Awake()
    {
        _magicPower = magicPowerMax;
        _magicManager = new MagicManager();
        isCalcMaigc = false;
    }
    private void Update()
    {
        //回复魔法值，常
        MagicPowerRevert();
        //魔法模式控制
        MagicModeControl();
        //测试用，待删除
        if (Input.GetKeyDown(KeyCode.M))
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            obj.transform.position = startPos.position;
            Rigidbody rbody =  obj.AddComponent<Rigidbody>();
            rbody.mass = 0.1f;
        }
    }
    /// <summary>
    /// 魔法模式控制函数
    /// </summary>
    private void MagicModeControl()
    {
        if (!canUseMagic)
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!isUsingMagic)
            {
                isUsingMagic = true;
                CameraController.canChangeCam = false;
                CameraController.canRotation = false;
                magicGroup.SetActive(true);
            }
            else
            {
                StartCoroutine("MagicPowerCalc");
                isCalcMaigc = true;
            }
        }
        else if (isCalcMaigc && Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopCoroutine("MagicPowerCalc");
            isCalcMaigc = false;
            Vector3 pos = Camera.main.WorldToScreenPoint(startPos.position);
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, pos.z));
            Vector3 direction = mousePos - originPos.position;
            direction.Normalize();
            float distance = 3f;
            magicManager.OnMagic(gameObject, new MagicEventArgs(magicExpense, originPos.position, direction, distance));
            magicManager.ClearMagic();
            isUsingMagic = false;
            CameraController.canChangeCam = true;
            CameraController.canRotation = true;
            magicGroup.SetActive(false);
            magicExpense = 0;
        }
    }
    /// <summary>
    /// 魔力值回复函数
    /// </summary>
    private void MagicPowerRevert()
    {
        if (!isCalcMaigc)
        {
            if (_magicPower < magicPowerMax)
                _magicPower += magicPowerMax * magicRevertSpeed * Time.deltaTime;
            else if (_magicPower > magicPowerMax)
                _magicPower = magicPowerMax;
        }
    }
    /// <summary>
    /// 蓄力计算携程函数
    /// </summary>
    /// <returns></returns>
    IEnumerator MagicPowerCalc()
    {
        float temp = 0;
        while (_magicPower > 0)
        {
            temp = magicPowerMax * magicExpenseSpeed * Time.deltaTime;
            magicExpense += temp;
            _magicPower -= temp;
            yield return 0;
        }
    }
}
