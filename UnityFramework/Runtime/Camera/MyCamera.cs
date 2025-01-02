/*
* FileName:          MyCamera
* CompanyName:       杭州中锐
* Author:            relly
* Description:       
* 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    public Transform Target;
    [Header("摄像机距离目标的距离 x:最小 y：最大")]
    public Vector2 TargetDis = new Vector2(0.1f, 5f);
    [Range(0f, 1f)]
    [SerializeField]
    float 距离滑动条值 = 0.5f;

    [Header("左右旋转速度")]
    public float HorizontalRotateSpeed = 2f;
    [Header("上下旋转速度")]
    public float VerticalRotateSpeed = 2f;
    [Header("上下旋转角度限制 x:最小 y：最大")]
    public Vector2 VerticalRotateAngleLimit = new Vector2(-20f, 80f);

    [Header("移动延迟")]
    public float MoveDelay = 4f;

    Vector3 StartAngles;
    float Start距离滑动条值;
    float Now距离滑动条值;
    float mouseX = 0.0f;
    float mouseY = 0.0f;
    Vector2 oldPosition1;
    Vector2 oldPosition2;
    #region  MonoBehaviour
    public void Start()
    {
        if (!Target)
        {
            Target = new GameObject("目标模型").transform;
            Target.position = Vector3.zero;
        }

        PosInit();
        Now距离滑动条值 = 距离滑动条值;
        ResetPos();
    }
    public void PosInit()
    {
        StartAngles = transform.eulerAngles;
        Start距离滑动条值 = 距离滑动条值;
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.Y))
        {
            距离滑动条值 -= Time.deltaTime * 0.5f;
            距离滑动条值 = Mathf.Clamp(距离滑动条值, 0, 1);


        }
        if (Input.GetKey(KeyCode.H))
        {
            距离滑动条值 += Time.deltaTime * 0.5f;
            距离滑动条值 = Mathf.Clamp(距离滑动条值, 0, 1);
        }


        if (Input.GetKey(KeyCode.W))
        {
            mouseY += 0.8f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            mouseY -= 0.8f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            mouseX += 0.8f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            mouseX -= 0.8f;
        }


#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        if (Input.GetMouseButton(1))
        {
            mouseX += Input.GetAxis("Mouse X") * HorizontalRotateSpeed;
            mouseY -= Input.GetAxis("Mouse Y") * VerticalRotateSpeed;


        }
        else if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            距离滑动条值 += Input.GetAxis("Mouse ScrollWheel") * 0.5f;
            距离滑动条值 = Mathf.Clamp(距离滑动条值, 0, 1);
        }


        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            mouseX += Input.GetTouch(0).deltaPosition.x;
            mouseY -= Input.GetTouch(0).deltaPosition.y;
        }
        else if (Input.touchCount > 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                var tempPosition1 = Input.GetTouch(0).position;
                var tempPosition2 = Input.GetTouch(1).position;


                if (isZoom(oldPosition1, oldPosition2, tempPosition1, tempPosition2))
                {
                    if (距离滑动条值 > 0)
                        距离滑动条值 -= 0.05f;
                }
                else
                {
                    if (距离滑动条值 < 1f)
                        距离滑动条值 += 0.05f;
                }


                oldPosition1 = tempPosition1;
                oldPosition2 = tempPosition2;
            }
        }
#else
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                mouseX += Input.GetAxis("Mouse X") * HorizontalRotateSpeed;
                mouseY -= Input.GetAxis("Mouse Y") * VerticalRotateSpeed;
            }
        }
        else if (Input.touchCount > 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                var tempPosition1 = Input.GetTouch(0).position;
                var tempPosition2 = Input.GetTouch(1).position;


                if (isZoom(oldPosition1, oldPosition2, tempPosition1, tempPosition2))
                {
                    if (距离滑动条值 > 0)
                        距离滑动条值 -= 0.05f;
                }
                else
                {
                    if (距离滑动条值 < 1f)
                        距离滑动条值 += 0.05f;
                }


                oldPosition1 = tempPosition1;
                oldPosition2 = tempPosition2;
            }
        }
#endif
    }


    void LateUpdate()
    {
        mouseY = ClampAngle(mouseY, VerticalRotateAngleLimit.x, VerticalRotateAngleLimit.y);
        Quaternion toRotation = Quaternion.Euler(mouseY, mouseX, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * MoveDelay);

        if (Target != null)
        {
            Vector3 toPosition = transform.rotation * new Vector3(0.0f, 0.0f, -Get当前距离()) + Target.position;
            transform.position = toPosition;
        }
    }


    void OnGUI()
    {
#if UNITY_EDITOR
        if (GUI.Button(new Rect(10, 10, 200, 50), "重置位置"))
        {
            ResetPos();
        }
#endif
    }
    #endregion



    public void ResetPos()
    {
        mouseX = StartAngles.y;
        mouseY = StartAngles.x;
        距离滑动条值 = Start距离滑动条值;
    }





    float Get当前距离()
    {
        Now距离滑动条值 = Mathf.Lerp(Now距离滑动条值, 距离滑动条值, Time.deltaTime * MoveDelay);
        return TargetDis.x + (TargetDis.y - TargetDis.x) * Now距离滑动条值;// 
    }


    #region Tools
    /// <summary>
    /// 放大/缩小
    /// </summary>
    /// <param name="oP1"></param>
    /// <param name="oP2"></param>
    /// <param name="nP1"></param>
    /// <param name="nP2"></param>
    /// <returns></returns>
    bool isZoom(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2)
    {
        float leng1 = Mathf.Sqrt((oP1.x - oP2.x) * (oP1.x - oP2.x) + (oP1.y - oP2.y) * (oP1.y - oP2.y));
        float leng2 = Mathf.Sqrt((nP1.x - nP2.x) * (nP1.x - nP2.x) + (nP1.y - nP2.y) * (nP1.y - nP2.y));
        if (leng1 > leng2)
            return false;
        else
            return true;

    }


    /// <summary>
    /// 限制旋转最大/最小值
    /// </summary>
    /// <param name="angle">当前</param>
    /// <param name="min">最小</param>
    /// <param name="max">最大</param>
    /// <returns></returns>
    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)//
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
    #endregion


}



