using UnityEngine;
using System.Collections;
using System.Threading;

namespace CommonUtil
{
	public class TaskObj
	{
		Thread m_Thread = null;

		public void Start (TaskFunc fun)
		{
			if (m_Thread == null)
				m_Thread = new Thread (TaskUpdate);

			//m_Thread
		}

		void TaskUpdate()
		{
		}
	}
}