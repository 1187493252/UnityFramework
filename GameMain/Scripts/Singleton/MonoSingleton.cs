using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityFramework
{
	public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
	{
		private static T instance;
		private static object syncRoot = new Object();
		public bool IsSetParent;
		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						T instances = FindObjectOfType<T>();
						if (instances != null)
						{
							instance = instances;
						}
						else
						{
							GameObject go = new GameObject();
							go.name = typeof(T).Name;
							instance = go.AddComponent<T>();
						}
					}
				}
				return instance;
			}
		}
		protected virtual void Awake()
		{
			if (instance != null && instance.gameObject != this.gameObject)
			{
				Debug.LogError($"{GetType().Name}为单例对象,不允许存在多个,已删除对象{gameObject.name}");
				Destroy(this.gameObject);
			}
			else
			{
				instance = (T)this;
				if (IsSetParent)
				{
					GameObject UnityFramework = GameObject.Find("Framework");
					if (!UnityFramework)
					{
						UnityFramework = new GameObject("Framework");
					}
					transform.SetParent(UnityFramework.transform);
				}

			}
		}
	}
}


