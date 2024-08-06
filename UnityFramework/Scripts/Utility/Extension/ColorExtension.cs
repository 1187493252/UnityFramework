/*
* FileName:          ColorExtension
* CompanyName:       
* Author:            relly
* Description:       
* 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityFramework
{
    public static class ColorExtension
    {
        public static Color SetColorA(this Color T, float a)
        {
            Color color = new Color(T.r, T.g, T.b, a);
            return color;
        }
        public static Color SetColorR(this Color T, float r)
        {
            Color color = new Color(r, T.g, T.b, T.a);
            return color;
        }
        public static Color SetColorG(this Color T, float g)
        {
            Color color = new Color(T.r, g, T.b, T.a);
            return color;
        }
        public static Color SetColorB(this Color T, float b)
        {
            Color color = new Color(T.r, T.g, b, T.a);
            return color;
        }
    }
}