using UnityEngine;

public class LinkTest : MonoBehaviour
{
    public LinkSpriteText _text;
    [TextArea(3, 20)]
    public string content;
    string content1 = "<quad name=hp size=30 width=1 />\r\n<quad name=player size=30 width=1 />\t<quad name=hp size=30 width=1 />\n<quad name=hp size=30 width=1 />暴击<quad name=hp size=30 width=1 />命中概率<color=#FF5512>+9.0</color><quad name=player size=30 width=1 /><quad name=hp size=30 width=1 /><quad name=hp size=30 width=1 /><quad name=hp size=30 width=1 /><quad name=player size=30 width=1 /><quad name=hp size=30 width=1 /><quad name=hp size=30 width=1 /><a link=www.baidu.com><color=#FEFF00>[百度一下]</color></a><a link=csdn.net><color=#FF12DB>[CSDN]</color></a>";
    // Start is called before the first frame update
    void Start()
    {
        _text.text = content1;

        _text.onLinkClick.AddListener(OnClick);
    }

    void OnClick(string name, string link)
    {
        Debug.Log($"name:{name}|linek:{link}");
        //  Application.OpenURL(link);
    }


}