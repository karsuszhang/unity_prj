using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class TouchingInput
{
	public Vector3 tap_pos;
	public int id;
	public Vector3 cur_pos;
	public void OnSliceEnd()
	{
		tap_pos = cur_pos;
	}
}

public class InputTouch : InputBase {

	Dictionary<int, TouchingInput> m_Touches = new Dictionary<int, TouchingInput>();

	public InputTouch()
	{
	}

	public override void CheckInput (System.Collections.Generic.List<InputOnce> inputs)
	{
		foreach (Touch t in Input.touches) {
			if (t.phase == TouchPhase.Began) {
				TouchingInput ti = new TouchingInput ();
				ti.tap_pos = ti.cur_pos = t.position;
				ti.id = t.fingerId;
				m_Touches [t.fingerId] = ti;

				InputOnce ino = new InputOnce ();
				ino.type = InputType.Tap;
				ino.tap_point = ti.tap_pos;
				inputs.Add (ino);
			} else if (t.phase == TouchPhase.Moved) {
				if(!m_Touches.ContainsKey(t.fingerId))
					CommonUtil.CommonLogger.LogError("Can't find Touch id in data : " + t.fingerId.ToString());
				else
				{
					m_Touches [t.fingerId].cur_pos = t.position;

					InputOnce ino = new InputOnce ();
					ino.type = InputType.Slice;
					ino.tap_point = m_Touches [t.fingerId].tap_pos;
					ino.second_point = m_Touches [t.fingerId].cur_pos;
					ino.slice_cb = m_Touches [t.fingerId].OnSliceEnd;

					inputs.Add (ino);
				}
			} else if (t.phase == TouchPhase.Canceled || t.phase == TouchPhase.Ended) {
				m_Touches.Remove (t.fingerId);
			}
		}
	}
}
