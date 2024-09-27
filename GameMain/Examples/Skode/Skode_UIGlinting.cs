using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Skode_UIGlinting : MonoBehaviour
{
    #region 闪耀
    //API:是否自动开启闪耀
    public bool AutoGlinting = true;
    //API:闪耀的颜色
    public Color GlintingColor = new Color(1, 0, 0, 1);

    //API:延时闪耀的时间
    public float delayGlintingTime = 0;
    //API:闪耀时长。当值为负数时，循环闪耀
    public float GlintingTime = -1;
    //API:闪动频率
    public float frequency = 0.7f;

    //开始闪烁、停止闪烁的开关
    bool isFirst = false;
    //切换闪烁（变亮、变暗）的开关
    bool isTab = false;

    //当前物体及其子物体的image
    Image[] imas;
    Color[] imasColor;  //初始颜色
    //当前物体及其子物体的Text
    Text[] texts;
    Color[] textsColor;
    #endregion

    #region Add：按次序点击闪耀
    //API:是否开启按次序点击闪耀：Start，第一个闪耀，点击第一个button，第二个开始闪耀，以此类推
    public bool useOrder = false;
    //API:要闪烁的物体，按顺序
    public GameObject[] glinObj;

    //界定该脚本是否是管理者脚本，便于禁用管理者脚本的collider
    [HideInInspector]
    public bool isController = true;

    //当前点击的物体
    [HideInInspector]
    public int currentObjNumber;

    //获得GameManager的原始控制脚本
    Skode_UIGlinting sug;

    [HideInInspector]
    public bool first = true;
    #endregion

    private void Start()
    {
        #region 闪耀：初始化
        if (AutoGlinting)
            Invoke("StartGlinting", delayGlintingTime);
        if (GlintingTime > 0)
            Invoke("StopGlinting", delayGlintingTime + GlintingTime);

        imas = GetComponentsInChildren<Image>();
        texts = GetComponentsInChildren<Text>();

        //记录初始颜色
        if (imas != null)
        {
            imasColor = new Color[imas.Length];
            for (int i = 0; i < imas.Length; i++)
            {
                imasColor[i] = new Color(imas[i].color.r, imas[i].color.g, imas[i].color.b, imas[i].color.a);
            }
        }
        if (texts != null)
        {
            textsColor = new Color[texts.Length];
            for (int i = 0; i < texts.Length; i++)
            {
                textsColor[i] = new Color(texts[i].color.r, texts[i].color.g, texts[i].color.b, texts[i].color.a);
            }
        }
        #endregion

        #region Add：按次序闪耀，初始化
        if (useOrder)
        {
            AutoGlinting = false;
            currentObjNumber = 0;

            //给所有要闪耀的物体添加本脚本
            if (glinObj != null)
            {
                foreach (var item in glinObj)
                {
                    if (item.GetComponent<Skode_UIGlinting>() == false)
                        item.AddComponent<Skode_UIGlinting>();
                    //同步闪光频率
                    item.GetComponent<Skode_UIGlinting>().frequency = frequency;
                    item.GetComponent<Skode_UIGlinting>().GlintingColor = GlintingColor;
                    item.GetComponent<Skode_UIGlinting>().isController = false;
                    item.GetComponent<Skode_UIGlinting>().useOrder = true;
                    if (item.GetComponent<Collider>() == null)
                        item.AddComponent<BoxCollider>();
                    //将所有闪的Collider禁用，在点击时，激活下一个闪的UI的Collider
                    item.GetComponent<BoxCollider>().enabled = false;
                    if (item.GetComponent<BoxCollider>() != null)
                        item.GetComponent<BoxCollider>().size = new Vector3(item.GetComponent<RectTransform>().sizeDelta.x, item.GetComponent<RectTransform>().sizeDelta.y, 1);
                }

                //禁用掉管理者脚本的collider
                if (isController&& GetComponent<BoxCollider>()!=null)
                    GetComponent<BoxCollider>().enabled = false;

                //第一个开始闪耀
                glinObj[0].GetComponent<Skode_UIGlinting>().StartGlinting();
                //激活第一个闪的UI的Collider
                glinObj[0].GetComponent<BoxCollider>().enabled = true;
            }

            sug = GameObject.Find("GameManager").GetComponent<Skode_UIGlinting>();
        }
        #endregion
    }

    private void Update()
    {
        #region 闪耀：逻辑判断
        if (isFirst)
        {
            if (isTab == false)
            {
                #region 要控制闪烁的属性
                if (imas != null)
                {
                    foreach (Image ima in imas)
                    {
                        ima.color = new Color(GlintingColor.r, GlintingColor.g, GlintingColor.b, GlintingColor.a -= Time.deltaTime * frequency);
                    }
                }
                if (texts != null)
                {
                    foreach (Text tex in texts)
                    {
                        tex.color = new Color(GlintingColor.r, GlintingColor.g, GlintingColor.b, GlintingColor.a -= Time.deltaTime * frequency);
                    }
                }
                #endregion
                if (GlintingColor.a <= 0.3f)
                {
                    isTab = true;
                }
            }
            if (isTab == true)
            {
                #region 要控制闪烁的属性
                if (imas != null)
                {
                    foreach (Image ima in imas)
                    {
                        ima.color = new Color(GlintingColor.r, GlintingColor.g, GlintingColor.b, GlintingColor.a += Time.deltaTime * frequency);
                    }
                }
                if (texts != null)
                {
                    foreach (Text tex in texts)
                    {
                        tex.color = new Color(GlintingColor.r, GlintingColor.g, GlintingColor.b, GlintingColor.a += Time.deltaTime * frequency);
                    }
                }
                #endregion
                if (GlintingColor.a >= 1)
                {
                    isTab = false;
                }
            }
        }
        #endregion
    }

    #region 闪耀：StartGlinting、StopGlinting
    public void StartGlinting()
    {
        isFirst = true;
    }

    /// <summary>
    /// 停止闪耀
    /// </summary>
    public void StopGlinting()
    {
        #region 还原初始颜色
        //若数组没初始化成功，也不会有数组的长度
        if (imas != null)
        {
            for (int i = 0; i < imas.Length; i++)
            {
                imas[i].color = imasColor[i];
            }
        }
        if (texts != null)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].color = textsColor[i];
            }
        }
        #endregion

        isFirst = false;
    }
    #endregion

    #region Add：按次序闪耀，OnMouseDown
    void OnMouseDown()
    {
        if (useOrder)
        {
            //若当前一个不是最后一个
            if (sug.currentObjNumber + 1 < sug.glinObj.Length)
            {
                sug.glinObj[sug.currentObjNumber].GetComponent<Skode_UIGlinting>().StopGlinting();
                sug.glinObj[sug.currentObjNumber + 1].GetComponent<Skode_UIGlinting>().StartGlinting();

                //激活下一个UI的Collider，取消激活当前UI的Collider
                sug.glinObj[sug.currentObjNumber].GetComponent<BoxCollider>().enabled = false;
                sug.glinObj[sug.currentObjNumber + 1].GetComponent<BoxCollider>().enabled = true;

                sug.currentObjNumber = sug.currentObjNumber + 1;
            }
            //当前一个是最后一个，停止闪现，释放权限，用户可随意点击
            else
            {
                sug.glinObj[sug.currentObjNumber].GetComponent<Skode_UIGlinting>().StopGlinting();

                foreach (GameObject obj in sug.glinObj)
                {
                    obj.GetComponent<BoxCollider>().enabled = true;
                }
            }
        }
        first = false;
    }

    //当正在闪烁的物体被disable时，执行该命令。first是保证该函数不管在外界，还是取消激活，都只调用一次
    private void OnDisable()
    {
        if (first && gameObject.activeSelf == true)
        {
            PointerEventData data = null;
            OnMouseDown();
        }
    }
    #endregion

    //重置闪烁，可从头开始再次闪烁
    public void Skode_UIGlintingReset()
    {
        if (glinObj != null)
        {
            foreach (var item in glinObj)
            {
                item.GetComponent<Skode_UIGlinting>().first = true;
                item.GetComponent<Skode_UIGlinting>().StopGlinting();
                if (GetComponent<BoxCollider>())
                    item.GetComponent<BoxCollider>().enabled = false;
            }

            sug.currentObjNumber = 0;

            //第一个开始闪耀
            glinObj[0].GetComponent<Skode_UIGlinting>().StartGlinting();
            //激活第一个闪的UI的Collider
            glinObj[0].GetComponent<BoxCollider>().enabled = true;
        }
    }
}

/*注：
 * 使用说明：该脚本功能类似学习场景脚本，用户按顺序点击闪光的UI后，用户才能随意点击任意UI
 * 1、该脚本需放在“GameManager”物体上
 * 2、要闪烁的UI不能重复使用。即点击一次后，要闪烁1，不能1 2 3 4 1，因为在OnPointerClick中first运行一次后被赋值false
 */
