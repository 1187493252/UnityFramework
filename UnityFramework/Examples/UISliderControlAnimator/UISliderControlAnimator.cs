/*
* FileName:          UISlider
* CompanyName:       杭州中锐
* Author:            relly
* Description:       UISlider控制Animator动画播放
* 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityFramework
{
    public class UISliderControlAnimator : UnityFramework.UI.UISlider, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler
    {
        bool drag = false;
        public Animator Animator;
        AnimatorStateInfo animatorStateInfo;

        public void SetAnimTarget(Animator target)
        {
            Animator = target;
        }
        private void OnEnable()
        {
            component.value = 0;
            drag = false;
        }

        private void Update()
        {
            if (!drag)
            {
                if (Animator)
                {
                    animatorStateInfo = Animator.GetCurrentAnimatorStateInfo(0);
                    component.value = animatorStateInfo.normalizedTime;
                }
                else
                {
                    Hide();
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            drag = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            drag = false;
        }
        protected override void ValueChangeEvent(float _value)
        {
            //UpdateAuto_Anim(_value);
        }
        public void UpdateAuto_Anim(float normalizedTime)
        {
            animatorStateInfo = Animator.GetCurrentAnimatorStateInfo(0);
            Animator.Play(animatorStateInfo.fullPathHash, 0, normalizedTime);
        }

        public void OnDrag(PointerEventData eventData)
        {
            drag = true;

            UpdateAuto_Anim(component.value);
        }
        public void OnPointerDown(PointerEventData eventData)
        {

            UpdateAuto_Anim(component.value);
        }

    }
}