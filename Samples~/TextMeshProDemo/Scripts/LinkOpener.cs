/*
* FileName:          LinkOpener 
* CompanyName:       
* Author:            
* Description:       
*/

using UnityEngine;
using UnityFramework.UI;

namespace TMPro
{
    public class LinkOpener : MonoBehaviour
    {

        TMPText tMPText;
        private void Awake()
        {
            tMPText = GetComponentInChildren<TMPText>();
        }

        private void OnEnable()
        {
            tMPText.OnLinkClick.AddListener(OnClickLink);
        }

        private void OnDisable()
        {
            tMPText.OnLinkClick.RemoveListener(OnClickLink);
        }
        private void OnClickLink(string linkID, string linkText, int linkIndex)
        {
            Application.OpenURL(linkID);
        }

        private void Start()
        {
            string content = "";
            content += TMPRichText.LineIndent("2em", $"{TMPRichText.Nobr($"{TMPRichText.Link("http://www.baidu.com", $"百度一下{TMPRichText.Sprite(0)}")}46546546544444444444444444444444444447")}");
            content += TMPRichText.NextLine();
            content += TMPRichText.Color("red", $"红红{TMPRichText.Color("blue", "Blue")} and red again.");
            content += TMPRichText.Mark("Mark");
            content += TMPRichText.NextLine();
            content += TMPRichText.HorizontalPos("30%", "百度4654455");
            content += TMPRichText.NextLine();
            content += TMPRichText.Size("50%", "Echo7878");
            content += "百度";
            content += TMPRichText.HorizontalPos("19%", $"{TMPRichText.Voffset("0.3em", $"{TMPRichText.Sprite("E0", 0)}")}{TMPRichText.Voffset("0.3em", $"{TMPRichText.Sprite("E0", "E0")}")}");

            content += TMPRichText.NextLine();
            content += TMPRichText.LineHeight("2em", $"{ TMPRichText.Margin("50", $"65454545787878787878787878787878787878")}{ TMPRichText.MarginLeft("50", $"65454545787878787878787878787878787878788666677345")}{ TMPRichText.MarginRight("50", $"65454545787878787878787878787878787878788666677345")}");
            content += TMPRichText.NextLine();
            content += "\nlike ";
            content += TMPRichText.Font("Anton SDF", "Anton SDF - Drop Shadow", "a different font?");
            content += "or just";
            content += TMPRichText.Font("Anton SDF", "a different material?");
            content += "\nlike a different font? or just  a different material?";
            content += TMPRichText.NextLine();
            content += TMPRichText.Noparse("<b>");
            content += TMPRichText.Strikethrough("百度一下");
            content += TMPRichText.Underline("百度一下");
            content += TMPRichText.Bold("百度一下");
            content += TMPRichText.Italic("百度一下");
            content += TMPRichText.NextLine();
            content += TMPRichText.Uppercase("aasdaqwe");
            content += TMPRichText.Smallcaps("Smallcaps");
            content += TMPRichText.Lowercase("UGHIUFL");
            content += $"H{TMPRichText.Superscript("2")}O{TMPRichText.Subscript("3")}";

            tMPText.UpdateContent(content);


        }


    }
}