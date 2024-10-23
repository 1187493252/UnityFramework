/*
* FileName:          OnCollider
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OnCollider : MonoBehaviour
{

    public enum CompareType
    {
        Tag,
        Name,
    }
    [SerializeField]
    [Header("比较类型")]
    CompareType compareType;
    [SerializeField]
    [Header("比较内容")]
    string requiredTag;

    [SerializeField]
    public UnityEvent<Collision> onCollisionEnter;
    [SerializeField]
    public UnityEvent<Collision> onCollisionStay;
    [SerializeField]
    public UnityEvent<Collision> onCollisionExit;

    [SerializeField]
    public UnityEvent<Collider> onTriggerEnter;
    [SerializeField]
    public UnityEvent<Collider> onTriggerStay;
    [SerializeField]
    public UnityEvent<Collider> onTriggerExit;


    private void OnCollisionEnter(Collision collision)
    {
        if (CanInvoke(collision.gameObject))
            onCollisionEnter?.Invoke(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (CanInvoke(collision.gameObject))
            onCollisionStay?.Invoke(collision);
    }
    private void OnCollisionExit(Collision collision)
    {
        if (CanInvoke(collision.gameObject))
            onCollisionExit?.Invoke(collision);
    }

    void OnTriggerEnter(Collider other)
    {
        if (CanInvoke(other.gameObject))
            onTriggerEnter?.Invoke(other);
    }
    private void OnTriggerStay(Collider other)
    {
        if (CanInvoke(other.gameObject))
            onTriggerStay?.Invoke(other);
    }
    void OnTriggerExit(Collider other)
    {
        if (CanInvoke(other.gameObject))
            onTriggerExit?.Invoke(other);
    }

    public void Test1(int index)
    {
        Debug.LogError(index);
    }
    bool CanInvoke(GameObject otherGameObject)
    {
        bool boolean = true;
        if (!string.IsNullOrEmpty(requiredTag))
        {
            switch (compareType)
            {
                case CompareType.Tag:
                    boolean = otherGameObject.CompareTag(requiredTag);
                    break;
                case CompareType.Name:
                    boolean = string.Equals(otherGameObject.name, requiredTag);
                    break;
            }
        }
        return boolean;
    }
}
