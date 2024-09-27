/*
* FileName:          GameEntry
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityFramework.Runtime;

public class GameEntry : MonoBehaviour
{
    private void Awake()
    {
        ComponentEntry.OnInitFinish += delegate
        {
            Init();
        };
    }
    void Init()
    {
        Global.Network.NetwokingID = ComponentEntry.Config.GetConfig("NetwokingID");
        Global.Network.Authentication = ComponentEntry.Config.GetConfig("Authentication");




        if (Global.Network.NetwokingID == "0")
        {
            Global.Network.BGIP = ComponentEntry.Config.GetConfig("BGIP_Local");
            Global.Network.IP_Local = ComponentEntry.Config.GetConfig("IP_Local");
            Global.Network.ManagerIP = ComponentEntry.Config.GetConfig("ManagerIP_Local");


        }
        else
        {
            Global.Network.BGIP = ComponentEntry.Config.GetConfig("BGIP");
            Global.Network.IP_Local = ComponentEntry.Config.GetConfig("IP");
            Global.Network.ManagerIP = ComponentEntry.Config.GetConfig("ManagerIP");

        }

        Global.Network.ExternalLink1 = Global.Network.IP_Local + ComponentEntry.Config.GetConfig("ExternalLink1");
        Global.Network.ExternalLink2 = Global.Network.IP_Local + ComponentEntry.Config.GetConfig("ExternalLink2");
        Global.Network.ExternalLink3 = Global.Network.IP_Local + ComponentEntry.Config.GetConfig("ExternalLink3");

        Global.Network.LoginUrl = Global.Network.BGIP + ComponentEntry.Config.GetConfig("Login");
        Global.Network.ChangePasswordUrl = Global.Network.BGIP + ComponentEntry.Config.GetConfig("ChangePassword");
        Global.Network.EnrollUrl = Global.Network.BGIP + ComponentEntry.Config.GetConfig("Enroll");

        Global.Setting.ShowLogo = bool.Parse(ComponentEntry.Config.GetConfig("ShowLogo"));
        Global.Setting.TimeLock = bool.Parse(ComponentEntry.Config.GetConfig("TimeLock"));


        //-----------------------------------



        Log.Info($"模式:{Global.Network.NetwokingID}");
        Log.Info($"本机IP:{Global.Network.IP_Local}");
        Log.Info($"后台IP:{Global.Network.BGIP}");

        ////-----------------
        ///
   //     ComponentEntry.Task.StartTask(0);

    }


}
