/*
* FileName:          ExcelTool
* CompanyName:       
* Author:            relly
* Description:       
*/
#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
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

                List<DataTable> dataTableList = ExcelHelper.GetDataTablesFromExcel(item.Key);
                string data = ExcelHelper.DataTableToJson(dataTableList);
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

                List<DataTable> dataTableList = ExcelHelper.GetDataTablesFromExcel(item.Key);
                string data = ExcelHelper.DataTableToJson(dataTableList);
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

                List<DataTable> dataTableList = ExcelHelper.GetDataTablesFromExcel(item.Key);
                string data = ExcelHelper.DataTableToJson(dataTableList);
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





    }
}
#endif