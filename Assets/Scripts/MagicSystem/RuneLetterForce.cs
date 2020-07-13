using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneLetterForce : RuneLetter
{
    private static RuneLetterForce _instance;
    /*****************************************************************************************
     *
     *
     *
     *****************************************************************************************/
    public static RuneLetterForce Instance
    {
        get
        {
            if (_instance == null)
                _instance = new RuneLetterForce();
            return _instance;
        }
    }
    private RuneLetterForce()
    {
        id = RuneLetterLibary.force;
        pattern = LoadGesturePatterById(id);
    }
    /// <summary>
    /// 接口实现
    /// </summary>
    public override void MagicLaunch(GameObject caster, MagicEventArgs e)
    {
        RaycastHit hit;
        if (Physics.Raycast(e.origin, e.direction, out hit, e.distance * 5f))
        {
            Collider collider = hit.collider;
            if(collider != null)
            {
                Rigidbody rbody = collider.GetComponent<Rigidbody>();
                if (rbody != null)
                    rbody.AddForce(e.direction * e.magicPower);
            }
        }
    }
}
