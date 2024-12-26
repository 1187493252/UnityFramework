using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TouchRaycast : MonoBehaviour
{
    [SerializeField]
    bool _canCtrl;//开启/关闭控制

    [SerializeField]
    bool _canMove;
    [SerializeField]
    LayerMask layerMask = -1;
    public Camera mainCamera;

    Transform touchedObject;
    public Transform TouchedObject => touchedObject;

    public float movespeed = 0.1f;

    public UnityEvent OnTouchBegan;
    public UnityEvent OnTouchMove;
    public UnityEvent OnTouchEnd;

    public UnityAction<Transform> OnTouchedObjectSelect;
    public UnityAction OnTouchedObjectUnSelect;
    public UnityAction<Transform> OnTouchedObjectTouchMove;


    void Start()
    {
        if (!mainCamera)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        if (!_canCtrl)
        {
            return;
        }
        if (Input.touchCount == 1) // 检测是否有触摸事件
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) // 触摸开始
            {
                OnTouchBegan?.Invoke();

                Ray ray = mainCamera.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    // 这里可以添加选中物体的逻辑
                    touchedObject = hit.transform;
                    OnTouchedObjectSelect?.Invoke(touchedObject);

                }
                else
                {

                    touchedObject = null;
                    OnTouchedObjectUnSelect?.Invoke();

                }
            }
            else if (touch.phase == TouchPhase.Moved) // 触摸移动
            {
                OnTouchMove?.Invoke();

                // 拖拽逻辑
                if (TouchedObject && _canMove)
                {


                    float x = Input.touches[0].deltaPosition.x * Time.deltaTime * movespeed;
                    float y = Input.touches[0].deltaPosition.y * Time.deltaTime * movespeed;

                    Vector3 vector3 = TouchedObject.position;
                    vector3 += new Vector3(x, y, 0);
                    TouchedObject.position = vector3;

                    OnTouchedObjectTouchMove?.Invoke(TouchedObject);

                }
            }
            else if (touch.phase == TouchPhase.Ended) // 触摸结束
            {
                // 释放或完成拖拽逻辑
                OnTouchEnd?.Invoke();
            }
        }
    }

    public void SetMoveState()
    {
        _canMove = !_canMove;
    }

    public void SetCtrlState()
    {
        _canCtrl = !_canCtrl;
    }

    public void SetLayerMask(params string[] name)
    {
        layerMask = LayerMask.GetMask(name);
    }
}