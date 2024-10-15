using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UnityFramework
{

    public class UtilityTools
    {

        /// <summary>
        /// Function returning the Square Distance from a Point to a Line.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static float DistanceToLine(Vector3 a, Vector3 b, Vector3 point)
        {
            Vector3 n = b - a;
            Vector3 pa = a - point;

            float c = Vector3.Dot(n, pa);

            // Closest point is a
            if (c > 0.0f)
                return Vector3.Dot(pa, pa);

            Vector3 bp = point - b;

            // Closest point is b
            if (Vector3.Dot(n, bp) > 0.0f)
                return Vector3.Dot(bp, bp);

            // Closest point is between a and b
            Vector3 e = pa - n * (c / Vector3.Dot(n, n));

            return Vector3.Dot(e, e);
        }

        /// <summary>
        /// 确定位置是否与RectTransform相交
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="position"></param>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static bool IsIntersectingRectTransform(RectTransform rectTransform, Vector3 position, Camera camera)
        {
            ScreenPointToWorldPointInRectangle(rectTransform, position, camera, out position);
            Vector3[] m_rectWorldCorners = new Vector3[4];
            rectTransform.GetWorldCorners(m_rectWorldCorners);

            if (PointIntersectRectangle(position, m_rectWorldCorners[0], m_rectWorldCorners[1], m_rectWorldCorners[2], m_rectWorldCorners[3]))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 将ScreenPoint转换为与矩形对齐的世界坐标
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="screenPoint"></param>
        /// <param name="cam"></param>
        /// <param name="worldPoint"></param>
        /// <returns></returns>
        public static bool ScreenPointToWorldPointInRectangle(Transform transform, Vector2 screenPoint, Camera cam, out Vector3 worldPoint)
        {
            worldPoint = Vector2.zero;
            Ray ray = RectTransformUtility.ScreenPointToRay(cam, screenPoint);

            float enter;
            if (!new Plane(transform.rotation * Vector3.back, transform.position).Raycast(ray, out enter))
                return false;

            worldPoint = ray.GetPoint(enter);

            return true;
        }

        /// <summary>
        /// 检查一个点是否在矩形中
        /// </summary>
        /// <param name="m"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        private static bool PointIntersectRectangle(Vector3 m, Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            Vector3 ab = b - a;
            Vector3 am = m - a;
            Vector3 bc = c - b;
            Vector3 bm = m - b;

            float abamDot = Vector3.Dot(ab, am);
            float bcbmDot = Vector3.Dot(bc, bm);

            return 0 <= abamDot && abamDot <= Vector3.Dot(ab, ab) && 0 <= bcbmDot && bcbmDot <= Vector3.Dot(bc, bc);
        }

        /// <summary>
        /// byte转AudioClip
        /// </summary>
        /// <param name="_bs"></param>
        /// <returns></returns>
        public static AudioClip ConvertBytesToClip(byte[] _bs)
        {
            float[] samples = new float[_bs.Length / 2];
            float rescaleFactor = 32767;
            short st = 0;
            float ft = 0;
            for (int i = 0; i < _bs.Length; i += 2)
            {
                st = BitConverter.ToInt16(_bs, i);
                ft = st / rescaleFactor;
                samples[i / 2] = ft;
            }
            AudioClip cp = AudioClip.Create("clip", samples.Length, 1, 16000, false);
            cp.SetData(samples, 0);
            return cp;
        }

        /// <summary>
        /// AudioClip转byte
        /// </summary>
        /// <param name="clip"></param>
        /// <returns></returns>
        public static byte[] ConvertClipToBytes(AudioClip clip)
        {
            float[] sample = new float[clip.samples];
            clip.GetData(sample, 0);
            short[] intdata = new short[sample.Length];
            byte[] bytesData = new byte[sample.Length * 2];
            int rescaleFactor = 32767;
            for (int i = 0; i < sample.Length; i++)
            {
                intdata[i] = (short)(sample[i] * rescaleFactor);
                byte[] byteArr = new byte[2];
                byteArr = BitConverter.GetBytes(intdata[i]);
                byteArr.CopyTo(bytesData, i * 2);
            }
            return bytesData;
        }

        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopyBinary<T>(T obj)
        {
            object rel;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                rel = bf.Deserialize(ms);
                ms.Close();
            }
            return (T)rel;
        }

        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopyJson<T>(T obj)
        {
            // 序列化
            string json = JsonHelper.ToJson(obj);
            // 反序列化
            return JsonHelper.ToObject<T>(json);
        }



        public static string WebUrlDecode(string content)
        {
            return WebUtility.UrlDecode(content);
        }
        public static string WebUrlEncode(string content)
        {
            return WebUtility.UrlEncode(content);
        }

        //获取16位安全验证随机数
        public static string GetSecurityCode()
        {
            List<string> verifyList = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
            string temp = "";
            for (int i = 0; i < verifyList.Count; i++)
            {
                int index = UnityEngine.Random.Range(0, 16);
                temp += verifyList[index];
            }
            return temp;
        }


        /// <summary>
        /// 对比两个list 大小 元素是否相等
        /// </summary>
        /// <param name="lsit1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static bool CompareEqual(List<int> lsit1, List<int> list2)
        {

            if (lsit1 == null || list2 == null || lsit1.Count != list2.Count)
            {
                return false;
            }
            for (int i = 0; i < lsit1.Count; i++)
            {
                if (lsit1[i] != list2[i])
                {
                    return false;
                }
            }
            return true;
        }



        /// <summary>
        /// 自适应全屏
        /// </summary>
        /// <param name="size"></param>
        /// <param name="isHor">是否适应横向分辨率</param>
        /// <returns></returns>
        public static Vector2 AspectScreen(Vector2 size)
        {
            Vector2 preSize = size;
            bool isHor = size.x / Screen.width < size.y / Screen.height;
            if (isHor)
            {
                preSize.x = Screen.width;
                preSize.y = size.y * (Screen.width / size.x);
            }
            else
            {
                preSize.y = Screen.height;
                preSize.x = size.x * (Screen.height / size.y);
            }
            return preSize;
        }

        /// <summary>
        /// 安卓端刷新相册代码
        /// </summary>
        /// <param name="path"></param>
        public static void ScanPhoto(string[] path)
        {
            using (AndroidJavaClass PlayerActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject playerActivity = PlayerActivity.GetStatic<AndroidJavaObject>("currentActivity");
                using (AndroidJavaObject Conn = new AndroidJavaObject("android.media.MediaScannerConnection", playerActivity, null))
                {
                    Conn.CallStatic("scanFile", playerActivity, path, null, null);
                }
            }
        }

        public static void LookForwardV(Transform trans, Transform target, int isLookAt = 1)
        {
            Vector3 forward = -target.forward;
            forward.y = 0;
            if (forward != Vector3.zero)
            {
                trans.forward = forward * isLookAt;
            }
        }

        /// <summary>
        /// 看向目标或者背向目标，并保持竖直方向垂直
        /// </summary>
        /// <param name="trans">要设置朝向的物体</param>
        /// <param name="target">参考的目标</param>
        /// <param name="isLookAt">是否正对</param>
        public static void LookAtV(Transform trans, Transform target, int isLookAt = 1)
        {
            Vector3 forward = (target.position - trans.position).normalized;
            forward.y = 0;
            if (forward != Vector3.zero)
            {
                trans.forward = forward * isLookAt;
            }
        }
        /// <summary>
        /// 指向目标 或者背对目标
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="target"></param>
        /// <param name="isLookAt"></param>
        public static void LookAt(Transform trans, Transform target, bool isLookAt = true)
        {
            if (isLookAt)
            {
                trans.LookAt(target);
                return;
            }
            trans.rotation = Quaternion.LookRotation(trans.position - target.position);
        }
        /// <summary>
        /// string 转Color 
        /// RGBA（1,1,1,1）
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color ParseColor(string color)
        {
            Color c = Color.white;
            string[] colors = color.Replace("RGBA", "").Replace("(", "").Replace(")", "").Split(',');
            float r = float.Parse(colors[0]);
            c.r = ColorValueChange(r);
            float g = float.Parse(colors[1]);
            c.g = ColorValueChange(g);
            c.b = ColorValueChange(float.Parse(colors[2]));
            c.a = ColorValueChange(float.Parse(colors[3]));
            return c;
        }
        private static float ColorValueChange(float r)
        {
            return r > 1.01 ? (r / 255.0f) : r;
        }

        /// <summary>
        /// string 转Vector
        /// (0,0,0)
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector3 ParseVectorWithParenthesis(string vector)
        {
            Vector3 v = Vector3.zero;
            string[] vectors = vector.Replace("(", "").Replace(")", "").Split(',');
            for (int i = 0; i < vectors.Length; i++)
            {
                if (i == 0)
                {
                    v.x = float.Parse(vectors[i]);
                }
                else if (i == 1)
                {
                    v.y = float.Parse(vectors[i]);
                }
                else if (i == 2)
                {
                    v.z = float.Parse(vectors[i]);
                }
            }
            return v;
        }

        /// <summary>
        /// string 转Vector 0,0,0
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static Vector3 ParseVector(string vector)
        {
            Vector3 v = Vector3.zero;
            string[] vectors = vector.Split(',');
            for (int i = 0; i < vectors.Length; i++)
            {
                if (i == 0)
                {
                    v.x = float.Parse(vectors[i]);
                }
                else if (i == 1)
                {
                    v.y = float.Parse(vectors[i]);
                }
                else if (i == 2)
                {
                    v.z = float.Parse(vectors[i]);
                }
            }
            return v;
        }

        /// <summary>
        /// 改变btns按钮图片为normial,btn改为press
        /// </summary>
        /// <param name="btns">要变成正常状态UI的button数组</param>
        /// <param name="btn">变成按下状态UI的按钮</param>
        /// <param name="normial">正常状态UI</param>
        /// <param name="press">按下状态UI</param>
        public static void ChangeImg(Button[] btns, Button btn, Sprite normial, Sprite press)
        {
            foreach (var item in btns)
            {
                item.image.overrideSprite = normial;
            }
            btn.image.overrideSprite = press;

        }
        /// <summary>
        /// 改变btns按钮图片为img
        /// </summary>
        /// <param name="img">按钮状态UI</param>
        /// <param name="btns">要改变的按钮(可以是多个按钮对象以","分开)</param>
        public static void ChangeImg(Sprite img, params Button[] btns)
        {
            foreach (var item in btns)
            {
                item.image.overrideSprite = img;
            }

        }

        /// <summary>
        /// 给按钮添加事件
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="call"></param>
        public static void BtnAddEvent(Button btn, UnityAction call)
        {
            btn.onClick.AddListener(call);
        }
        /// <summary>
        /// 给有button组件的GameObject添加事件
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="call"></param>
        public static void BtnAddEvent(GameObject btn, UnityAction call)
        {
            btn.GetComponent<Button>().onClick.AddListener(call);
        }

        //-----------------------------------------------------

        /// <summary>
        /// 显示物体
        /// </summary>
        /// <param name="objs"></param>
        public static void ShowObj(GameObject obj)
        {
            if (!obj)
            {
                return;
            }
            obj.SetActive(true);
        }
        /// <summary>
        /// 显示物体
        /// </summary>
        /// <param name="objs"></param>
        public static void ShowObj(Transform obj)
        {
            if (!obj)
            {
                return;
            }
            obj.gameObject.SetActive(true);
        }
        /// <summary>
        /// 显示多个物体
        /// </summary>
        /// <param name="objs"></param>
        public static void ShowObj(params GameObject[] objs)
        {
            foreach (var item in objs)
            {
                ShowObj(item);
            }
        }
        /// <summary>
        /// 显示多个物体
        /// </summary>
        /// <param name="objs"></param>
        public static void ShowObj(params Transform[] objs)
        {
            foreach (var item in objs)
            {
                ShowObj(item);
            }
        }

        /// <summary>
        /// 显示多个物体
        /// </summary>
        /// <param name="objs"></param>
        public static void ShowObj(List<GameObject> objs)
        {
            foreach (var item in objs)
            {
                ShowObj(item);
            }
        }
        /// <summary>
        /// 显示多个物体
        /// </summary>
        /// <param name="objs"></param>
        public static void ShowObj(List<Transform> objs)
        {
            foreach (var item in objs)
            {
                ShowObj(item);
            }
        }
        /// <summary>
        /// 显示单个物体,隐藏其他物体
        /// </summary>
        /// <param name="showobj">要显示的物体</param>
        /// <param name="hideobjs">要隐藏的物体</param>
        public static void ShowObj(GameObject showobj, GameObject[] hideobjs)
        {
            foreach (var item in hideobjs)
            {
                HideObj(item);
            }
            ShowObj(showobj);
        }
        /// <summary>
        /// 显示单个物体,隐藏其他物体
        /// </summary>
        /// <param name="showobj">要显示的物体</param>
        /// <param name="hideobjs">要隐藏的物体</param>
        public static void ShowObj(GameObject showobj, Transform[] hideobjs)
        {
            foreach (var item in hideobjs)
            {
                HideObj(item);
            }
            ShowObj(showobj);
        }
        /// <summary>
        /// 显示单个物体,隐藏其他物体
        /// </summary>
        /// <param name="showobj">要显示的物体</param>
        /// <param name="hideobjs">要隐藏的物体</param>
        public static void ShowObj(Transform showobj, Transform[] hideobjs)
        {
            foreach (var item in hideobjs)
            {
                HideObj(item);
            }
            ShowObj(showobj);
        }
        /// <summary>
        /// 显示单个物体,隐藏其他物体
        /// </summary>
        /// <param name="showobj">要显示的物体</param>
        /// <param name="hideobjs">要隐藏的物体</param>
        public static void ShowObj(Transform showobj, GameObject[] hideobjs)
        {
            foreach (var item in hideobjs)
            {
                HideObj(item);
            }
            ShowObj(showobj);
        }

        /// <summary>
        /// 显示单个物体,隐藏其他物体
        /// </summary>
        /// <param name="showobj">要显示的物体</param>
        /// <param name="hideobjs">要隐藏的物体</param>
        public static void ShowObj(GameObject showobj, List<GameObject> hideobjs)
        {
            foreach (var item in hideobjs)
            {
                HideObj(item);
            }
            ShowObj(showobj);
        }
        /// <summary>
        /// 显示单个物体,隐藏其他物体
        /// </summary>
        /// <param name="showobj">要显示的物体</param>
        /// <param name="hideobjs">要隐藏的物体</param>
        public static void ShowObj(GameObject showobj, List<Transform> hideobjs)
        {
            foreach (var item in hideobjs)
            {
                HideObj(item);
            }
            ShowObj(showobj);
        }
        /// <summary>
        /// 显示单个物体,隐藏其他物体
        /// </summary>
        /// <param name="showobj">要显示的物体</param>
        /// <param name="hideobjs">要隐藏的物体</param>
        public static void ShowObj(Transform showobj, List<Transform> hideobjs)
        {
            foreach (var item in hideobjs)
            {
                HideObj(item);
            }
            ShowObj(showobj);
        }
        /// <summary>
        /// 显示单个物体,隐藏其他物体
        /// </summary>
        /// <param name="showobj">要显示的物体</param>
        /// <param name="hideobjs">要隐藏的物体</param>
        public static void ShowObj(Transform showobj, List<GameObject> hideobjs)
        {
            foreach (var item in hideobjs)
            {
                HideObj(item);
            }
            ShowObj(showobj);
        }


        /// <summary>
        /// 隐藏单个物体
        /// </summary>
        /// <param name="hideobj">要隐藏的物体</param>
        public static void HideObj(GameObject hideobj)
        {
            if (!hideobj)
            {
                return;
            }
            hideobj.SetActive(false);
        }
        /// <summary>
        /// 隐藏单个物体
        /// </summary>
        /// <param name="hideobj">要隐藏的物体</param>
        public static void HideObj(Transform hideobj)
        {
            if (!hideobj)
            {
                return;
            }
            hideobj.gameObject.SetActive(false);
        }
        /// <summary>
        /// 隐藏多个物体
        /// </summary>
        /// <param name="objs"></param>
        public static void HideObj(params GameObject[] objs)
        {
            foreach (var item in objs)
            {
                HideObj(item);
            }
        }
        /// <summary>
        /// 隐藏多个物体
        /// </summary>
        /// <param name="objs"></param>
        public static void HideObj(params Transform[] objs)
        {
            foreach (var item in objs)
            {
                HideObj(item);
            }
        }
        /// <summary>
        /// 隐藏物体
        /// </summary>
        /// <param name="objs"></param>
        public static void HideObj(List<GameObject> objs)
        {
            foreach (var item in objs)
            {
                HideObj(item);
            }
        }
        /// <summary>
        /// 隐藏物体
        /// </summary>
        /// <param name="objs"></param>
        public static void HideObj(List<Transform> objs)
        {
            foreach (var item in objs)
            {
                HideObj(item);
            }
        }
        /// <summary>
        /// 隐藏hideobj,显示showobjs
        /// </summary>
        /// <param name="hideobj">要隐藏的物体</param>
        /// <param name="showobjs">要显示的物体</param>
        public static void HideObj(GameObject hideobj, GameObject[] showobjs)
        {
            foreach (var item in showobjs)
            {
                ShowObj(item);
            }
            HideObj(hideobj);
        }
        /// <summary>
        /// 隐藏hideobj,显示showobjs
        /// </summary>
        /// <param name="hideobj">要隐藏的物体</param>
        /// <param name="showobjs">要显示的物体</param>
        public static void HideObj(GameObject hideobj, Transform[] showobjs)
        {
            foreach (var item in showobjs)
            {
                ShowObj(item);
            }
            HideObj(hideobj);
        }
        /// <summary>
        /// 隐藏hideobj,显示showobjs
        /// </summary>
        /// <param name="hideobj">要隐藏的物体</param>
        /// <param name="showobjs">要显示的物体</param>
        public static void HideObj(Transform hideobj, Transform[] showobjs)
        {
            foreach (var item in showobjs)
            {
                ShowObj(item);
            }
            HideObj(hideobj);
        }
        /// <summary>
        /// 隐藏hideobj,显示showobjs
        /// </summary>
        /// <param name="hideobj">要隐藏的物体</param>
        /// <param name="showobjs">要显示的物体</param>
        public static void HideObj(Transform hideobj, GameObject[] showobjs)
        {
            foreach (var item in showobjs)
            {
                ShowObj(item);
            }
            HideObj(hideobj);
        }

        /// <summary>
        /// 隐藏hideobj,显示showobjs
        /// </summary>
        /// <param name="hideobj">要隐藏的物体</param>
        /// <param name="showobjs">要显示的物体</param>
        public static void HideObj(GameObject hideobj, List<GameObject> showobjs)
        {
            foreach (var item in showobjs)
            {
                ShowObj(item);
            }
            HideObj(hideobj);
        }
        /// <summary>
        /// 隐藏hideobj,显示showobjs
        /// </summary>
        /// <param name="hideobj">要隐藏的物体</param>
        /// <param name="showobjs">要显示的物体</param>
        public static void HideObj(GameObject hideobj, List<Transform> showobjs)
        {
            foreach (var item in showobjs)
            {
                ShowObj(item);
            }
            HideObj(hideobj);
        }
        /// <summary>
        /// 隐藏hideobj,显示showobjs
        /// </summary>
        /// <param name="hideobj">要隐藏的物体</param>
        /// <param name="showobjs">要显示的物体</param>
        public static void HideObj(Transform hideobj, List<Transform> showobjs)
        {
            foreach (var item in showobjs)
            {
                ShowObj(item);
            }
            HideObj(hideobj);
        }
        /// <summary>
        /// 隐藏hideobj,显示showobjs
        /// </summary>
        /// <param name="hideobj">要隐藏的物体</param>
        /// <param name="showobjs">要显示的物体</param>
        public static void HideObj(Transform hideobj, List<GameObject> showobjs)
        {
            foreach (var item in showobjs)
            {
                ShowObj(item);
            }
            HideObj(hideobj);
        }
        /// <summary>
        /// 如果物体当前为显示就隐藏,如果隐藏就显示
        /// </summary>
        /// <param name="obj">物体</param>
        public static void ChangeActiveSelf(GameObject obj)
        {
            if (obj.activeSelf)
            {
                HideObj(obj);
            }
            else
            {
                ShowObj(obj);
            }
        }
        /// <summary>
        /// 如果物体当前为显示就隐藏,如果隐藏就显示
        /// </summary>
        /// <param name="obj">物体</param>
        public static void ChangeActiveSelf(Transform obj)
        {
            if (obj.gameObject.activeSelf)
            {
                HideObj(obj);
            }
            else
            {
                ShowObj(obj);
            }
        }

        //-----------------------------------------

        /// <summary>
        /// 继续播放当前动画
        /// </summary>
        /// <param name="anim"></param>
        public static void ContinuePlayAnim(Animation anim)
        {
            anim[anim.clip.name].speed = 1;
        }
        /// <summary>
        /// 暂停当前动画
        /// </summary>
        /// <param name="anim"></param>
        public static void PauseAnim(Animation anim)
        {
            anim[anim.clip.name].speed = 0;
        }
        /// <summary>
        /// 播放某动画
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="str"></param>
        public static void PlayAnim(int type, GameObject obj, string str)
        {
            switch (type)
            {
                case 0:
                    Animation animation = obj.GetComponent<Animation>();
                    if (animation)
                    {
                        animation.Play(str);
                    }
                    break;
                case 1:
                    Animator animator = obj.GetComponent<Animator>();
                    if (animator)
                    {
                        animator.Play(str);
                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="obj"></param>
        public static void PlayAnim(GameObject obj)
        {
            obj.GetComponent<Animation>().Play();
        }
        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="obj"></param>
        public static void PlayAnim(Animation obj)
        {
            obj.Play();
        }
        /// <summary>
        /// 播放某动画
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="str"></param>
        public static void PlayAnim(Animation obj, string str)
        {
            obj.Play(str);
        }
        /// <summary>
        /// 停止动画
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="str"></param>
        public static void StopAnim(GameObject obj, string str)
        {
            obj.GetComponent<Animation>().Stop(str);
        }
        /// <summary>
        /// 停止动画
        /// </summary>
        /// <param name="obj"></param>
        public static void StopAnim(GameObject obj)
        {
            obj.GetComponent<Animation>().Stop();
        }
        /// <summary>
        /// 停止动画
        /// </summary>
        /// <param name="obj"></param>
        public static void StopAnim(Animation obj)
        {
            obj.Stop();

        }
        /// <summary>
        /// 停止动画
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="str"></param>
        public static void StopAnim(Animation obj, string str)
        {
            obj.Stop(str);
        }
        /// <summary>
        /// 设置颜色
        /// </summary>
        /// <param name="_image">Image</param>
        /// <param name="_color">Color</param>
        public static void SetColor(Image _image, Color _color)
        {
            _image.color = _color;
        }
        /// <summary>
        /// 设置颜色
        /// </summary>
        /// <param name="_image">Image</param>
        /// <param name="_colorStr">Color 16进制</param>
        public static void SetColor(Image _image, string _colorStr)
        {
            if (ColorUtility.TryParseHtmlString(_colorStr, out Color _color))
            {
                _image.color = _color;
            }
        }

        /// <summary>
        /// 设置Image射线检测状态
        /// </summary>
        /// <param name="_image">Image</param>
        /// <param name="_rayCast">是否具有射线交互属性</param>
        public static void SetRayCastTarget(Image _image, bool _rayCast)
        {
            _image.raycastTarget = _rayCast;
        }

        /// <summary>
        /// 设置内容
        /// </summary>
        /// <param name="_text">Text</param>
        /// <param name="_content">修改的内容</param>
        public static void SetContent(Text _text, string _content)
        {
            _text.text = _content;
        }

        /// <summary>
        /// 退出游戏
        /// </summary>
        public static void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }


        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="name"></param>
        public static void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
        }
        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="num"></param>
        public static void LoadScene(int num)
        {
            SceneManager.LoadScene(num);
        }
        public static Transform FindChildByName(Transform _findParent, string _targetName)
        {
            Transform obj = _findParent.Find(_targetName);
            if (obj != null) return obj;
            for (int i = 0; i < _findParent.childCount; i++)
            {
                obj = FindChildByName(_findParent.GetChild(i), _targetName);
                if (obj != null)
                {
                    return obj;
                }
            }
            return null;
        }

        /// <summary>
        /// 从本地读取文件 返回Sprite
        /// </summary>
        /// <param name="_filePath">图片路径</param>
        /// <param name="textureWidth">图片宽度</param>
        /// <param name="textureHeight">图片高度</param>
        /// <returns></returns>
        public static Sprite GetSpriteByPath(string _filePath, int textureWidth, int textureHeight)
        {
            Sprite sprite = null;
            //读取路径下文件
            StreamReader reader = new StreamReader(_filePath);
            //获取字节流
            MemoryStream ms = new MemoryStream();
            reader.BaseStream.CopyTo(ms);
            byte[] bytes = ms.ToArray();

            sprite = ConverSpriteByData(bytes, textureWidth, textureHeight);

            reader.Close();

            return sprite;
        }
        /// <summary>
        /// Json字符串转码
        /// </summary>
        /// <param name="_json"></param>
        /// <returns></returns>
        public static string EncodingJson(string _json)
        {
            string content = "";
            Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
            content = reg.Replace(_json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
            return content;
        }
        /// <summary>
        /// 64位字符串转化为精灵图片
        /// </summary>
        /// <param name="_spriteStr"></param>
        /// <returns></returns>
        public static Sprite GetSpriteBy64Str(string _spriteStr, int textureWidth, int textureHeight)
        {
            Sprite sprite = null;
            byte[] bytes = Convert.FromBase64String(_spriteStr);
            sprite = ConverSpriteByData(bytes, textureWidth, textureHeight);
            return sprite;
        }

        /// <summary>
        /// 字节转化为精灵图片
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static Sprite ConverSpriteByData(byte[] datas, int textureWidth, int textureHeight)
        {
            Sprite sprite = null;

            //定义Textrue2D并转化为精灵图片          
            Texture2D texture = new Texture2D(textureWidth, textureHeight);
            texture.LoadImage(datas);
            sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one / 2);

            return sprite;
        }

        public static void ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }

        /// <summary>
        /// 获取当前物体和目标点的水平角度值
        /// 同时判断是左边还是右边
        /// </summary>
        /// <returns></returns>
        public static int IsPositionAtRight(Transform body, Vector3 point, out float distance)
        {
            Vector3 hPoint = point;
            hPoint.y = body.position.y;
            var forward = body.TransformDirection(Vector3.right);
            var toOther = hPoint - body.position;
            distance = Vector3.Distance(hPoint, body.position);
            float dot = Vector3.Dot(forward.normalized, toOther.normalized);
            if (Mathf.Abs(dot) < 0.003)
            {
                return 0;
            }
            else if (dot > 0)
            {
                return 1;
            }
            return -1;
        }
        /// <summary>
        /// 判断一个点是否在物体的前方
        /// </summary>
        /// <returns></returns>
        public static int IsPositionAtForward(Transform body, Vector3 point, out float distance)
        {
            Vector3 hPoint = point;
            hPoint.y = body.position.y;
            var forward = body.TransformDirection(Vector3.forward);
            var toOther = hPoint - body.position;
            distance = Vector3.Distance(hPoint, body.position);
            float dot = Vector3.Dot(forward.normalized, toOther.normalized);
            if (dot >= 0)
            {
                return 1;
            }
            return -1;
        }


    }
}



