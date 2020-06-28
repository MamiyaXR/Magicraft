using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    //要相机跟随的GameObject
    public Transform m_target;
    //鼠标敏度  
    public float mousesSensity = 5F;
    //上下最大视角(Y视角)  
    public float minYLimit = -40F;
    public float maxYLimit = 80F;

    Vector3 m_camRotation;
    void Update()
    {
        if (CameraController.canRotation)
        {
            //根据鼠标的移动,获取相机旋转的角度
            m_camRotation.x = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mousesSensity;
            m_camRotation.y += Input.GetAxis("Mouse Y") * mousesSensity;
            //角度限制
            m_camRotation.y = Mathf.Clamp(m_camRotation.y, minYLimit, maxYLimit);
            //相机角度随着鼠标旋转  
            transform.localEulerAngles = new Vector3(-m_camRotation.y, m_camRotation.x, 0);
        }
        transform.position = m_target.position;
    }
}
