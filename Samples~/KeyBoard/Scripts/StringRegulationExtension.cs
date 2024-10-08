using System.Text.RegularExpressions;

namespace ZRKFramework
{
    /// <summary>
    /// �ַ������� ��չ
    /// </summary>
    public static class StringRegulationExtension
    {
        /// <summary>
        /// �ַ�����������
        /// </summary>
        /// <param name="content">��Ҫ���Ƶ��ַ���</param>
        /// <param name="regulation">����</param>
        /// <returns></returns>
        public static string RegulationLimit(this string content,string regulation)
        {
            if (regulation.Equals(StringRegulation.NoRegulation))
            {
                return content;
            }
            return Regex.Replace(content, regulation, "");
        }
    }

    /// <summary>
    /// �ṹ�� �ַ������� ������ʽ
    /// </summary>
    public struct StringRegulation
    {
        /// <summary>
        /// �޹���
        /// </summary>
        public const string NoRegulation = "";

        /// <summary>
        /// ����-������
        /// </summary>
        public const string Regulation1 = @"[^0-9]";

        /// <summary>
        /// ����-��Сд��ĸ+���ֽ��
        /// </summary>
        public const string Regulation2 = @"[^A-Za-z0-9]";
    }
}
