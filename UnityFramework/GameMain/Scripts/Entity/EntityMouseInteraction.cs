/*
* FileName:          EntityClick
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityFramework.Runtime
{
    public class EntityMouseInteraction : EntityInteraction
    {

        protected virtual void OnMouseEnter()
        {
            EntityProxy.RayProxy.OnPointerEnter?.Invoke(this, new PointerEventData(EventSystem.current));
        }

        protected virtual void OnMouseOver()
        {
            EntityProxy.RayProxy.OnPointerHover?.Invoke(this, new PointerEventData(EventSystem.current));
        }

        protected virtual void OnMouseExit()
        {
            EntityProxy.RayProxy.OnPointerExit?.Invoke(this, new PointerEventData(EventSystem.current));
        }

        protected virtual void OnMouseDown()
        {
            EntityProxy.RayProxy.OnPointerDown?.Invoke(this, new PointerEventData(EventSystem.current));
        }

        protected virtual void OnMouseUp()
        {
            EntityProxy.RayProxy.OnPointerUp?.Invoke(this, new PointerEventData(EventSystem.current));
        }

        protected virtual void OnMouseUpAsButton()
        {
            EntityProxy.RayProxy.OnPointerClick?.Invoke(this, new PointerEventData(EventSystem.current));
        }

    }
}
