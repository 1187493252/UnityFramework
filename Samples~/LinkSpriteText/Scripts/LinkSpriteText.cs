using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Color = UnityEngine.Color;


/// <summary>
/// 文本控件，支持超链接、图片,不支持动图
/// </summary>
[AddComponentMenu("UI/LinkSpriteText", 10)]
public class LinkSpriteText : Text, IPointerClickHandler, IPointerDownHandler
{
    [TextArea(3, 10), SerializeField]
    private string inputText;

    /// <summary>
    /// 解析完最终的文本
    /// </summary>
    [TextArea(3, 10), SerializeField]
    private string _outputText;


    /// <summary>
    /// 对应的顶点输出的文本
    /// </summary>
    private string _vertexText;

    /// <summary>
    /// 是否需要解析
    /// </summary>
    private bool _mTextDirty = true;

    public override string text
    {
        get => _outputText;

        set
        {
            if (string.IsNullOrEmpty(value))
            {
                if (string.IsNullOrEmpty(text))
                {
                    return;
                }

                inputText = String.Empty;
                _mTextDirty = true;
                SetVerticesDirty();
            }
            else
            {
                if (inputText == value)
                {
                    return;
                }

                inputText = value;
                _mTextDirty = true;
                SetVerticesDirty();
            }
        }
    }

    public string customLinkColor
    {
        set
        {
            m_UniteLinkColor = HtmlStringToColor(value);
        }
        get
        {
            return ColorToHtmlString(m_UniteLinkColor);
        }
    }

    public bool m_UseUniteLinkColor = true;//true 统一所有Link颜色,false 自定义
    public Color m_UniteLinkColor = Color.blue;
    public bool m_QuadUseFontSize = false;//true 图片使用文字的大小
    [Range(0, 1)]
    public float m_OffsetY = 0.625f;//图片Y轴偏移


    public void SetLinkColor(string c)
    {
        customLinkColor = c;
        _mTextDirty = false;
    }
    /// <summary>
    /// 颜色转换 Color转16进制
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public string ColorToHtmlString(Color color)
    {
        return ColorUtility.ToHtmlStringRGB(color);
    }

    /// <summary>
    /// 颜色转换 16进制转Color
    /// </summary>
    /// <param name="htmlString">颜色16进制</param>
    /// <returns></returns>
    public Color HtmlStringToColor(string htmlString)
    {
        ColorUtility.TryParseHtmlString(htmlString, out Color color);

        return color;
    }



    /// <summary>
    /// 图片池
    /// </summary>
    private readonly List<Image> _mImagesPool = new List<Image>();

    /// <summary>
    /// 图片的最后一个顶点的索引
    /// </summary>
    private readonly List<int> _mImagesVertexIndex = new List<int>();

    /// <summary>
    /// 超链接信息列表
    /// </summary>
    private readonly List<LinkInfo> _mLinkInfos = new List<LinkInfo>();

    [Serializable]
    public class LinkClickEvent : UnityEvent<string, string>
    {
    }

    /// <summary>
    /// 超链接点击事件
    /// </summary>
    public LinkClickEvent onLinkClick;
    public LinkClickEvent onLinkDown;

    //  Dictionary<string, Action<string, string>> LinkClickDic = new Dictionary<string, Action<string, string>>();
    /// <summary>
    /// 正则取出所需要的属性
    /// </summary>
    private static readonly Regex ImageRegex =
        new Regex(@"<quad name=(.+?) size=(\d*\.?\d+%?) width=(\d*\.?\d+%?) />", RegexOptions.Singleline);

    /// <summary>
    /// 超链接正则
    /// </summary>
    private static readonly Regex LinkRegex =
        new Regex(@"<a link=([^>\n\s]+)>(.*?)(</a>)", RegexOptions.Singleline);

    /// <summary>
    /// 加载精灵图片位置
    /// </summary>
    string loadSprite = "LinkSpriteText/";

    protected override void Start()
    {
        base.Start();
    }
    public override void SetVerticesDirty()
    {
        base.SetVerticesDirty();
        _mTextDirty = true;
        UpdateQuadImage();
    }


    private void UpdateQuadImage()
    {
        if (_mTextDirty)
        {
            _outputText = GetOutputText(inputText);
        }


        _mImagesVertexIndex.Clear();
        int startSearchIndex = 0;
        var matches = ImageRegex.Matches(inputText);
        for (var i = 0; i < matches.Count; i++)
        {
            Match match = matches[i];
            int index = _vertexText.IndexOf('&', startSearchIndex);

            var firstIndex = index * 4;
            startSearchIndex = index + 1;

            _mImagesVertexIndex.Add(firstIndex);

            _mImagesPool.RemoveAll(image => image == null);
            if (_mImagesPool.Count == 0)
            {
                GetComponentsInChildren(_mImagesPool);
            }

            if (_mImagesVertexIndex.Count > _mImagesPool.Count)
            {
                var resources = new DefaultControls.Resources();
                var go = DefaultControls.CreateImage(resources);
                go.layer = gameObject.layer;
                var rt = go.transform as RectTransform;
                if (rt)
                {
                    rt.SetParent(rectTransform, false);
                    rt.localPosition = Vector3.zero;
                    rt.localRotation = Quaternion.identity;
                    rt.localScale = Vector3.one;
                }

                _mImagesPool.Add(go.GetComponent<Image>());
            }

            var spriteName = match.Groups[1].Value;
            var img = _mImagesPool[i];
            if (img.sprite == null || img.sprite.name != spriteName)
            {
                img.sprite = Resources.Load<Sprite>(loadSprite + spriteName);
            }

            var imgRectTransform = img.GetComponent<RectTransform>();
            //设置图片大小
            imgRectTransform.sizeDelta = new Vector2(fontSize, fontSize);
            if (!m_QuadUseFontSize)
            {
                if (Int32.TryParse(match.Groups[2].Value, out int size))
                {
                    imgRectTransform.sizeDelta = new Vector2(size, size);
                }
            }


            img.enabled = true;
        }

        for (var i = _mImagesVertexIndex.Count; i < _mImagesPool.Count; i++)
        {
            if (_mImagesPool[i])
            {
                _mImagesPool[i].enabled = false;
            }
        }
    }

    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        m_DisableFontTextureRebuiltCallback = true;
        base.OnPopulateMesh(toFill);
        UIVertex vert = new UIVertex();


        for (var i = 0; i < _mImagesVertexIndex.Count; i++)
        {
            var index = _mImagesVertexIndex[i];
            var rt = _mImagesPool[i].rectTransform;
            var size = rt.sizeDelta;
            if (index < toFill.currentVertCount)
            {
                //设置图片位置
                toFill.PopulateUIVertex(ref vert, index);
                rt.anchoredPosition = new Vector2(vert.position.x + size.x / 2, vert.position.y - size.y * m_OffsetY);
                toFill.PopulateUIVertex(ref vert, index);
                for (int j = index + 3, m = index; j > m; j--)
                {
                    toFill.SetUIVertex(vert, j);
                }
            }
        }


        // 处理超链接包围框
        foreach (var info in _mLinkInfos)
        {
            info.Boxes.Clear();
            if (info.StartIndex >= toFill.currentVertCount)
            {
                continue;
            }

            // 将超链接里面的文本顶点索引坐标加入到包围框
            toFill.PopulateUIVertex(ref vert, info.StartIndex);
            var pos = vert.position;
            var bounds = new Bounds(pos, Vector3.zero);
            for (int i = info.StartIndex, m = info.EndIndex; i < m; i++)
            {
                if (i >= toFill.currentVertCount)
                {
                    break;
                }

                toFill.PopulateUIVertex(ref vert, i);
                pos = vert.position;
                if (pos.x < bounds.min.x) // 换行重新添加包围框
                {
                    info.Boxes.Add(new Rect(bounds.min, bounds.size));
                    bounds = new Bounds(pos, Vector3.zero);
                }
                else
                {
                    bounds.Encapsulate(pos); // 扩展包围框
                }
            }

            info.Boxes.Add(new Rect(bounds.min, bounds.size));
        }

        m_DisableFontTextureRebuiltCallback = false;
    }


    /// <summary>
    /// 获取超链接解析后的最后输出文本 
    /// </summary>
    /// <returns></returns>
    protected virtual string GetOutputText(string outputText)
    {
        _mLinkInfos.Clear();
        if (string.IsNullOrEmpty(outputText))
            return "";
        string tempOutputText = outputText;
        _vertexText = outputText;
        _vertexText = Regex.Replace(_vertexText, "<color.*?>", "");
        _vertexText = Regex.Replace(_vertexText, "</color>", "");
        _vertexText = ImageRegex.Replace(_vertexText, "&");
        _vertexText = _vertexText.Replace("\n", "");
        _vertexText = Regex.Replace(_vertexText, @"(?<!a)\s(?!link)", "");
        foreach (Match match in LinkRegex.Matches(_vertexText))
        {

            var group = match.Groups[1];
            _vertexText = _vertexText.Replace(match.Value, match.Groups[2].Value);

            // int startNum = _vertexText.IndexOf(match.Groups[2].Value, match.Index, StringComparison.Ordinal);

            int startNum = _vertexText.IndexOf(match.Groups[2].Value, 0, StringComparison.Ordinal);

            if (startNum < 0)
            {
                Debug.LogError("超链接顶点解析错误");
            }

            var info = new LinkInfo
            {
                StartIndex = startNum * 4, // 超链接里的文本起始顶点索引

                EndIndex = startNum * 4 + (match.Groups[2].Length - 1) * 4 + 3,
                Link = match.Groups[1].Value,
                Name = match.Groups[2].Value
            };
            _mLinkInfos.Add(info);
        }


        foreach (Match match in LinkRegex.Matches(outputText))
        {
            if (m_UseUniteLinkColor)
            {
                string tmp = Regex.Replace(match.Groups[2].Value, "<color.*?>", "");
                tmp = Regex.Replace(tmp, "</color>", "");
                //tempOutputText = tempOutputText.Replace(match.Value, $"<a link={match.Groups[1].Value}><color=#{customLinkColor}>{tmp}</color></a>");

                tempOutputText = tempOutputText.Replace(match.Value, $"<color=#{customLinkColor}>" + tmp + "</color>");
            }
        }

        //处理quad的size大小
        foreach (Match match in ImageRegex.Matches(tempOutputText))
        {
            if (m_QuadUseFontSize)
            {
                tempOutputText = tempOutputText.Replace(match.Value, $"<quad name={match.Groups[1].Value} size={fontSize} width={match.Groups[3].Value} />");
            }
        }
        //处理换\n \t
        tempOutputText = tempOutputText.Replace("\t", "\t");
        tempOutputText = tempOutputText.Replace("\r\n", "\n");

        _mTextDirty = false;

        return tempOutputText;
    }



    /// <summary>
    /// 点击事件检测是否点击到超链接文本
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, eventData.position, eventData.pressEventCamera, out var lp);

        foreach (var info in _mLinkInfos)
        {
            var boxes = info.Boxes;
            for (var i = 0; i < boxes.Count; ++i)
            {
                if (!boxes[i].Contains(lp)) continue;
                onLinkClick?.Invoke(info.Name, info.Link);
                return;
            }
        }
    }

    /// <summary>
    /// 点击事件检测是否点击到超链接文本
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, eventData.position, eventData.pressEventCamera, out var lp);

        foreach (var info in _mLinkInfos)
        {
            var boxes = info.Boxes;
            for (var i = 0; i < boxes.Count; ++i)
            {
                if (!boxes[i].Contains(lp)) continue;
                onLinkDown?.Invoke(info.Name, info.Link);
                return;
            }
        }
    }

#if UNITY_EDITOR
    Vector3[] _textWolrdVertexs = new Vector3[4];
    private void OnDrawGizmos()
    {
        Rect rect = new Rect();

        for (int i = 0; i < _mLinkInfos.Count; i++)
        {
            for (int j = 0; j < _mLinkInfos[i].Boxes.Count; j++)
            {
                rect = _mLinkInfos[i].Boxes[j];
                _textWolrdVertexs[0] = TransformPoint2World(transform, rect.position);
                _textWolrdVertexs[1] = TransformPoint2World(transform, new Vector3(rect.x + rect.width, rect.y));
                _textWolrdVertexs[2] = TransformPoint2World(transform, new Vector3(rect.x + rect.width, rect.y + rect.height));
                _textWolrdVertexs[3] = TransformPoint2World(transform, new Vector3(rect.x, rect.y + rect.height));

                GizmosDrawLine(Color.green, _textWolrdVertexs);
            }
        }
    }

    //划线
    private void GizmosDrawLine(Color color, Vector3[] pos)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(pos[0], pos[1]);
        Gizmos.DrawLine(pos[1], pos[2]);
        Gizmos.DrawLine(pos[2], pos[3]);
        Gizmos.DrawLine(pos[3], pos[0]);
    }
#endif

    /// <summary>
    /// 获取Transform的世界坐标
    /// </summary>
    /// <param name="rectTransform"></param>
    /// <param name="canvas"></param>
    /// <returns></returns>
    public static Vector3 TransformPoint2World(Transform transform, Vector3 point)
    {
        return transform.localToWorldMatrix.MultiplyPoint(point);
    }

    /// <summary>
    /// 获取Transform的本地坐标
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    public static Vector3 TransformWorld2Point(Transform transform, Vector3 point)
    {
        return transform.worldToLocalMatrix.MultiplyPoint(point);
    }


    /// <summary>
    /// 超链接信息类
    /// </summary>
    private class LinkInfo
    {
        public int StartIndex;

        public int EndIndex;

        public string Link;

        public string Name;

        public readonly List<Rect> Boxes = new List<Rect>();
    }
}
