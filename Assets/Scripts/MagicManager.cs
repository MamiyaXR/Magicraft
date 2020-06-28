using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicManager
{
    public delegate void MagicEventHandler(GameObject caster, MagicEventArgs e);
    private event MagicEventHandler Magic;
    private List<MagicEventHandler> magicList;
    public void RegistMagic(MagicEventHandler handler)
    {
        Magic += handler;
        magicList.Add(handler);
    }
    public void UnRegistMagic(MagicEventHandler handler)
    {
        Magic -= handler;
        magicList.Remove(handler);
    }
    public void OnMagic(GameObject caster, MagicEventArgs e)
    {
        Magic?.Invoke(caster, e);
    }
    public void ClearMagic()
    {
        foreach (MagicEventHandler handler in magicList)
            Magic -= handler;
        magicList.Clear();
    }
    public MagicManager()
    {
        magicList = new List<MagicEventHandler>();
    }
}
public class MagicEventArgs : EventArgs
{
    private float _magicPower;
    private Vector3 _origin;
    private Vector3 _direction;
    private float _distance;
    public float magicPower { get => _magicPower; set => _magicPower = value; }
    public Vector3 origin { get => _origin; set => _origin = value; }
    public Vector3 direction { get => _direction; set => _direction = value; }
    public float distance { get => _distance; set => _distance = value; }
    public MagicEventArgs(Vector3 origin, Vector3 direction, float distance)
    {
        magicPower = 0;
        this.origin = origin;
        this.direction = direction;
        this.distance = distance;
    }
    public MagicEventArgs(float magicPower, Vector3 origin, Vector3 direction, float distance) : this(origin, direction, distance)
    {
        this.magicPower = magicPower;
    }
}
