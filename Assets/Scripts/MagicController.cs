using GestureRecognizer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MagicController : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////
    [SerializeField] private GameObject blockGroupPrefab = null;
    ////////////////////////////////////////////////////////////////
    [Space]
    public bool active = true;
    public bool isUsingMagic = false;
    [SerializeField] private Transform originPos = null;
    [SerializeField] private Transform startPos = null;
    [SerializeField] private GameObject magicGroup = null;
    [Space]
    public float magicPowerMax = 100;
    [SerializeField] private float magicRevertSpeed = 0.5f;
    [SerializeField] private float magicExpenseSpeed = 0.5f;
    private float magicExpense = 0;
    private bool isCalcMaigc = false;
    private float _magicPower;
    public float magicPower { get => _magicPower; }
    private MagicManager _magicManager;
    public MagicManager magicManager { get => _magicManager; }
    private static MagicController _instance;
    public static MagicController instance { get => _instance; }
    /*****************************************************************************************
     *
     *
     *
     *****************************************************************************************/
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        _magicPower = magicPowerMax;
        _magicManager = new MagicManager();
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
            //GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //obj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            //obj.transform.position = startPos.position;
            //Rigidbody rbody = obj.AddComponent<Rigidbody>();
            //rbody.mass = 0.1f;

            BlockGroup bg = Instantiate(blockGroupPrefab, startPos.position, Quaternion.identity).GetComponent<BlockGroup>();
            List<Block> blocks = new List<Block>();
            Block block0 = new BlockGrass();
            //Block block1 = new BlockStone();
            //Block block2 = new BlockDirt();
            //block0.east.linkBlock = block1;
            //block1.west.linkBlock = block0;
            //block1.top.linkBlock = block2;
            //block2.bottom.linkBlock = block1;
            blocks.Add(block0);
            //blocks.Add(block1);
            //blocks.Add(block2);
            bg.blocks = blocks;
            bg.BuildMesh();
        }
    }
    /// <summary>
    /// 魔法模式控制函数
    /// </summary>
    private void MagicModeControl()
    {
        if (!active)
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!isUsingMagic)
            {
                isUsingMagic = true;
                GameManager.instance.ChangeGameState(GameState.UsingMagic);
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
            magicGroup.SetActive(false);
            magicExpense = 0;
            GameManager.instance.ChangeGameState(GameState.Normal);
            isUsingMagic = false;
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
