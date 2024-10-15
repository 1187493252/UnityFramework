/*
* FileName:          ExcelTool
* CompanyName:       
* Author:            relly
* Description:       
*/
#if UNITY_EDITOR

using LitJson;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace UnityFramework.Editor
{
    public class ExcelTool
    {
        static string folder = "/Data/";          //路径文件夹

        [MenuItem("Assets/ExcelTool/ExcelToJson（Excel转Json）", false, 0)]
        static void ExcelToJson()
        {
            string directoryPath = Application.streamingAssetsPath + folder;
            Dictionary<string, string> excelPathDic = new Dictionary<string, string>();
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                string path = AssetDatabase.GetAssetPath(obj);
                if (Path.GetExtension(path) == ".xlsx")
                {
                    excelPathDic.Add(path, "xlsx");
                }
                else if (Path.GetExtension(path) == ".xls")
                {
                    excelPathDic.Add(path, "xls");
                }
            }
            foreach (var item in excelPathDic)
            {
                string _name = Path.GetFileNameWithoutExtension(item.Key);

                List<DataTable> dataTableList = GetDataTablesFromExcel(item.Key);
                string data = DataTableToJson(dataTableList);
                string path = $"{directoryPath}{_name}.json";
                SaveJsonData(data, path);
                Debug.Log($"Excel转化Json成功:{path}");
            }

            AssetDatabase.Refresh();
        }


        [MenuItem("Assets/ExcelTool/ExcelToJson（Excel转Json Base64字符串）", false, 1)]
        static void ExcelJsonDataToBase64String()
        {
            string directoryPath = Application.streamingAssetsPath + folder;
            Dictionary<string, string> excelPathDic = new Dictionary<string, string>();
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                string path = AssetDatabase.GetAssetPath(obj);
                if (Path.GetExtension(path) == ".xlsx")
                {
                    excelPathDic.Add(path, "xlsx");
                }
                else if (Path.GetExtension(path) == ".xls")
                {
                    excelPathDic.Add(path, "xls");
                }
            }
            foreach (var item in excelPathDic)
            {
                string _name = Path.GetFileNameWithoutExtension(item.Key);

                List<DataTable> dataTableList = GetDataTablesFromExcel(item.Key);
                string data = DataTableToJson(dataTableList);
                string path = $"{directoryPath}{_name}.base";
                SaveJsonDataToBase64String(data, path);
                Debug.Log($"Excel转化Json Base64字符串成功:{path}");
            }

            AssetDatabase.Refresh();
        }

        [MenuItem("Assets/ExcelTool/ExcelToJson（Excel转Json 二进制字符串）", false, 2)]
        static void ExcelJsonDataToBinary()
        {
            string directoryPath = Application.streamingAssetsPath + folder;
            Dictionary<string, string> excelPathDic = new Dictionary<string, string>();
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                string path = AssetDatabase.GetAssetPath(obj);
                if (Path.GetExtension(path) == ".xlsx")
                {
                    excelPathDic.Add(path, "xlsx");
                }
                else if (Path.GetExtension(path) == ".xls")
                {
                    excelPathDic.Add(path, "xls");
                }
            }
            foreach (var item in excelPathDic)
            {
                string _name = Path.GetFileNameWithoutExtension(item.Key);

                List<DataTable> dataTableList = GetDataTablesFromExcel(item.Key);
                string data = DataTableToJson(dataTableList);
                string path = $"{directoryPath}{_name}.sc";
                SaveJsonDataToBinary(data, path);
                Debug.Log($"Excel转化Json 二进制字符串成功:{path}");
            }

            AssetDatabase.Refresh();
        }

        public static void SaveJsonData(string data, string path, string securityCode)
        {
            string content = EncryptionUtil.Encrypt(data, securityCode);
            SaveJsonData(content, path);
        }
        public static void SaveJsonData(string data, string path)
        {

            File.WriteAllText(path, data);


        }
        public static void SaveJsonDataToBinary(string data, string path)
        {
            byte[] unicodeData = Encoding.Unicode.GetBytes(data);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.Create(path);
            binaryFormatter.Serialize(fileStream, unicodeData);
            fileStream.Close();
        }
        public static void SaveJsonDataToBase64String(string data, string path)
        {
            byte[] datas = Encoding.Unicode.GetBytes(data);
            string dataBase64 = Convert.ToBase64String(datas);
            StreamWriter swData = new StreamWriter(path);
            swData.Write(dataBase64);
            swData.Close();
        }
        public static string DataTableToJson(List<DataTable> dataTableList)
        {
            string jsonData = "";
            Dictionary<string, List<Dictionary<string, object>>> excelDataDic = new Dictionary<string, List<Dictionary<string, object>>>();

            excelDataDic.Clear();
            foreach (var item in dataTableList)
            {
                // 准备一个列表存储整个表的数据
                //count是行数 string列名 object数据
                List<Dictionary<string, object>> tableDataList = new List<Dictionary<string, object>>();
                tableDataList.Clear();
                bool hasData = false;

                // 读取数据
                // 第一行列名
                // 第二行类型
                // 第三行备注
                // 第四行开始
                for (int i = 3; i < item.Rows.Count; i++)
                {
                    // 准备一个字典存储每一行的数据
                    Dictionary<string, object> rowData = new Dictionary<string, object>();
                    for (int j = 0; j < item.Columns.Count; j++)
                    {
                        //如果该列列名为空则进入下一个行
                        if (string.IsNullOrEmpty(item.Rows[0][j].ToString()))
                        {
                            break;
                        }
                        // 如果该列列名以!开始则忽略进入下一列
                        if (item.Rows[0][j].ToString().StartsWith("!"))
                        {
                            continue;
                        }

                        //读取第一行数据作为字段
                        string field = item.Rows[0][j].ToString();
                        //读取第二行数据转换成对应Type
                        string typestring = item.Rows[1][j].ToString();
                        System.Type type = GetTypeByString(typestring);
                        string content = item.Rows[i][j].ToString();

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
                                    Debug.LogError($"表格{item.TableName}第{i}行第{j}列,转换数据:{content}to{type} 错误: {e}");
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
                            //type = null content可能是数组/链表
                            object value = GetValueByString(typestring, content);
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
                    excelDataDic.Add(item.TableName, tableDataList);
                }
            }
            jsonData = JsonMapper.ToJson(excelDataDic);
            // jsonData = JsonConvert.SerializeObject(excelDataDic);
            //转换字符串中的任何转义字符
            jsonData = Regex.Unescape(jsonData);

            return jsonData;
        }


        public static List<DataTable> GetDataTablesFromExcel(string xlsxFile)
        {
            List<DataTable> dataTableList = new List<DataTable>();
            FileInfo fileInfo = new FileInfo(xlsxFile);

            using (FileStream fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                IWorkbook workbook;
                if (Path.GetExtension(xlsxFile) == ".xlsx")
                {
                    workbook = new XSSFWorkbook(fileInfo);
                }
                else
                {
                    workbook = new HSSFWorkbook(fileStream);
                }

                for (int index = 0; index < workbook.NumberOfSheets; index++)
                {
                    DataTable dataTable = new DataTable();
                    ISheet sheet = workbook.GetSheetAt(index);
                    if (sheet == null)
                    {
                        continue;
                    }
                    dataTable.TableName = sheet.SheetName;
                    int rowsCount = sheet.PhysicalNumberOfRows;//获取sheet的最大行数


                    int colsCount = sheet.GetRow(0).PhysicalNumberOfCells;

                    //给dataTable添加列数
                    for (int i = 0; i < colsCount; i++)
                    {
                        var cellValue = sheet.GetRow(0).GetCell(i);
                        dataTable.Columns.Add(cellValue?.ToString());
                    }

                    //给每行每列添加数据
                    for (int x = 0; x < rowsCount; x++)
                    {
                        DataRow dr = dataTable.NewRow();
                        for (int y = 0; y < colsCount; y++)
                        {
                            var cellValue = sheet.GetRow(x).GetCell(y);
                            dr[y] = cellValue?.ToString();
                        }
                        dataTable.Rows.Add(dr);
                    }

                    dataTableList.Add(dataTable);
                }
            }
            return dataTableList;
        }


        static string GetTypeNameByString(string typeName)
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
        /// <summary>
        /// 获取表格中填的type转换为代码对应类型,数组/链表除外
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static System.Type GetTypeByString(string typeName)
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
        /// 获取表格中type为数组/链表的值,读表专用
        /// </summary>
        /// <param name="type">表格中填的数组/链表 float[],List<string></param>
        /// <param name="value">表格中填的值</param>
        /// <returns></returns>
        public static object GetValueByString(string type, string value)
        {
            bool isList = false;
            if (type.ToUpperFirst().Contains("List"))
            {
                isList = true;
            }
            string[] strArry = value.Split('_');
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
}
#endif