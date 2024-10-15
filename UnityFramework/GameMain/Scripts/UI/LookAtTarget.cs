using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{

    public GameObject _targetObj;
    public bool isLockY;

    private Vector3 vec;
    public bool isNeedLookCamera = true;

    // Start is called before the first frame update
    void Start()
    {
        vec = Vector3.zero;
        if (_targetObj == null)
        {
            _targetObj = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }
    public void SetTargetObj(GameObject gameObject)
    {
        _targetObj = gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        if (isNeedLookCamera && _targetObj != null)
        {
            if (!isLockY)
            {
                transform.LookAt(_targetObj.transform);
            }
            else
            {
                vec.Set(_targetObj.transform.position.x, transform.position.y, _targetObj.transform.position.z);
                transform.LookAt(vec);
            }

        }
    }
}
