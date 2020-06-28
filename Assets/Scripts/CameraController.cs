using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CameraMode
{
    FirstPersonCam,
    ThirdPersonCam
}
public class CameraController : MonoBehaviour
{
    public static bool canChangeCam = true;
    public static bool canRotation = true;
    public static CameraMode mode;
    public GameObject firstPersonCam;
    public GameObject thirdPersonCam;
    void Update()
    {
        if (canChangeCam && Input.GetKeyDown(KeyCode.F5))
        {
            ChangeLiveCamera();
        }
    }
    /// <summary>
    /// 视角切换
    /// </summary>
    private void ChangeLiveCamera()
    {
        if(firstPersonCam.activeSelf)
        {
            thirdPersonCam.SetActive(true);
            firstPersonCam.SetActive(false);
            firstPersonCam.tag = "Untagged";
            thirdPersonCam.tag = "MainCamera";
            mode = CameraMode.ThirdPersonCam;
            MagicController.canUseMagic = false;
        } else
        {
            firstPersonCam.SetActive(true);
            thirdPersonCam.SetActive(false);
            thirdPersonCam.tag = "Untagged";
            firstPersonCam.tag = "MainCamera";
            mode = CameraMode.FirstPersonCam;
            MagicController.canUseMagic = true;
        }
    }
}
