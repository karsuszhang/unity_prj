using UnityEngine;
using System.Collections;

namespace CommonUtil
{
	public delegate bool TaskFunc();
	public delegate IEnumerable CoroutineTask();
	
	public class TaskMng : MonoBehaviour
	{
		#region static
		static TaskMng _instance;
		public static TaskMng Instance
		{
			get {
				if (_instance == null) {
					GameObject go = new GameObject ("CoroutineObject");
					GameObject.DontDestroyOnLoad (go);
					_instance = go.AddComponent<TaskMng> ();
				}
				return _instance;
			}
		}
		#endregion

		private TaskMng()
		{
			
		}

		void Update()
		{
		}
	}
}