using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 安卓控制器
/// </summary>

public class TouchCtrl : MonoBehaviour
{
    [SerializeField]
    bool _canCtrl;//开启/关闭控制

    [SerializeField]
    bool _bMoveOrRotation; //旋转还是移动布尔



    public float roatespeed = 0.1f;
    public float movespeed = 0.1f;
    public float scalespeed = 1f;
    public float minscale = 0.3f;

    private Touch _OldTouch1;  //上次触摸点1(手指1)  
    private Touch _OldTouch2;  //上次触摸点2(手指2)  

    // 记录手指触屏的位置  
    Vector2 _M_Screenpos = new Vector2();

    //相机初始位置
    Vector3 _OldPosition;
    Vector3 _OldRoate;
    void Start()
    {
        //记录开始摄像机的Position
        _OldPosition = transform.position;
        _OldRoate = transform.eulerAngles;
    }
    void Update()
    {
        if (!_canCtrl)
        {
            return;
        }
        //没有触摸  
        if (Input.touchCount <= 0)
        {
            return;
        }

        //单点触摸   
        if (1 == Input.touchCount)
        {
            //如果为 True 单指操作为旋转  如果为 Fals 单指操作为移动
            if (_bMoveOrRotation)
            {
                //水平上下旋转
                Touch _Touch = Input.GetTouch(0);
                Vector2 _DeltaPos = _Touch.deltaPosition;

                if (Mathf.Abs(_DeltaPos.x) > Mathf.Abs(_DeltaPos.y))
                {
                    transform.Rotate(Vector3.down * _DeltaPos.x * roatespeed, Space.World);
                }
                else if (Mathf.Abs(_DeltaPos.x) < Mathf.Abs(_DeltaPos.y))
                {
                    transform.Rotate(Vector3.right * _DeltaPos.y * roatespeed, Space.World);
                }
            }
            else
            {
                //移动

                if (Input.touches[0].phase == TouchPhase.Began)
                {
                    // 记录手指触屏的位置  
                    _M_Screenpos = Input.touches[0].position;

                }
                // 手指移动  
                else if (Input.touches[0].phase == TouchPhase.Moved)
                {

                    // 移动摄像机  

                    float x = Input.touches[0].deltaPosition.x * Time.deltaTime * movespeed;
                    float y = Input.touches[0].deltaPosition.y * Time.deltaTime * movespeed;
                    //   transform.Translate(new Vector3(Input.touches[0].deltaPosition.x * Time.deltaTime * movespeed, Input.touches[0].deltaPosition.y * Time.deltaTime * movespeed, 0));
                    Vector3 vector3 = transform.position;
                    vector3 += new Vector3(x, y, 0);
                    transform.position = vector3;
                }
            }
        }


        //多点触摸, 放大缩小  
        Touch _NewTouch1 = Input.GetTouch(0);
        Touch _NewTouch2 = Input.GetTouch(1);

        //第2点刚开始接触屏幕, 只记录，不做处理  
        if (_NewTouch2.phase == TouchPhase.Began)
        {
            _OldTouch2 = _NewTouch2;
            _OldTouch1 = _NewTouch1;
            return;
        }

        //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型  
        float _OldDistance = Vector2.Distance(_OldTouch1.position, _OldTouch2.position);
        float _NewDistance = Vector2.Distance(_NewTouch1.position, _NewTouch2.position);

        //两个距离之差，为正表示放大手势， 为负表示缩小手势  
        float _Offset = _NewDistance - _OldDistance;

        //放大因子， 一个像素按 0.01倍来算(100可调整)  
        float _ScaleFactor = _Offset / 100f;
        _ScaleFactor *= scalespeed;
        Vector3 _LocalScale = transform.localScale;
        Vector3 _Scale = new Vector3(_LocalScale.x + _ScaleFactor, _LocalScale.y + _ScaleFactor, _LocalScale.z + _ScaleFactor);

        if (_Scale.x > minscale)
        {
            transform.localScale = _Scale;
        }

        //记住最新的触摸点，下次使用  
        _OldTouch1 = _NewTouch1;
        _OldTouch2 = _NewTouch2;
    }


    //通过按钮让物体回到最初的位置
    public void BackPosition()
    {
        //位置回归原点
        transform.position = _OldPosition;
        //旋转归零
        transform.eulerAngles = _OldRoate;
    }

    //设置单指操作方式 旋转还是移动
    public void RotationOrMove()
    {
        _bMoveOrRotation = !_bMoveOrRotation;
    }

    public void SetCtrlState()
    {
        _canCtrl = !_canCtrl;
    }


}

