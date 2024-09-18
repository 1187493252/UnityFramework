using System;
using UnityEngine;

namespace UnityFramework
{
	/// <summary>
	/// 委托操作类
	/// </summary>
	public class Delegation
	{

		/// <summary>
		/// 射线进入
		/// </summary>
		public static DelegateObserver<Transform> Event_RayEnter;

		/// <summary>
		/// 点击物体
		/// </summary>
		public static DelegateObserver<Transform> Event_ClickItem;

		/// <summary>
		/// 射线离开
		/// </summary>
		public static DelegateObserver<Transform> Event_RayExit;

		/// <summary>
		/// 开始触碰
		/// </summary>
		public static DelegateObserver<Transform> Event_TouchStart;

		/// <summary>
		/// 开始拾取
		/// </summary>
		public static DelegateObserver<Transform, Transform> Event_GrabStart;

		/// <summary>
		/// 结束拾取
		/// </summary>
		public static DelegateObserver<Transform> Event_GrabEnd;

		#region[Delegate]

		/// <summary>
		/// 委托 无参数
		/// </summary>
		public delegate void DelegateObserver();

		/// <summary>
		/// 委托 1参数
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="arg1"></param>
		public delegate void DelegateObserver<T>(T arg1);

		/// <summary>
		/// 委托 2参数
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="X"></typeparam>
		/// <param name="arg1"></param>
		/// <param name="arg2"></param>
		public delegate void DelegateObserver<T, X>(T arg1, X arg2);

		/// <summary>
		/// 委托 3参数
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="X"></typeparam>
		/// <typeparam name="Y"></typeparam>
		/// <param name="arg1"></param>
		/// <param name="arg2"></param>
		/// <param name="arg3"></param>
		public delegate void DelegateObserver<T, X, Y>(T arg1, X arg2, Y arg3);

		/// <summary>
		/// 委托 4参数
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="X"></typeparam>
		/// <typeparam name="Y"></typeparam>
		/// <typeparam name="Z"></typeparam>
		/// <param name="arg1"></param>
		/// <param name="arg2"></param>
		/// <param name="arg3"></param>
		/// <param name="arg4"></param>
		public delegate void DelegateObserver<T, X, Y, Z>(T arg1, X arg2, Y arg3, Z arg4);

		/// <summary>
		/// 委托 5参数
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="X"></typeparam>
		/// <typeparam name="Y"></typeparam>
		/// <typeparam name="Z"></typeparam>
		/// <typeparam name="A"></typeparam>
		/// <param name="arg1"></param>
		/// <param name="arg2"></param>
		/// <param name="arg3"></param>
		/// <param name="arg4"></param>
		/// <param name="arg5"></param>
		public delegate void DelegateObserver<T, X, Y, Z, A>(T arg1, X arg2, Y arg3, Z arg4, A arg5);

		#endregion

		#region[Add delegate event]

		/// <summary>
		/// 无参数 广播
		/// </summary>
		/// <param name="_event"></param>
		public static void Broadcast(DelegateObserver _event)
		{
			if (_event != null)
			{
				_event();
			}
		}

		/// <summary>
		/// 广播 一个参数
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="_event"></param>
		/// <param name="arg"></param>
		public static void Broadcast<T>(DelegateObserver<T> _event, T arg)
		{
			if (_event != null)
			{
				_event(arg);
			}
		}

		/// <summary>
		/// 广播 两个参数
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="X"></typeparam>
		/// <param name="_event"></param>
		/// <param name="arg1"></param>
		/// <param name="arg2"></param>
		public static void Broadcast<T, X>(DelegateObserver<T, X> _event, T arg1, X arg2)
		{
			if (_event != null)
			{
				_event(arg1, arg2);
			}
		}

		/// <summary>
		/// 广播 三个参数
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="X"></typeparam>
		/// <typeparam name="Y"></typeparam>
		/// <param name="_event"></param>
		/// <param name="arg1"></param>
		/// <param name="arg2"></param>
		/// <param name="arg3"></param>
		public static void Broadcast<T, X, Y>(DelegateObserver<T, X, Y> _event, T arg1, X arg2, Y arg3)
		{
			if (_event != null)
			{
				_event(arg1, arg2, arg3);
			}
		}

		/// <summary>
		/// 广播 四个参数
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="X"></typeparam>
		/// <typeparam name="Y"></typeparam>
		/// <typeparam name="Z"></typeparam>
		/// <param name="_event"></param>
		/// <param name="arg1"></param>
		/// <param name="arg2"></param>
		/// <param name="arg3"></param>
		/// <param name="arg4"></param>
		public static void Broadcast<T, X, Y, Z>(DelegateObserver<T, X, Y, Z> _event, T arg1, X arg2, Y arg3, Z arg4)
		{
			if (_event != null)
			{
				_event(arg1, arg2, arg3, arg4);
			}
		}

		/// <summary>
		/// 广播 五个参数
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="X"></typeparam>
		/// <typeparam name="Y"></typeparam>
		/// <typeparam name="Z"></typeparam>
		/// <typeparam name="A"></typeparam>
		/// <param name="_event"></param>
		/// <param name="arg1"></param>
		/// <param name="arg2"></param>
		/// <param name="arg3"></param>
		/// <param name="arg4"></param>
		/// <param name="arg5"></param>
		public static void Broadcast<T, X, Y, Z, A>(DelegateObserver<T, X, Y, Z, A> _event, T arg1, X arg2, Y arg3, Z arg4, A arg5)
		{
			if (_event != null)
			{
				_event(arg1, arg2, arg3, arg4, arg5);
			}
		}

		#endregion
	}
}

