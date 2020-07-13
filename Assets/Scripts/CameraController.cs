using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool active = true;
    public bool blockCameraRotation = false;
    public CameraMode cameraMode;
    public GameObject firstPersonCam;
    public GameObject thirdPersonCam;
    private static CameraController _instance;
    public static CameraController instance { get => _instance; }
    /*****************************************************************************************
     *
     *
     *
     *****************************************************************************************/
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
    private void Start()
    {
        ChangeCameraMode(CameraMode.FirstPersonCam);
    }
    void Update()
    {
        if (!active)
            return;

        if (Input.GetKeyDown(KeyCode.F5))
        {
            switch (cameraMode)
            {
                case CameraMode.FirstPersonCam:
                    ChangeCameraMode(CameraMode.ThirdPersonCam);
                    break;
                case CameraMode.ThirdPersonCam:
                    ChangeCameraMode(CameraMode.FirstPersonCam);
                    break;
                default:
                    break;
            }
        }
    }
    /// <summary>
    /// 视角切换函数
    /// </summary>
    private void ChangeCameraMode(CameraMode mode)
    {
        if (cameraMode == mode)
            return;

        switch (mode)
        {
            case CameraMode.FirstPersonCam:
                thirdPersonCam.SetActive(false);
                firstPersonCam.SetActive(true);
                thirdPersonCam.tag = "Untagged";
                firstPersonCam.tag = "MainCamera";
                cameraMode = mode;
                GameManager.instance.ChangeGameState(GameState.Normal);
                break;
            case CameraMode.ThirdPersonCam:
                firstPersonCam.SetActive(false);
                thirdPersonCam.SetActive(true);
                firstPersonCam.tag = "Untagged";
                thirdPersonCam.tag = "MainCamera";
                cameraMode = mode;
                GameManager.instance.ChangeGameState(GameState.ThirdPerson);
                break;
            default:
                break;
        }
    }
}
public enum CameraMode
{
    FirstPersonCam,
    ThirdPersonCam
}
