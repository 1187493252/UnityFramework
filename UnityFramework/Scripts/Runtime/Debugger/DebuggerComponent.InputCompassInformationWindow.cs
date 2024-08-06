/*
* FileName:          DebuggerComponent
* CompanyName:       
* Author:            relly
* Description:       
*/

using Framework;
using Framework.Debugger;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityFramework.Runtime
{

    public sealed partial class DebuggerComponent : UnityFrameworkComponent
    {
        private sealed class InputCompassInformationWindow : ScrollableDebuggerWindowBase
        {
            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Input Compass Information</b>");
                GUILayout.BeginVertical("box");
                {
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Enable", GUILayout.Height(30f)))
                        {
                            Input.compass.enabled = true;
                        }
                        if (GUILayout.Button("Disable", GUILayout.Height(30f)))
                        {
                            Input.compass.enabled = false;
                        }
                    }
                    GUILayout.EndHorizontal();

                    DrawItem("Enabled", Input.compass.enabled.ToString());
                    if (Input.compass.enabled)
                    {
                        DrawItem("Heading Accuracy", Input.compass.headingAccuracy.ToString());
                        DrawItem("Magnetic Heading", Input.compass.magneticHeading.ToString());
                        DrawItem("Raw Vector", Input.compass.rawVector.ToString());
                        DrawItem("Timestamp", Input.compass.timestamp.ToString());
                        DrawItem("True Heading", Input.compass.trueHeading.ToString());
                    }
                }
                GUILayout.EndVertical();
            }
        }


    }
}
