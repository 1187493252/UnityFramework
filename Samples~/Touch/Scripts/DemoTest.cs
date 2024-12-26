using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoTest : MonoBehaviour
{
    public TouchRaycast touchRaycast;
    public TouchCtrl touchCtrl;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        touchRaycast.OnTouchedObjectSelect += delegate (Transform tmp)
        {
            text.text = $"{tmp.name}";
        };
        touchRaycast.OnTouchedObjectUnSelect += delegate ()
        {
            text.text = $"";
        };
    }



}
