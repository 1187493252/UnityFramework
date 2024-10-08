/*
* FileName:          TMPRichText
* CompanyName:       
* Author:            relly
* Description:       
*/

namespace TMPro
{
    public static class TMPRichText
    {

        /// <summary>
        /// 换行
        /// </summary>
        /// <param name="tMP_Text"></param>
        /// <returns></returns>
        public static string NextLine()
        {
            return $"\n";
        }

        /// <summary>
        /// 控制下一行开始的高度,直到结束标记位置，该标记不会改变当前行
        /// </summary>
        /// <param name="size">可以使用像素(10em)、字体单位(30)或百分比(100%)</param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string LineHeight(string size, string text)
        {
            return $"<line-height={size}>{text}</line-height>";
        }

        /// <summary>
        /// 调整字符间距
        /// </summary>
        /// <param name="size">可以使用像素(10em)、字体单位(30)</param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string CharacterSpacing(string size, string text)
        {
            return $"<cspace={size}>{text}</cspace>";
        }

        /// <summary>
        /// 切换不同字体
        /// </summary>
        /// <param name="fontAssetName">字体资源名</param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Font(string fontAssetName, string text)
        {
            return $"<font=\"{fontAssetName}\">{text}</font>";
        }

        /// <summary>
        /// 切换不同字体
        /// </summary>
        /// <param name="fontAssetName">字体资源名</param>
        /// <param name="materialName">字体资源材质</param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Font(string fontAssetName, string materialName, string text)
        {
            return $"<font=\"{fontAssetName}\" material=\"{materialName}\">{text}</font>";
        }

        /// <summary>
        /// 调整文本的左边距
        /// </summary>
        /// <param name="size">可以使用像素(10em)、字体单位(30)或百分比(100%)</param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string MarginLeft(string size, string text)
        {
            return $"<margin-left={size}>{text}</margin>";
        }

        /// <summary>
        /// 调整文本的右边距
        /// </summary>
        /// <param name="size">可以使用像素(10em)、字体单位(30)或百分比(100%)</param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string MarginRight(string size, string text)
        {
            return $"<margin-right={size}>{text}</margin>";
        }

        /// <summary>
        /// 调整文本的左右边距
        /// </summary>
        /// <param name="size">可以使用像素(10em)、字体单位(30)或百分比(100%)</param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Margin(string size, string text)
        {
            return $"<margin={size}>{text}</margin>";
        }

        /// <summary>
        /// 显示标记文本,比如<b>
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Noparse(string text)
        {
            return $"<noparse>{text}</noparse>";
        }


        /// <summary>
        /// 保持文本在一起，不会被自动换行分开
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Nobr(string text)
        {
            return $"<nobr>{text}</nobr>";
        }

        /// <summary>
        /// 控制水平插入位置,可以放在同一行的任何地方，此标签最好与左对齐一起使用。
        /// </summary>
        /// <param name="size">可以使用像素(10em)、字体单位(30)或百分比(100%)</param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string HorizontalPos(string size, string text)
        {
            return $"<pos={size}>{text}</pos>";
        }

        /// <summary>
        /// 垂直偏移
        /// </summary>
        /// <param name="size">可以使用像素(10em)、字体单位(30)或百分比(100%)</param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Voffset(string size, string text)
        {
            return $"<voffset={size}>{text}</voffset>";
        }


        /// <summary>
        /// 缩进,效果会跨行持续存在,影响自动换行
        /// </summary>
        /// <param name="size">可以使用像素(10em)、字体单位(30)或百分比(100%)</param>
        /// <returns></returns>
        public static string Indent(string size)
        {
            return $"<indent={size}>";
        }

        /// <summary>
        /// 缩进,效果会跨行持续存在,影响自动换行
        /// </summary>
        /// <param name="size">可以使用像素(10em)、字体单位(30)或百分比(100%)</param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Indent(string size, string text)
        {
            return $"<indent={size}>{text}";
        }

        /// <summary>
        /// 缩进不带结束标记,只影响手动换行符,不影响自动换行
        /// </summary>
        /// <param name="size">可以使用像素(10em)、字体单位(30)或百分比(100%)</param>
        /// <returns></returns>
        public static string LineIndent(string size)
        {
            return $"<line-indent={size}>";
        }

        /// <summary>
        /// 缩进带结束标记,只影响手动换行符,不影响自动换行
        /// </summary>
        /// <param name="size">可以使用像素(10em)、字体单位(30)或百分比(100%)</param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string LineIndent(string size, string text)
        {
            return $"<line-indent={size}>{text}</line-indent>";
        }

        /// <summary>
        /// 链接
        /// </summary>
        /// <param name="id">LinkId,一般填跳转链接</param>
        /// <param name="text">显示的文本</param>
        /// <param name="color">常用颜色英文,或者十六进制</param>
        /// <returns></returns>
        public static string Link(string id, string text, string color = "blue")
        {
            return $"<link=\"{id}\">{Color(color, text)}</link>";
        }

        /// <summary>
        /// 对齐方式
        /// </summary>
        /// <param name="alignType">left,center,right</param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Align(string alignType, string text)
        {
            return $"<align=\"{alignType}\">{text}";
        }

        /// <summary>
        /// 删除线
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Strikethrough(string text)
        {
            return $"<s>{text}</s>";
        }

        /// <summary>
        /// 下划线
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Underline(string text)
        {
            return $"<u>{text}</u>";
        }

        /// <summary>
        /// 加粗
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Bold(string text)
        {
            return $"<b>{text}</b>";
        }

        /// <summary>
        /// 斜体
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Italic(string text)
        {
            return $"<i>{text}</i>";
        }

        /// <summary>
        /// 所有字母都大写
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Uppercase(string text)
        {
            return $"<uppercase>{text}</uppercase>";
        }

        /// <summary>
        /// 所有内容都大写，但是以前小写的字母会小一点
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Smallcaps(string text)
        {
            return $"<smallcaps>{text}</smallcaps>";
        }

        /// <summary>
        /// 所有字母都小写
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Lowercase(string text)
        {
            return $"<lowercase>{text}</lowercase>";
        }

        /// <summary>
        /// 颜色带结束标记
        /// </summary>
        /// <param name="colorCode">常用颜色英文,或者十六进制</param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Color(string colorCode, string text)
        {
            return $"<color=\"{colorCode}\">{text}</color>";
        }

        /// <summary>
        /// 颜色不带结束标记
        /// </summary>
        /// <param name="colorCode">常用颜色英文,或者十六进制</param>
        /// <returns></returns>
        public static string Color(string colorCode)
        {
            return $"<color=\"{colorCode}\">";
        }

        /// <summary>
        /// 突出显示标记的文本
        /// </summary>
        /// <param name="text"></param>
        /// <param name="colorCode">常用颜色英文,或者十六进制</param>
        /// <returns></returns>
        public static string Mark(string text, string colorCode = "#ffff00aa")
        {
            return $"<mark=\"{colorCode}\">{text}</mark>";
        }

        /// <summary>
        /// 文本字体大小
        /// </summary>
        /// <param name="size">可以使用像素(10em)、字体单位(30)或百分比(100%)</param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Size(string size, string text)
        {
            return $"<size={size}>{text}</size>";
        }

        /// <summary>
        /// 图,从默认图集资源加载
        /// </summary>
        /// <param name="index">下标</param>
        /// <returns></returns>
        public static string Sprite(int index)
        {
            return $"<sprite index={index}>";
        }

        /// <summary>
        /// 图,从默认图集资源加载
        /// </summary>
        /// <param name="spriteName">图片名</param>
        /// <returns></returns>
        public static string Sprite(string spriteName)
        {
            return $"<sprite name=\"{spriteName}\">";
        }

        /// <summary>
        /// 图
        /// </summary>
        /// <param name="assetName">图集资源名</param>
        /// <param name="index">下标</param>
        /// <returns></returns>
        public static string Sprite(string assetName, int index)
        {
            return $"<sprite=\"{assetName}\" index={index}>";
        }

        /// <summary>
        /// 图
        /// </summary>
        /// <param name="assetName">图集资源名</param>
        /// <param name="spriteName">图片名</param>
        /// <returns></returns>
        public static string Sprite(string assetName, string spriteName)
        {
            return $"<sprite=\"{assetName}\" name=\"{spriteName}\">";
        }

        /// <summary>
        /// 上标
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Superscript(string text)
        {
            return $"<sup>{text}</sup>";
        }

        /// <summary>
        /// 下标
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Subscript(string text)
        {
            return $"<sub>{text}</sub>";
        }


    }
}