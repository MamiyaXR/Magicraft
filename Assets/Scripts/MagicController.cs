using GestureRecognizer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour
{
    public static bool canUseMagic = true;
    public static bool isUsingMagic = false;
    public Transform originPos;
    public Transform startPos;
    public GameObject magicGroup;
    public DrawDetector drawDetector;
    private MagicManager _magicManager;
    public MagicManager magicManager { get => _magicManager; }
    private void Awake()
    {
        _magicManager = new MagicManager();
    }
    private void Update()
    {
        if(canUseMagic && Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (isUsingMagic)
            {
                Vector3 pos = Camera.main.WorldToScreenPoint(startPos.position);
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, pos.z));
                Vector3 direction = mousePos - originPos.position;
                direction.Normalize();
                float distance = 3f;
                magicManager.OnMagic(gameObject, new MagicEventArgs(originPos.position, direction, distance));
                magicManager.ClearMagic();
            }
            magicGroup.SetActive(!magicGroup.activeSelf);
            isUsingMagic = !isUsingMagic;
            CameraController.canChangeCam = !CameraController.canChangeCam;
            CameraController.canRotation = !CameraController.canRotation;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            obj.transform.position = startPos.position;
            Rigidbody rbody =  obj.AddComponent<Rigidbody>();
            rbody.mass = 0.1f;
        }
    }
}
