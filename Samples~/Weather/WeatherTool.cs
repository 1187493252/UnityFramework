/*
* FileName:          WeatherTool
* CompanyName:       
* Author:            relly
* Description:       
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityFramework;
using UnityFramework.Runtime;

[Serializable]
public class WeatherSprite
{
    public string name;
    public Sprite icon;
    public Sprite bg;
}

public class WeatherTool : MonoBehaviour
{
    public Transform CityParent;
    public Transform CityBtn;

    public Transform WeatherParent;

    public ScrollRect WeatherScrollView;
    public List<string> CityName = new List<string>();

    Dictionary<string, string> cityCode = new Dictionary<string, string>();
    Dictionary<string, DateTime> cityLastTime = new Dictionary<string, DateTime>();

    Dictionary<string, WeathBody> cityWeather = new Dictionary<string, WeathBody>();

    const string url = @"http://t.weather.itboy.net/api/weather/city/";
    public List<WeatherSprite> SpriteWeatherList = new List<WeatherSprite>();
    public Dictionary<string, WeatherSprite> spriteWeatherDic = new Dictionary<string, WeatherSprite>();

    public double IntervalSeconds = 7200;//时间间隔,单位秒
    DateTime last;
    private void Awake()
    {
        if (ComponentEntry.Data == null)
        {
            ComponentEntry.OnInitFinish += delegate
            {
                Init();
            };
        }
        else
        {
            Init();
        }

    }


    void Init()
    {
        List<CityCode> lsit = ComponentEntry.Data.GetDataByTableName<List<CityCode>>("CityCode");
        foreach (var item in lsit)
        {
            if (!item.city_code.IsNullOrEmpty())
            {
                cityCode.Add(item.city_name, item.city_code);
            }
        }

        foreach (var item in SpriteWeatherList)
        {
            spriteWeatherDic.Add(item.name, item);
        }
        List<GameObject> tmp = new List<GameObject>();
        foreach (Transform item in CityParent)
        {
            tmp.Add(item.gameObject);
        }

        for (int i = 0; i < CityName.Count; i++)
        {
            Transform transform = Instantiate(CityBtn);
            transform.parent = CityParent;
            transform.localScale = Vector3.one;
            transform.name = CityName[i];
            transform.GetComponentInChildren<Text>().text = CityName[i];
            SetBtnStateOnly setBtnSelectedState = transform.GetComponentInChildren<SetBtnStateOnly>();
            setBtnSelectedState.root = CityParent;
            if (i == 0)
            {
                setBtnSelectedState.IsStartSelected = true;
            }
            setBtnSelectedState.SelectedEvent.AddListener(delegate
            {

                GetWeather(transform.name);
            });
        }

        foreach (var item in tmp)
        {
            Destroy(item);
        }



    }
    public void ClickCityBtn(GameObject cityName)
    {
        GetWeather(cityName.name);

    }
    void UpdateWeatherUI(WeathBody weathBody)
    {
        if (weathBody == null)
        {
            return;
        }
        for (int i = 0; i < WeatherParent.childCount; i++)
        {
            Transform transform = WeatherParent.GetChild(i);
            WeathDetailData data = weathBody.data.forecast[i];
            WeatherSprite weatherSprite;

            weatherSprite = GetWeatherSprite(data.type);

            transform.GetComponent<Image>().sprite = weatherSprite.bg;

            transform.Find("日期").GetComponent<Text>().text = data.ymd;
            transform.Find("天气图标").GetComponent<Image>().sprite = weatherSprite.icon;
            transform.Find("天气").GetComponent<Text>().text = data.type;
            transform.Find("温度").GetComponent<Text>().text = $"{data.high.Split(' ')[1]}/{data.low.Split(' ')[1]}";
            transform.Find("风向").GetComponent<Text>().text = data.fx;
            transform.Find("风速等级").GetComponent<Text>().text = data.fl;
        }
    }


    WeatherSprite GetWeatherSprite(string content)
    {
        WeatherSprite weatherSprite = null;
        try
        {
            weatherSprite = spriteWeatherDic[content];
        }
        catch (Exception)
        {

            Debug.LogError(content);
        }
        if (weatherSprite == null)
        {
            foreach (var item in spriteWeatherDic)
            {
                if (item.Key.Contains(content.Substring(content.Length - 1, 1)))
                {
                    weatherSprite = item.Value;
                    break;
                }
            }
        }

        return weatherSprite;
    }

    public WeathBody GetWeather(string cityName)
    {
        WeathBody weathBody = null;
        if (cityLastTime.TryGetValue(cityName, out last))
        {
            TimeSpan timeSpan = DateTime.Now - last;
            if (timeSpan.TotalSeconds >= IntervalSeconds)
            {
                //重新获取天气
                string cityKey = cityCode.GetValue(cityName);
                if (cityKey == "")
                {
                    return null;
                }
                ComponentEntry.WebRequest.RequestGet(url + cityKey, callBack: (string content, byte[] data) =>
                {
                    weathBody = Seriallize.JsonDeserialization<WeathBody>(content);
                    cityLastTime[cityName] = DateTime.Now;
                    cityWeather[cityName] = weathBody;
                    UpdateWeatherUI(weathBody);

                });
            }
            else
            {
                //使用上一次的记录
                weathBody = cityWeather[cityName];
                UpdateWeatherUI(weathBody);

            }
        }
        else
        {
            //重新获取天气
            string cityKey = cityCode.GetValue(cityName);
            if (cityKey == "")
            {
                return null;
            }
            ComponentEntry.WebRequest.RequestGet(url + cityKey, callBack: (string content, byte[] data) =>
            {
                weathBody = Seriallize.JsonDeserialization<WeathBody>(content);
                cityLastTime[cityName] = DateTime.Now;
                cityWeather[cityName] = weathBody;
                UpdateWeatherUI(weathBody);

            });
        }


        return weathBody;
    }




}

public class CityCode
{
    public int id { get; set; }
    public int pid { get; set; }
    public string city_code { get; set; }
    public string city_name { get; set; }
    public string post_code { get; set; }
    public string area_code { get; set; }
    public string ctime { get; set; }
}
public class WeathBody
{
    public string message;
    public int status;
    public string date;
    public string time;
    public CityInfo cityInfo;
    public WeathData data;
}

public class CityInfo
{
    public string city;
    public string cityKey;
    public string parent;
    public string updateTime;
}

public class WeathData
{
    public string shidu;//湿度
    public double pm25;//pm2.5
    public double pm10;//pm10
    public string quality;//空气质量
    public string wendu;//温度
    public string ganmao;//感冒提醒
    public WeathDetailData yesterday;//昨日天气
    public WeathDetailData[] forecast;//今日+未来天气
}

public class WeathDetailData
{
    public string date;//日期 
    public string high;//最高温
    public string low;//最低温
    public string ymd;//完整日期 2023-04-14
    public string week;//星期
    public string sunrise;//日出
    public string sunset;//日落
    public double aqi;//空气质量指数
    public string fx;//风向
    public string fl;//风速级别
    public string type;//天气
    public string notice;//提示
}


