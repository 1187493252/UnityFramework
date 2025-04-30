using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Touch = UnityEngine.Touch;
using TouchPhase = UnityEngine.TouchPhase;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance
    {
        get; private set;
    }

    [SerializeField]
    bool controlSwitch = true;


    Camera camera;

    [Header("目标设置")]
    public Transform target; // 摄像机焦点目标
    public LayerMask raycastMask; // 射线检测层

    [Header("距离控制")]
    public float minDistance = 1f;
    public float maxDistance = 10f;
    [SerializeField]
    float Distance = 10f;

    private float currentDistance;
    public float CurrentDistance => currentDistance;


    [Header("旋转参数")]
    public float rotateSpeed = 5f;
    [Range(-90, 0)] public float minVerticalAngle = -90f;
    [Range(0, 90)] public float maxVerticalAngle = 90f;
    private Vector2 currentRotation;

    [Header("移动参数")]
    public float panSpeed = 0.5f;

    [Header("缩放参数")]
    public float zoomSpeed = 2f;

    private Vector2 lastMousePosition; // 上一帧的鼠标位置


    Vector3 originalPos;
    Vector3 originalRotate;
    Vector3 targetoriginalPos;
    Vector3 targetoriginalRotate;


    GameObject touchedObject;
    public GameObject TouchedObject => touchedObject;
    public UnityAction<GameObject> OnTouchedObjectSelect;
    public UnityAction<RaycastHit> OnRaycastHit;

    //聚焦
    public float tempDistance;
    void Awake()
    {
        Instance = this;

        Init();

    }

    void Init()
    {
        camera = GetComponent<Camera>();
        if (target != null)
        {
            targetoriginalPos = target.position;
            targetoriginalRotate = target.eulerAngles;
        }
        originalPos = transform.position;
        originalRotate = transform.eulerAngles;


        // 初始化参数
        currentDistance = Distance;
        currentRotation = new Vector2(transform.eulerAngles.y, transform.eulerAngles.x);

#if UNITY_IOS || UNITY_ANDROID&&!UNITY_EDITOR
        zoomSpeed = 0.005f;
#endif

    }
    private void OnDestroy()
    {

    }
    /// <summary>
    /// 摄像机聚焦模型
    /// </summary>
    /// <param name="newTarget"></param>
    /// <param name="distance"></param>
    public void FocusOnModel(Transform newTarget, float distance)
    {
        if (newTarget == null)
        {
            Debug.LogWarning("New target is null. Cannot focus.");
            return;
        }

        target.position = newTarget.position; // 直接将 target 的位置设置为模型的位置

        currentDistance = distance;
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
        //   currentRotation = new Vector2(transform.eulerAngles.y, transform.eulerAngles.x);
        transform.position = target.position - transform.forward * currentDistance;
    }

    public void FocusOnModel(Vector3 pos, Vector3 eulerAngles, float distance)
    {

        target.position = pos; // 直接将 target 的位置设置为模型的位置

        currentDistance = distance;
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
        currentRotation = new Vector2(eulerAngles.y, eulerAngles.x);
        UpdateCameraPosition();

    }

    private void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
        transform.position = target.position + rotation * Vector3.back * currentDistance;

        transform.LookAt(target.position);
    }


    private void LateUpdate()
    {
        if (target == null) return;



#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBGL
        HandleMouseInput();
        HandleMouseRaycast();

#elif UNITY_IOS || UNITY_ANDROID

  
        HandleTouchInput();
        HandleTouchRaycast();
#endif







    }


    private void HandleMouseInput()
    {
        if (!controlSwitch)
        {
            return;
        }
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            lastMousePosition = Input.mousePosition;
            return;
        }
        if (Input.GetMouseButton(0))
        {
            Vector2 input = (Vector2)Input.mousePosition - lastMousePosition;
            currentRotation.x += input.x * rotateSpeed * Time.deltaTime;
            currentRotation.y = Mathf.Clamp(
                currentRotation.y - input.y * rotateSpeed * Time.deltaTime,
                minVerticalAngle,
                maxVerticalAngle
            );
            UpdateCameraPosition();
            //Quaternion rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
            //transform.position = target.position + rotation * Vector3.back * currentDistance;
            //transform.LookAt(target.position);
        }
        else if (Input.GetMouseButton(1))
        {

            Vector2 input = (Vector2)Input.mousePosition - lastMousePosition;
            Vector3 move = new Vector3(-input.x * panSpeed * Time.deltaTime, -input.y * panSpeed * Time.deltaTime, 0);

            target.position += transform.TransformDirection(move);

        }
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        if (zoomInput != 0)
        {
            currentDistance = Mathf.Clamp(currentDistance - zoomInput * zoomSpeed * Time.deltaTime, minDistance, maxDistance);


        }
        transform.position = target.position - transform.forward * currentDistance;
        lastMousePosition = Input.mousePosition;

    }

    private void HandleTouchInput()
    {
        if (!controlSwitch)
        {
            return;
        }
        if (Input.touchCount == 1)
        {

            Touch touch0 = Input.GetTouch(0);
            // 检测触摸是否在 UI 上
            if (EventSystem.current.IsPointerOverGameObject(touch0.fingerId))
            {
                return; // 如果触摸在 UI 上，则不处理相机逻辑
            }
            if (touch0.phase == TouchPhase.Began)
            {
                lastMousePosition = touch0.position;
                return;
            }
            if (touch0.phase == TouchPhase.Moved)
            {
                Vector2 input = touch0.deltaPosition;
                currentRotation.x += input.x * rotateSpeed * Time.deltaTime;
                currentRotation.y = Mathf.Clamp(
                    currentRotation.y - input.y * rotateSpeed * Time.deltaTime,
                    minVerticalAngle,
                    maxVerticalAngle
                );
                UpdateCameraPosition();

                //Quaternion rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
                //transform.position = target.position + rotation * Vector3.back * currentDistance;
                //transform.LookAt(target.position);
            }

        }
        else if (Input.touchCount == 2)
        {


            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);
            if (EventSystem.current.IsPointerOverGameObject(touch0.fingerId) || EventSystem.current.IsPointerOverGameObject(touch1.fingerId))
            {
                return; // 如果触摸在 UI 上，则不处理相机逻辑
            }
            Vector2 prevTouch0 = touch0.position - touch0.deltaPosition;
            Vector2 prevTouch1 = touch1.position - touch1.deltaPosition;

            float prevMagnitude = (prevTouch0 - prevTouch1).magnitude;
            float currentMagnitude = (touch0.position - touch1.position).magnitude;

            float difference = prevMagnitude - currentMagnitude;


            Vector2 input = touch0.deltaPosition;
            Vector3 move = new Vector3(-input.x * panSpeed * Time.deltaTime, -input.y * panSpeed * Time.deltaTime, 0);
            target.position += transform.TransformDirection(move);
            // 缩放
            Zoom(difference);


        }

        transform.position = target.position - transform.forward * currentDistance;
    }

    void Zoom(float zoomInput)
    {
        currentDistance = Mathf.Clamp(
            currentDistance + zoomInput * zoomSpeed,
            minDistance,
            maxDistance
        );

    }
    bool IsPointerOverUIObject()
    {
        if (EventSystem.current == null)
        {
            return false;
        }
#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBGL
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

#elif UNITY_IOS || UNITY_ANDROID

  
         Touch touch0 = Input.GetTouch(0);
        // 检测触摸是否在 UI 上
        if (EventSystem.current.IsPointerOverGameObject(touch0.fingerId))
        {
            return true; // 如果触摸在 UI 上，则不处理相机逻辑
        }

#endif
        return false;

    }

    private void HandleMouseRaycast()
    {

        if (Input.GetMouseButtonUp(0))
        {

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, raycastMask))
            {
                // 处理点击到的物体
                touchedObject = hit.transform.gameObject;
                OnTouchedObjectSelect?.Invoke(touchedObject);
                OnRaycastHit?.Invoke(hit);

            }
            else
            {
                // 处理未点击到物体
                touchedObject = null;
                OnTouchedObjectSelect?.Invoke(null);

            }
        }

    }

    private void HandleTouchRaycast()
    {
        if (Input.touchCount == 1)
        {
            Touch touch0 = Input.GetTouch(0);
            // 检测触摸是否在 UI 上
            if (EventSystem.current.IsPointerOverGameObject(touch0.fingerId))
            {
                return; // 如果触摸在 UI 上，则不处理相机逻辑
            }
            if (touch0.deltaPosition != Vector2.zero)
            {
                return;
            }
            if (touch0.phase == TouchPhase.Ended)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch0.position);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, raycastMask))
                {
                    // 处理点击到的物体
                    touchedObject = hit.transform.gameObject;
                    OnTouchedObjectSelect?.Invoke(touchedObject);
                    OnRaycastHit?.Invoke(hit);

                }
                else
                {
                    // 处理未点击到物体
                    touchedObject = null;
                    OnTouchedObjectSelect?.Invoke(null);


                }
            }
        }

    }

    public void SetControlSwitch(bool isOn)
    {
        controlSwitch = isOn;
    }

    public void ResetPosition()
    {
        target.position = targetoriginalPos;
        target.eulerAngles = targetoriginalRotate;
        currentDistance = Distance;
        transform.position = originalPos;
        transform.eulerAngles = originalRotate;
        currentRotation = new Vector2(transform.eulerAngles.y, transform.eulerAngles.x);

        transform.position = target.position - transform.forward * currentDistance;

    }

}
