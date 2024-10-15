/*
* FileName:          GameObjectExtension
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace UnityFramework
{
    public static class GameObjectExtension
    {

        public static GameObject Clone(this GameObject gameObject, Transform parent)
        {
            return GameObject.Instantiate(gameObject, parent);

        }
        public static GameObject Clone(this GameObject gameObject, Vector3 position, Quaternion rotation)
        {
            return GameObject.Instantiate(gameObject, position, rotation);
        }

        public static GameObject Clone(this GameObject gameObject, Vector3 position, Quaternion rotation, Transform parent)
        {
            return GameObject.Instantiate(gameObject, position, rotation, parent);
        }


        public static GameObject Clone(this GameObject gameObject)
        {
            return GameObject.Instantiate(gameObject);
        }


        /// <summary>
        /// 设置绝对位置的 x 坐标。
        /// </summary>
        /// <param name="transform"><see cref="GameObject" /> 对象。</param>
        /// <param name="newValue">x 坐标值。</param>
        public static void SetPositionX(this GameObject gameObject, float newValue)
        {
            Vector3 v = gameObject.transform.position;
            v.x = newValue;
            gameObject.transform.position = v;
        }

        /// <summary>
        /// 设置绝对位置的 y 坐标。
        /// </summary>
        /// <param name="transform"><see cref="GameObject" /> 对象。</param>
        /// <param name="newValue">y 坐标值。</param>
        public static void SetPositionY(this GameObject gameObject, float newValue)
        {
            Vector3 v = gameObject.transform.position;
            v.y = newValue;
            gameObject.transform.position = v;
        }

        /// <summary>
        /// 设置绝对位置的 z 坐标。
        /// </summary>
        /// <param name="transform"><see cref="GameObject" /> 对象。</param>
        /// <param name="newValue">z 坐标值。</param>
        public static void SetPositionZ(this GameObject gameObject, float newValue)
        {
            Vector3 v = gameObject.transform.position;
            v.z = newValue;
            gameObject.transform.position = v;
        }

        /// <summary>
        /// 增加绝对位置的 x 坐标。
        /// </summary>
        /// <param name="transform"><see cref="GameObject" /> 对象。</param>
        /// <param name="deltaValue">x 坐标值增量。</param>
        public static void AddPositionX(this GameObject gameObject, float deltaValue)
        {
            Vector3 v = gameObject.transform.position;
            v.x += deltaValue;
            gameObject.transform.position = v;
        }

        /// <summary>
        /// 增加绝对位置的 y 坐标。
        /// </summary>
        /// <param name="transform"><see cref="GameObject" /> 对象。</param>
        /// <param name="deltaValue">y 坐标值增量。</param>
        public static void AddPositionY(this GameObject gameObject, float deltaValue)
        {
            Vector3 v = gameObject.transform.position;
            v.y += deltaValue;
            gameObject.transform.position = v;
        }

        /// <summary>
        /// 增加绝对位置的 z 坐标。
        /// </summary>
        /// <param name="transform"><see cref="GameObject" /> 对象。</param>
        /// <param name="deltaValue">z 坐标值增量。</param>
        public static void AddPositionZ(this GameObject gameObject, float deltaValue)
        {
            Vector3 v = gameObject.transform.position;
            v.z += deltaValue;
            gameObject.transform.position = v;
        }

        /// <summary>
        /// 设置相对位置的 x 坐标。
        /// </summary>
        /// <param name="transform"><see cref="GameObject" /> 对象。</param>
        /// <param name="newValue">x 坐标值。</param>
        public static void SetLocalPositionX(this GameObject gameObject, float newValue)
        {
            Vector3 v = gameObject.transform.localPosition;
            v.x = newValue;
            gameObject.transform.localPosition = v;
        }

        /// <summary>
        /// 设置相对位置的 y 坐标。
        /// </summary>
        /// <param name="transform"><see cref="GameObject" /> 对象。</param>
        /// <param name="newValue">y 坐标值。</param>
        public static void SetLocalPositionY(this GameObject gameObject, float newValue)
        {
            Vector3 v = gameObject.transform.localPosition;
            v.y = newValue;
            gameObject.transform.localPosition = v;
        }

        /// <summary>
        /// 设置相对位置的 z 坐标。
        /// </summary>
        /// <param name="transform"><see cref="GameObject" /> 对象。</param>
        /// <param name="newValue">z 坐标值。</param>
        public static void SetLocalPositionZ(this GameObject gameObject, float newValue)
        {
            Vector3 v = gameObject.transform.localPosition;
            v.z = newValue;
            gameObject.transform.localPosition = v;
        }

        /// <summary>
        /// 增加相对位置的 x 坐标。
        /// </summary>
        /// <param name="transform"><see cref="GameObject" /> 对象。</param>
        /// <param name="deltaValue">x 坐标值。</param>
        public static void AddLocalPositionX(this GameObject gameObject, float deltaValue)
        {
            Vector3 v = gameObject.transform.localPosition;
            v.x += deltaValue;
            gameObject.transform.localPosition = v;
        }

        /// <summary>
        /// 增加相对位置的 y 坐标。
        /// </summary>
        /// <param name="transform"><see cref="GameObject" /> 对象。</param>
        /// <param name="deltaValue">y 坐标值。</param>
        public static void AddLocalPositionY(this GameObject gameObject, float deltaValue)
        {
            Vector3 v = gameObject.transform.localPosition;
            v.y += deltaValue;
            gameObject.transform.localPosition = v;
        }

        /// <summary>
        /// 增加相对位置的 z 坐标。
        /// </summary>
        /// <param name="transform"><see cref="GameObject" /> 对象。</param>
        /// <param name="deltaValue">z 坐标值。</param>
        public static void AddLocalPositionZ(this GameObject gameObject, float deltaValue)
        {
            Vector3 v = gameObject.transform.localPosition;
            v.z += deltaValue;
            gameObject.transform.localPosition = v;
        }

        /// <summary>
        /// 设置相对尺寸的 x 分量。
        /// </summary>
        /// <param name="transform"><see cref="GameObject" /> 对象。</param>
        /// <param name="newValue">x 分量值。</param>
        public static void SetLocalScaleX(this GameObject gameObject, float newValue)
        {
            Vector3 v = gameObject.transform.localScale;
            v.x = newValue;
            gameObject.transform.localScale = v;
        }

        /// <summary>
        /// 设置相对尺寸的 y 分量。
        /// </summary>
        /// <param name="transform"><see cref="GameObject" /> 对象。</param>
        /// <param name="newValue">y 分量值。</param>
        public static void SetLocalScaleY(this GameObject gameObject, float newValue)
        {
            Vector3 v = gameObject.transform.localScale;
            v.y = newValue;
            gameObject.transform.localScale = v;
        }

        /// <summary>
        /// 设置相对尺寸的 z 分量。
        /// </summary>
        /// <param name="transform"><see cref="GameObject" /> 对象。</param>
        /// <param name="newValue">z 分量值。</param>
        public static void SetLocalScaleZ(this GameObject gameObject, float newValue)
        {
            Vector3 v = gameObject.transform.localScale;
            v.z = newValue;
            gameObject.transform.localScale = v;
        }

        /// <summary>
        /// 增加相对尺寸的 x 分量。
        /// </summary>
        /// <param name="transform"><see cref="GameObject" /> 对象。</param>
        /// <param name="deltaValue">x 分量增量。</param>
        public static void AddLocalScaleX(this GameObject gameObject, float deltaValue)
        {
            Vector3 v = gameObject.transform.localScale;
            v.x += deltaValue;
            gameObject.transform.localScale = v;
        }

        /// <summary>
        /// 增加相对尺寸的 y 分量。
        /// </summary>
        /// <param name="transform"><see cref="GameObject" /> 对象。</param>
        /// <param name="deltaValue">y 分量增量。</param>
        public static void AddLocalScaleY(this GameObject gameObject, float deltaValue)
        {
            Vector3 v = gameObject.transform.localScale;
            v.y += deltaValue;
            gameObject.transform.localScale = v;
        }

        /// <summary>
        /// 增加相对尺寸的 z 分量。
        /// </summary>
        /// <param name="transform"><see cref="GameObject" /> 对象。</param>
        /// <param name="deltaValue">z 分量增量。</param>
        public static void AddLocalScaleZ(this GameObject gameObject, float deltaValue)
        {
            Vector3 v = gameObject.transform.localScale;
            v.z += deltaValue;
            gameObject.transform.localScale = v;
        }

        /// <summary>
        /// 二维空间下使 <see cref="GameObject" /> 指向指向目标点的算法，使用世界坐标。
        /// </summary>
        /// <param name="transform"><see cref="GameObject" /> 对象。</param>
        /// <param name="lookAtPoint2D">要朝向的二维坐标点。</param>
        /// <remarks>假定其 forward 向量为 <see cref="Vector3.up" />。</remarks>
        public static void LookAt2D(this GameObject gameObject, Vector2 lookAtPoint2D)
        {
            Vector3 vector = lookAtPoint2D.ToVector3() - gameObject.transform.position;
            vector.y = 0f;

            if (vector.magnitude > 0f)
            {
                gameObject.transform.rotation = Quaternion.LookRotation(vector.normalized, Vector3.up);
            }
        }



        /// <summary>
        /// 递归设置游戏对象的层次。
        /// </summary>
        /// <param name="gameObject"><see cref="GameObject" /> 对象。</param>
        /// <param name="layer">目标层次的编号。</param>
        public static void SetLayerRecursively(this GameObject gameObject, int layer)
        {
            foreach (Transform item in gameObject.transform)
            {
                item.gameObject.layer = layer;
            }
        }


        /// <summary>
        /// 获取或增加组件。
        /// </summary>
        /// <param name="gameObject">目标对象。</param>
        /// <param name="type">要获取或增加的组件类型。</param>
        /// <returns>获取或增加的组件。</returns>
        public static Component GetOrAddComponent(this GameObject gameObject, Type type)
        {
            Component component = gameObject.GetComponent(type);
            if (component == null)
            {
                component = gameObject.AddComponent(type);
            }

            return component;
        }

        /// <summary>
        /// 获取或增加组件。
        /// </summary>
        /// <typeparam name="T">要获取或增加的组件。</typeparam>
        /// <param name="gameObject">目标对象。</param>
        /// <returns>获取或增加的组件。</returns>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }

        /// <summary>
        /// 获取 GameObject 是否在场景中。
        /// </summary>
        /// <param name="gameObject">目标对象。</param>
        /// <returns>GameObject 是否在场景中。</returns>
        /// <remarks>若返回 true，表明此 GameObject 是一个场景中的实例对象；若返回 false，表明此 GameObject 是一个 Prefab。</remarks>
        public static bool InScene(this GameObject gameObject)
        {
            return gameObject.scene.name != null;
        }

        public static void SetParent(this GameObject gameObject, Transform parent)
        {
            if (gameObject)
            {
                gameObject.transform.SetParent(parent);
            }
        }
        public static void SetParent(this GameObject gameObject, GameObject parent)
        {
            if (gameObject)
            {
                gameObject.transform.SetParent(parent.transform);
            }
        }


        public static void Hide(this GameObject gameObject)
        {
            if (gameObject)
            {
                gameObject.SetActive(false);
            }
        }
        public static void Show(this GameObject gameObject)
        {
            if (gameObject)
            {
                gameObject.SetActive(true);
            }
        }
    }
}
