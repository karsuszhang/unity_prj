using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CommonUtil
{
	public enum LogType
	{
		Log,
		Warning,
		Error,
	}
	public class LogRecord
	{
		public LogType type;
		public string log;
	}

	public delegate void OnNewLog(LogType type, string log);

	public class CommonLogger{
		const string PreFix = "[CommonLogZyf] ";
		const int MaxLogRecord = 100;
		public static bool ShowLogOnScreen = true;
		public static event OnNewLog NewLogEvent;

		public static Queue<LogRecord> s_Records = new Queue<LogRecord> ();
		public static void Log(string l)
		{
			Debug.Log (PreFix + l);
			RecordLog (LogType.Log, l);
		}

		public static void LogWarning(string l)
		{
			#if UNITY_EDITOR
			Debug.LogWarning("<color=yellow>" + PreFix + l + "</color>");
			#else
			Debug.LogWarning(PreFix + l);
			#endif
			RecordLog (LogType.Warning, l);
		}

		public static void LogError(string e)
		{
			Debug.LogError (PreFix + e);
			RecordLog (LogType.Error, e);
		}

		static void RecordLog(LogType type, string l)
		{
			LogRecord lr = new LogRecord ();
			lr.type = type;
			lr.log = l;

			s_Records.Enqueue (lr);
			if (s_Records.Count > MaxLogRecord)
				s_Records.Dequeue ();

			if (NewLogEvent != null)
				NewLogEvent (type, l);

			CheckLogScreen ();
		}

		static void CheckLogScreen()
		{
			if (!ShowLogOnScreen)
				return;

			GameObject ui_root = GameObject.Find ("UI Root");
			if (ui_root != null) {
				if (ui_root.GetComponentInChildren<ScreenLog> () == null) {
					GameObject sl = GameObject.Instantiate(Resources.Load ("UI/OnScreenLogger") as GameObject);
					Vector3 pos = sl.transform.localPosition;
					Vector3 scale = sl.transform.localScale;
					sl.transform.parent = ui_root.transform.FindChild ("MainPanel");
					sl.transform.localPosition = pos;
					sl.transform.localScale = scale;
				}
			}
		}
	}
}