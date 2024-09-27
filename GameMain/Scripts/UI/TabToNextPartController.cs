/*
* FileName:          TabToNextController
* CompanyName:       
* Author:            relly
* Description:       Tab键切换选中组件
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabToNextPartController : MonoBehaviour
{

    private List<Selectable> selectables = new List<Selectable>();
    public bool SetSelectedFirst;
    // Use this for initialization
    void Start()
    {
        selectables.AddRange(transform.GetComponentsInChildren<Selectable>());
        if (SetSelectedFirst)
        {
            if (selectables == null || selectables.Count < 1)
            {
                return;
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(selectables[0].gameObject);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (selectables == null || selectables.Count < 1)
            {
                return;
            }
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                var v = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
                if (v != null)
                {
                    if (selectables.Contains(v))
                    {
                        var index = selectables.IndexOf(v);
                        index += 1;
                        index %= selectables.Count;
                        EventSystem.current.SetSelectedGameObject(selectables[index].gameObject);
                    }
                }
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(selectables[0].gameObject);
            }
        }
    }
}

