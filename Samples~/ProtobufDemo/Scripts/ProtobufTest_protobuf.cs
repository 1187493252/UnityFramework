/*
* FileName:          ProtobufTest_protobuf
* CompanyName:       
* Author:            
* Description:       
*/

using Google.Protobuf;
using Person;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class ProtobufTest_protobuf : MonoBehaviour
{
    private void Start()
    {
        OnePerson onePerson = new OnePerson();
        onePerson.Name = "张三";
        onePerson.IdNumber = 000001;
        onePerson.Gender = genders.Man;
        onePerson.Profession = "法外狂徒";
        //序列化 将onePerson对象转换为字节数组
        byte[] dataByte = onePerson.ToByteArray();

        //反序列化 将字节数组转换为OnePerson对象
        OnePerson mySelf = new OnePerson();
        mySelf = OnePerson.Parser.ParseFrom(dataByte);
        //打印输出
        Debug.Log($"My name is:{mySelf.Name}");
        Debug.Log($"My idNumber is:{mySelf.IdNumber}");
        Debug.Log($"My gender is:{mySelf.Gender}");
        Debug.Log($"My profession is:{mySelf.Profession}");

    }
}
