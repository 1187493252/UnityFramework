/*
* FileName:          ExcelToJson
* CompanyName:  
* Author:            relly
* Description:       
* 
*/

using ExcelDataReader;
using LitJson;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace UnityFramework.Editor
{

    /* 一张xlsx里面可有多张sheet表格
	 * 一张sheet就是一个数据集合 
	 * 根据sheet表字段生成对应的数据集合json
	 * 可以创建实体类
	 * 表格第一行为列名,以"!"开头会忽略该列
	 * 表格第二行为类型
	 * 表格第三行为备注
	 * 数组用,分割
	 */
    public class ExcelToJson : EditorWindow
    {
        /// <summary>
        /// 实体类保存目录
        /// </summary>
        internal static string entitySaveDir;
        /// <summary>
        /// excel文件路径
        /// </summary>
        internal static string excelPath;
        /// <summary>
        /// excel文件目录
        /// </summary>
        internal static string excelDir;
        /// <summary>
        /// json保存目录
        /// </summary>
        internal static string jsonSavePath;
        private Vector2 scrollPos;
        internal static string delimiter = "_";//数组/列表数据分隔符
        private void OnGUI()
        {

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            GUILayout.Space(5);

            GUILayout.Label("说明:Excel表格示例在UnityFramework/ExcelData ");
            GUILayout.Space(5);

            EditorGUILayout.BeginVertical();
            GUILayout.Label("Excel文件路径: ");
            GUILayout.Space(5);

            excelPath = GUILayout.TextField(excelPath);
            GUILayout.Space(5);

            if (GUILayout.Button("选择文件"))
            {
                SelectExcelFile();
            }

            EditorGUILayout.EndVertical();

            GUILayout.Space(5);

            EditorGUILayout.BeginVertical();
            GUILayout.Label("Excel文件目录: ");
            GUILayout.Space(5);
            excelDir = GUILayout.TextField(excelDir);
            GUILayout.Space(5);

            if (GUILayout.Button("选择文件目录"))
            {
                SelectExcelDir();
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(5);

            EditorGUILayout.BeginVertical();
            GUILayout.Label("Json保存路径: ");
            GUILayout.Space(5);
            jsonSavePath = GUILayout.TextField(jsonSavePath);
            GUILayout.Space(5);

            if (GUILayout.Button("选择Json保存目录"))
            {
                SelectJsonSaveDirectory();
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(5);
            EditorGUILayout.BeginVertical();
            GUILayout.Label("实体类保存路径: ");
            GUILayout.Space(5);
            entitySaveDir = GUILayout.TextField(entitySaveDir);
            GUILayout.Space(5);

            if (GUILayout.Button("选择实体类保存目录"))
            {
                SelectEntitySaveDirectory();
            }
            EditorGUILayout.EndVertical();

            GUILayout.Label("_________________________________________________________________________________________________");
            GUILayout.Space(5);
            GUILayout.Label("数组/列表 数据分割字符,仅限1个: 如  |  ,  _ ");
            GUILayout.Space(5);

            delimiter = GUILayout.TextField(delimiter);
            GUILayout.Space(5);

            GUILayout.Label("_________________________________________________________________________________________________");

            //画线
            //GL.LoadPixelMatrix();
            //GL.PushMatrix();
            //GL.Begin(1);
            //GL.Color(Color.black);
            //GL.Vertex(new Vector2(0, 320));

            //GL.Vertex(new Vector2(500, 320));

            //GL.End();
            //GL.PopMatrix();


            GUILayout.Space(5);

            if (GUILayout.Button("生成实体类"))
            {
                CreateEntities(excelPath);
            }
            GUILayout.Space(5);

            if (GUILayout.Button("批量生成实体类"))
            {
                BatchCreateEntities(excelDir);
            }
            GUILayout.Space(10);

            if (GUILayout.Button("生成Json"))
            {
                ConvertToJson(excelPath);
            }
            GUILayout.Space(5);

            if (GUILayout.Button("批量生成Json"))
            {
                BatchConvertToJson(excelDir);
            }
            GUILayout.Space(10);

            if (GUILayout.Button("默认路径"))
            {
                entitySaveDir = string.Concat(Application.dataPath, "/Scripts/EntityClass");
                excelPath = string.Concat(Application.dataPath, "/ExcelData");
                excelDir = string.Concat(Application.dataPath, "/ExcelData");
                jsonSavePath = string.Concat(Application.streamingAssetsPath, "/Data");

            }
            GUILayout.Space(5);

            if (GUILayout.Button("关闭"))
            {
                Close();
            }
            GUILayout.Space(5);

            EditorGUILayout.EndScrollView();



        }
        private void BatchCreateEntities(string _directory)
        {
            if (Directory.Exists(_directory))
            {
                string[] files = Directory.GetFiles(_directory, "*.xlsx");
                foreach (string file in files)
                {
                    CreateEntities(file);
                }
            }
        }
        void CreateEntities(string _filePath)
        {
            if (string.IsNullOrEmpty(_filePath) || !File.Exists(_filePath))
            {
                Debug.LogError($"CreateEntities  Error {_filePath}");
                return;
            }
            using (FileStream fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
            {
                using (ExcelPackage ep = new ExcelPackage(fs))
                {
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                    DataSet result = excelReader.AsDataSet();
                    // 判断Excel文件中是否存在数据表
                    if (result.Tables.Count < 1)
                    {
                        return;
                    }
                    for (int i = 0; i < result.Tables.Count; i++)
                    {
                        CreateEntity(result.Tables[i]);
                    }
                    AssetDatabase.Refresh();
                }
            }
        }
        void CreateEntity(DataTable sheet)
        {
            int columnCount = sheet.Columns.Count;
            int rowCount = sheet.Rows.Count;
            string dir = entitySaveDir;
            string path = $"{dir}/{sheet.TableName}.cs";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("// 此脚本为自动生成,请勿修改");
            sb.AppendLine("");
            sb.AppendLine($"public class {sheet.TableName}");
            sb.AppendLine("{");
            //遍历sheet首行每个字段描述的值
            for (int i = 0; i < columnCount; i++)
            {
                // 如果该列title以!开始则进入下一列
                if (sheet.Rows[0][i].ToString().StartsWith("!"))
                {
                    continue;
                }
                //Debug.LogError (sheet.Cells[0 , i].Text);
                if (rowCount > 2)//有备注
                {
                    sb.AppendLine("\t/// <summary>");
                    sb.AppendLine($"\t/// {sheet.Rows[2][i].ToString()}");
                    sb.AppendLine("\t/// </summary>");
                }

                string _type = sheet.Rows[1][i].ToString();
                if (_type.ToUpperFirst().Contains("List"))
                {
                    _type = _type.ToUpperFirst();
                }
                sb.AppendLine($"\tpublic {_type} {sheet.Rows[0][i].ToString()};");
            }

            sb.AppendLine("}");
            try
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                if (!File.Exists(path))
                {
                    File.Create(path).Dispose(); //避免资源占用
                }
                File.WriteAllText(path, sb.ToString());
                Debug.LogError($"生成{sheet.TableName}类成功");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"生成{sheet.TableName}类错误 :{e.Message}");
            }
        }
        /// <summary>
        /// 选择Excel文件
        /// </summary>
        private void SelectExcelFile()
        {
            string path = EditorUtility.OpenFilePanelWithFilters("选择Excel文件", excelPath, new string[] { "*", "xlsx" });
            if (path.Length != 0)
            {
                excelPath = path;

            }
        }

        /// <summary>
        /// 选择Excel文件夹
        /// </summary>
        private void SelectExcelDir()
        {
            string path = EditorUtility.OpenFolderPanel("选择Excel文件夹", Application.dataPath, "");
            if (path.Length != 0)
            {
                excelDir = path;

            }
        }
        /// <summary>
        /// 选择json保存目录
        /// </summary>
        private void SelectJsonSaveDirectory()
        {
            string path = EditorUtility.OpenFolderPanel("选择json保存目录", Application.dataPath, "");
            if (path.Length != 0)
            {
                jsonSavePath = path;
            }
        }
        /// <summary>
        /// 选择实体类保存目录
        /// </summary>
        private void SelectEntitySaveDirectory()
        {
            string path = EditorUtility.OpenFolderPanel("选择实体类保存目录", Application.dataPath, "");
            if (path.Length != 0)
            {
                entitySaveDir = path;
            }
        }
        /// <summary>
        /// 生成json
        /// </summary>
        /// <param name="_filePath"></param>
        private void ConvertToJson(string _filePath)
        {
            string _name = Path.GetFileNameWithoutExtension(_filePath);

            if (string.IsNullOrEmpty(_filePath) || !File.Exists(_filePath))
            {
                Debug.LogError($"json文件生成错误:{_filePath}");
                return;
            }
            using (FileStream fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (ExcelPackage ep = new ExcelPackage(fs))
                {
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);

                    DataSet result = excelReader.AsDataSet();
                    // 判断Excel文件中是否存在数据表
                    if (result.Tables.Count < 1)
                    {
                        return;
                    }
                    //存储整个excel数据,一张sheet一个字典
                    Dictionary<string, List<Dictionary<string, object>>> excelDataDic = new Dictionary<string, List<Dictionary<string, object>>>();

                    excelDataDic.Clear();
                    // 遍历所有工作表
                    for (int i = 0; i < result.Tables.Count; i++)
                    {
                        //用来判断是否有数据
                        bool hasData = false;
                        // 准备一个列表存储整个表的数据
                        List<Dictionary<string, object>> tableDataList = new List<Dictionary<string, object>>();
                        tableDataList.Clear();
                        int columnCount = result.Tables[i].Columns.Count;
                        int rowCount = result.Tables[i].Rows.Count;


                        // 读取数据
                        // 第一行列名
                        // 第二行类型
                        // 第三行备注
                        // 第四行开始
                        for (int j = 3; j < rowCount; j++)
                        {
                            // 如果该行首列数据为空则进入下一行
                            //if (string.IsNullOrEmpty(result.Tables[i].Rows[j][0].ToString()))
                            //{
                            //	continue;
                            //}
                            // 准备一个字典存储每一行的数据
                            Dictionary<string, object> rowData = new Dictionary<string, object>();

                            for (int k = 0; k < columnCount; k++)
                            {
                                //如果该列列名为空则进入下一个行
                                if (string.IsNullOrEmpty(result.Tables[i].Rows[0][k].ToString()))
                                {
                                    break;
                                }
                                // 如果该列列名以!开始则忽略进入下一列
                                if (result.Tables[i].Rows[0][k].ToString().StartsWith("!"))
                                {
                                    continue;
                                }
                                //读取第一行数据作为字段
                                string field = result.Tables[i].Rows[0][k].ToString();
                                //读取第二行数据转换成对应Type
                                string typestring = result.Tables[i].Rows[1][k].ToString();
                                System.Type type = GetTypeByString(typestring);
                                string content = result.Tables[i].Rows[j][k].ToString();

                                if (type != null)
                                {
                                    if (!string.IsNullOrEmpty(content))
                                    {
                                        try
                                        {
                                            // 将单元格里的数据转换为对应格式的内容
                                            object value = Convert.ChangeType(content, type);
                                            //Key-Value对应
                                            rowData[field] = value;
                                        }
                                        catch (Exception e)
                                        {
                                            if (type == typeof(string))
                                            {
                                                rowData[field] = "";
                                            }
                                            else
                                            {
                                                //转换失败初始化为默认值
                                                rowData[field] = type.IsValueType ? Activator.CreateInstance(type) : null;
                                            }

                                            Debug.LogError($"表格{result.Tables[i].TableName}第{j}行第{k}列,转换数据:{content}to{type} 错误,已初始化为默认值: {e}");
                                        }
                                    }
                                    else
                                    {
                                        if (type == typeof(string))
                                        {
                                            rowData[field] = "";
                                        }
                                        else
                                        {
                                            //如果当前表格没有值初始化为默认值
                                            rowData[field] = type.IsValueType ? Activator.CreateInstance(type) : null;
                                        }

                                    }

                                }
                                else
                                {
                                    //type = null content可能是数组
                                    object value = CovertToArrayorList(typestring, content);
                                    //Key-Value对应
                                    rowData[field] = value;
                                }
                            }
                            //添加到表数据中
                            tableDataList.Add(rowData);

                            if (rowData.Count > 0)
                            {
                                hasData = true;
                            }
                        }
                        if (hasData)
                        {
                            excelDataDic.Add(result.Tables[i].TableName, tableDataList);
                        }
                    }
                    //写入json文件
                    string jsonPath = $"{jsonSavePath}/{_name}.json";
                    if (!Directory.Exists(jsonSavePath))
                    {
                        Directory.CreateDirectory(jsonSavePath);
                    }
                    if (!File.Exists(jsonPath))
                    {
                        File.Create(jsonPath).Dispose();
                    }
                    string jsondata = JsonMapper.ToJson(excelDataDic);
                    //string jsondata = JsonConvert.SerializeObject(excelDataDic);
                    jsondata = Regex.Unescape(jsondata);
                    File.WriteAllText(jsonPath, jsondata);

                    Debug.Log($"生成json文件成功==>{jsonPath}");
                }
            }
            AssetDatabase.Refresh();

        }
        private void BatchConvertToJson(string _directory)
        {
            if (Directory.Exists(_directory))
            {
                string[] files = Directory.GetFiles(_directory, "*.xlsx");
                foreach (string file in files)
                {
                    ConvertToJson(file);
                }
            }
        }

        string GetTypeNameByString(string typeName)
        {
            string realTypeName = typeName;
            switch (typeName.Trim().ToLower())
            {
                case "bool":
                    realTypeName = "System.Boolean";
                    break;
                case "byte":
                    realTypeName = "System.Byte";
                    break;
                case "sbyte":
                    realTypeName = "System.SByte";
                    break;
                case "char":
                    realTypeName = "System.Char";
                    break;
                case "decimal":
                    realTypeName = "System.Decimal";
                    break;
                case "double":
                    realTypeName = "System.Double";
                    break;
                case "float":
                    realTypeName = "System.Single";
                    break;
                case "int":
                    realTypeName = "System.Int32";
                    break;
                case "uint":
                    realTypeName = "System.UInt32";
                    break;
                case "long":
                    realTypeName = "System.Int64";
                    break;
                case "ulong":
                    realTypeName = "System.UInt64";
                    break;
                case "object":
                    realTypeName = "System.Object";
                    break;
                case "short":
                    realTypeName = "System.Int16";
                    break;
                case "ushort":
                    realTypeName = "System.UInt16";
                    break;
                case "string":
                    realTypeName = "System.String";
                    break;
                case "date":
                case "datetime":
                    realTypeName = "System.DateTime";
                    break;
                case "guid":
                    realTypeName = "System.Guid";
                    break;
                default:
                    break;
            }
            return realTypeName;
        }

        System.Type GetTypeByString(string typeName)
        {
            System.Type result = null;

            string realTypeName = GetTypeNameByString(typeName);
            try
            {
                result = System.Type.GetType(realTypeName, true, true);
            }
            catch (Exception e)
            {
                Debug.Log($"基础类型转换错误,{typeName}可能为数组/列表: {e}");
            }
            return result;
        }


        /// <summary>
        /// 转换成数组/链表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        object CovertToArrayorList(string type, string value)
        {
            bool isList = false;
            if (type.ToUpperFirst().Contains("List"))
            {
                isList = true;
            }
            string[] strArry = value.Split(delimiter.ToCharArray()[0]);
            int lgh = strArry.Length;

            if (type.ToLower().Contains("int"))
            {
                int[] result = new int[lgh];
                for (int i = 0; i < lgh; i++)
                {
                    if (string.IsNullOrEmpty(strArry[i]))
                    {
                        result[i] = 0;
                    }
                    else
                    {
                        try
                        {
                            result[i] = int.Parse(strArry[i]);
                        }
                        catch (Exception e)
                        {
                            result[i] = 0;

                            Debug.LogError($"int.Parse 转换  {strArry[i]} 错误,已初始化为默认值: {e}");
                        }
                    }
                }
                if (isList)
                {
                    return result.ToList();
                }
                else
                {
                    return result;

                }
                //	return isList ? result.ToList() : result;
            }
            else if (type.ToLower().Contains("float"))
            {
                float[] result = new float[lgh];
                for (int i = 0; i < lgh; i++)
                {
                    if (string.IsNullOrEmpty(strArry[i]))
                    {
                        result[i] = 0;
                    }
                    else
                    {
                        try
                        {
                            result[i] = float.Parse(strArry[i]);
                        }
                        catch (Exception e)
                        {
                            result[i] = 0;

                            Debug.LogError($"float.Parse 转换  {strArry[i]} 错误,已初始化为默认值: {e}");
                        }
                    }
                }
                if (isList)
                {
                    return result.ToList();
                }
                else
                {
                    return result;

                }
                //return isList ? result.ToList() : result;

            }
            else if (type.ToLower().Contains("double"))
            {
                double[] result = new double[lgh];
                for (int i = 0; i < lgh; i++)
                {
                    if (string.IsNullOrEmpty(strArry[i]))
                    {
                        result[i] = 0;
                    }
                    else
                    {
                        try
                        {
                            result[i] = double.Parse(strArry[i]);
                        }
                        catch (Exception e)
                        {
                            result[i] = 0;

                            Debug.LogError($"double.Parse 转换  {strArry[i]} 错误,已初始化为默认值: {e}");
                        }
                    }
                }
                if (isList)
                {
                    return result.ToList();
                }
                else
                {
                    return result;

                }
                //return isList ? result.ToList() : result;

            }
            else if (type.ToLower().Contains("bool"))
            {
                bool[] result = new bool[lgh];
                for (int i = 0; i < lgh; i++)
                {
                    if (string.IsNullOrEmpty(strArry[i]))
                    {
                        result[i] = false;
                    }
                    else
                    {
                        try
                        {
                            result[i] = bool.Parse(strArry[i]);
                        }
                        catch (Exception e)
                        {
                            result[i] = false;

                            Debug.LogError($"bool.Parse 转换  {strArry[i]} 错误,已初始化为默认值: {e}");
                        }
                    }
                }
                if (isList)
                {
                    return result.ToList();
                }
                else
                {
                    return result;

                }
                //return isList ? result.ToList() : result;

            }
            else if (type.ToLower().Contains("string"))
            {
                if (isList)
                {
                    return strArry.ToList();
                }
                else
                {
                    return strArry;

                }
                //	return isList ? strArry.ToList() : strArry;
            }
            return null;
        }


    }
    public partial class UnityEditorTools
    {
        [MenuItem("UnityFramework/Excel表格转Json")]
        public static void CreateExcelToJsonWindows()
        {
            ExcelToJson window = EditorWindow.GetWindow<ExcelToJson>();//获取指定类型的窗口.
            window.titleContent = new GUIContent("Excel表格转Json");
            window.maxSize = new Vector2(500, 600);
            window.minSize = new Vector2(500, 600);
            window.Show();
            if (string.IsNullOrEmpty(ExcelToJson.entitySaveDir))
            {
                ExcelToJson.entitySaveDir = string.Concat(Application.dataPath, "/Scripts/EntityClass");

            }
            if (string.IsNullOrEmpty(ExcelToJson.excelPath))
            {
                ExcelToJson.excelPath = string.Concat(Application.dataPath, "/ExcelData");

            }
            if (string.IsNullOrEmpty(ExcelToJson.excelDir))
            {
                ExcelToJson.excelDir = string.Concat(Application.dataPath, "/ExcelData");

            }
            if (string.IsNullOrEmpty(ExcelToJson.jsonSavePath))
            {
                ExcelToJson.jsonSavePath = string.Concat(Application.streamingAssetsPath, "/Data");

            }


            ClearConsoleLog();

        }
        static void ClearConsoleLog()
        {
            System.Type log = typeof(EditorWindow).Assembly.GetType("UnityEditor.LogEntries");
            System.Reflection.MethodInfo clearMethod = log.GetMethod("Clear");
            clearMethod.Invoke(null, null);
        }
    }

}