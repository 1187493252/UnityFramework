using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FlexFramework.Excel;
using LitJson;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UnityEngine;
using UnityFramework;

/// <summary>
/// xml 解析助手
/// </summary>
public static class ExcelHelper
{

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
                    System.Type type = ExcelHelper.GetTypeByString(typestring);
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
                        object value = ExcelHelper.GetValueByString(typestring, content);
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

    public static List<DataTable> GetDataTablesFromExcel(byte[] data)
    {
        List<DataTable> dataTableList = new List<DataTable>();

        var book = new WorkBook(data);

        foreach (var item in book)
        {
            WorkSheet sheet = item;
            if (sheet == null)
            {
                continue;
            }
            DataTable dataTable = new DataTable();
            List<Row> rowData = (List<Row>)sheet.Rows;
            dataTable.TableName = sheet.Name;
            int rowsCount = rowData.Count;//获取sheet的最大行数
            int colsCount = rowData[0].Count;
            //给dataTable添加列数
            for (int i = 0; i < colsCount; i++)
            {
                var cellValue = rowData[0][i];
                dataTable.Columns.Add(cellValue?.ToString());
            }
            //给每行每列添加数据
            for (int x = 0; x < rowsCount; x++)
            {
                DataRow dr = dataTable.NewRow();
                for (int y = 0; y < colsCount; y++)
                {
                    try
                    {
                        var cellValue = rowData[x][y];
                        dr[y] = cellValue.Text;
                    }
                    catch (Exception)
                    {
                        Debug.LogError($"表格:{dataTable.TableName}:{x}行{y}列读取错误");
                        throw;
                    }
                }
                dataTable.Rows.Add(dr);
            }
            dataTableList.Add(dataTable);
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
            Debug.Log($"基础类型转换提示,{typeName}可能为数组/列表: {e}");
        }
        return result;
    }


    /// <summary>
    /// 获取表格中type为数组/链表的值,读表专用
    /// </summary>
    /// <param name="type">表格中填的数组/链表 float[],List<string></param>
    /// <param name="value">表格中填的值</param>
    /// <returns></returns>
    public static object GetValueByString(string type, string value, char delimiter = '_')
    {
        bool isList = false;
        if (type.ToUpperFirst().Contains("List"))
        {
            isList = true;
        }
        string[] strArry = value.Split(delimiter);
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

