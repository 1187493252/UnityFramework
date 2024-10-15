/*
* FileName:          Entity_Cube
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityFramework.Runtime;

public class Entity_Cube : EntityBase
{
    EntityMouseInteraction entityClick;
    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        entityClick = GetComponent<EntityMouseInteraction>();
        entityClick.EntityProxy.RayProxy.OnPointerEnter += OnPointerEnter;
        entityClick.EntityProxy.RayProxy.OnPointerExit += OnPointerExit;
        entityClick.EntityProxy.RayProxy.OnPointerClick += OnPointerClick;


    }

    private void OnPointerClick(object arg1, object arg2)
    {
        // entityClick.EntityProxy.HighlightProxy.StopHighlight();
    }

    private void OnPointerExit(object arg1, object arg2)
    {
        //  entityClick.HighlightProxy.StopHighlight();
    }

    private void OnPointerEnter(object arg1, object arg2)
    {
        //entityClick.EntityProxy.HighlightProxy.StartHighlight();
    }

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
    }


}
