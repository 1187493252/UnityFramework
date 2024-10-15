using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityFramework.Runtime;

public class Example_Test : MonoBehaviour
{
    public InputField InputField;
    public Button Button;
    // Start is called before the first frame update
    void Start()
    {



        Button.onClick.AddListener(delegate ()
        {

            OpenUrlWindow(InputField.text);
        });




    }

    public void OpenUrlWindow(string url)
    {
        Application.ExternalCall("OpenUrl_Window", url);

    }
    public void CloseUrl_Window()
    {
        Application.ExternalCall("CloseUrl_Window");

    }
}
