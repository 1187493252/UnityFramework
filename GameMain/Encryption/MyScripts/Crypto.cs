using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MachineOperation;

public class Crypto : MonoBehaviour
{

	private const string mccodeName = "/mccode.txt";                    //机器码文件名
	private const string vercodeName = "/vercode.txt";                  //授权码文件名

	public Text jiQiMa;                                                 //机器码输入框
	public InputField shouQuanMa;                                       //授权码输入框

	private string jiQimaText;                                          //机器码文本
	private string shouQuanMaText;                                      //授权码文本

	public Text hintText;                                               //提示文本框

	string appPathNow;
	public string sceneName;

	/// <summary>
	/// 生成机器码
	/// </summary>
	public void CreateMachineCode(string _fileName, string _content)
	{
		//声明字符串
		StringBuilder sb = new StringBuilder();
		//添加字符串内容
		sb.Append(_content);
		//创建机器码文件 如果文件存在则覆盖重新创建
		FileStream fs = new FileStream(appPathNow + _fileName, FileMode.Create);
		//转换成二进制存储
		byte[] bytes = new UTF8Encoding().GetBytes(sb.ToString());
		//将二进制内容写入文件
		fs.Write(bytes, 0, bytes.Length);

		fs.Close();

		Debug.Log("创建成功");
	}

	/// <summary>
	/// 读取文件
	/// </summary>
	public string Load(string fileName)
	{
		//如果指定路劲内有指定文件 则继续执行
		if (File.Exists(appPathNow + fileName))
		{
			//打开指定路径下的指定文件
			FileStream fs = new FileStream(appPathNow + fileName, FileMode.Open);
			//声明二进制数组
			byte[] bytes = new byte[64];
			//读取文件
			fs.Read(bytes, 0, bytes.Length);
			//将二进制文件转化为字符串
			string s = new UTF8Encoding().GetString(bytes);
			return s;
		}
		//如果指定路径内无指定文件 返回空
		else
		{
			Debug.Log("找不到文件");
			return " ";
		}
	}

	private void Awake()
	{
		appPathNow = System.Environment.CurrentDirectory;
		Debug.Log(appPathNow);
		if (File.Exists(appPathNow + vercodeName))
		{
			//如果授权码文件存在，对比授权码和机器码
			if (Encryption.CompareMathineCode(SystemInfo.deviceUniqueIdentifier, Load(vercodeName)))
			{
				//机器码和授权码对应 直接进去游戏
				SceneManager.LoadScene(sceneName);
				return;
			}
		}
		else
		{
			gameObject.SetActive(true);
		}
	}

	private void Start()
	{
		jiQiMa.text = SystemInfo.deviceUniqueIdentifier;

	}

	/// <summary>
	/// 获取路径
	/// </summary>
	/// <returns></returns>
	public static string GetPath()
	{
#if UNITY_EDITOR
		return Application.dataPath;
#elif UNITY_STANDALONE_WIN
        return Application.dataPath + "/StreamingAssets/";
#elif UNITY_ANDROID
            return "jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IPHONE
            return Application.dataPath + "/Raw/";
#else
        return "";
#endif
	}

	/// <summary>
	/// 登录事件
	/// </summary>
	public void SignEvent()
	{
		shouQuanMaText = shouQuanMa.textComponent.text;                 //获取授权码文本

		if (Encryption.CompareMathineCode(SystemInfo.deviceUniqueIdentifier, shouQuanMaText))
		{
			Debug.Log("机器码:" + jiQimaText);
			Debug.Log("授权码" + shouQuanMaText);
			Debug.Log("验证成功,准备进入主场景");
			//创建授权码文件
			CreateMachineCode(vercodeName, shouQuanMaText);
			SceneManager.LoadScene(sceneName);
		}
		else
		{
			hintText.text = "机器码或授权码输入错误";
			Invoke("DelayHideHintText", 0.4f);
		}
	}

	/// <summary>
	/// 延迟隐藏文本
	/// </summary>
	private void DelayHideHintText()
	{
		hintText.text = "";
	}

	/// <summary>
	/// 拷贝机器码
	/// </summary>
	public void CopyText()
	{
		TextEditor te = new TextEditor();
		te.text = new GUIContent(SystemInfo.deviceUniqueIdentifier).text;
		te.OnFocus();
		te.Copy();
	}
}
