using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagManager : InventoryManager
{
    private static BagManager _instance;
    public static BagManager instance { get => _instance; }
    private BagManager() { }
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
    protected override void Start()
    {
        base.Start();
    }
}
