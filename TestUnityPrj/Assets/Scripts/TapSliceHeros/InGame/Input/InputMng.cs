using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum InputType
{
	Tap,
	Slice,
}

public class InputOnce
{
	public InputType type;
	public Vector3 tap_point;
	public Vector3 second_point;
	public SliceEndCallBack slice_cb;
}

public class InputBase
{
	public virtual void CheckInput (List<InputOnce> inputs)
	{
	}
}

public delegate bool InputHandler(InputOnce inputs);
public delegate void SliceEndCallBack();

public class InputMng {

	List<InputOnce> m_Inputs = new List<InputOnce>();
	List<InputHandler> m_Handlers = new List<InputHandler>();
	InputBase m_curInput = null;

	public InputMng()
	{
		#if UNITY_EDITOR
		m_curInput = new InputMouse();
		#else
		m_curInput = new InputTouch();
		#endif
	}

	public void AddHandler (InputHandler h, bool head = false)
	{
		if(h == null || m_Handlers.Contains(h))
		{
			CommonUtil.CommonLogger.LogError("AddHandler Error : is null " + (h == null));
			return;
		}

		if (head)
			m_Handlers.Insert (0, h);
		else
			m_Handlers.Add (h);
	}

	public void RemoveHandler(InputHandler h)
	{
		m_Handlers.Remove (h);
	}

	public void Release()
	{
		m_Handlers.Clear ();
	}

	public void Update () {

		m_curInput.CheckInput (m_Inputs);
		foreach (InputOnce input in m_Inputs) {
			for (int i = 0; i < m_Handlers.Count; i++) {
				if (m_Handlers [i] (input))
					break;
			}
		}
		m_Inputs.Clear ();
	}
}
