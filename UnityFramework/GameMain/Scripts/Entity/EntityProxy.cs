/*
* FileName:          EntityProxy
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using UnityEngine;

namespace UnityFramework.Runtime
{
    public class EntityProxy : MonoBehaviour
    {
        public enum HighlightProxyType
        {
            Highlighter,
            HighlightEffect
        }
        public enum AnimProxyType
        {
            Animation,
            Animator
        }
        public HighlightProxyType highlightProxyType;

        public AnimProxyType animProxyType;

        //--------------------
        private EntityBase entityBase;
        public EntityBase EntityBase
        {
            get
            {
                if (entityBase == null)
                {
                    entityBase = GetComponent<EntityBase>();
                }
                return entityBase;
            }
        }

        private TransformProxy transformProxy;
        public TransformProxy TransformProxy
        {
            get
            {
                if (transformProxy == null)
                {
                    transformProxy = new TransformProxy(this.gameObject);
                }
                return transformProxy;
            }
        }

        private ColliderProxy colliderProxy;
        public ColliderProxy ColliderProxy
        {
            get
            {
                if (colliderProxy == null)
                {
                    colliderProxy = new ColliderProxy(this.gameObject);
                }
                return colliderProxy;
            }
        }

        private RigidbodyProxy rigidbodyProxy;
        public RigidbodyProxy RigidbodyProxy
        {
            get
            {
                if (rigidbodyProxy == null)
                {
                    rigidbodyProxy = new RigidbodyProxy(this.gameObject);
                }
                return rigidbodyProxy;
            }
        }



        private AnimProxy animProxy;
        public AnimProxy AnimProxy
        {
            get
            {
                if (animProxy == null)
                {
                    animProxy = GetComponent<AnimProxy>();
                }
                return animProxy;
            }
        }

        private RayProxy rayProxy;
        public RayProxy RayProxy
        {
            get
            {
                if (rayProxy == null)
                {
                    rayProxy = new RayProxy();
                }
                return rayProxy;
            }
        }

    }
}
