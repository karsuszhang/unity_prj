using UnityEngine;
using System.Collections;

public class InputMouse : InputBase {

	public InputMouse()
	{
		
	}

	bool m_isButtonDown = false;
	Vector3 m_TapPos;
	public override void CheckInput (System.Collections.Generic.List<InputOnce> inputs)
	{
		if (Input.GetMouseButtonDown (0)) {
			InputOnce ino = new InputOnce ();
			ino.type = InputType.Tap;
			ino.tap_point = Input.mousePosition;
			inputs.Add (ino);

			m_isButtonDown = true;
			m_TapPos = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp (0)) {
			m_isButtonDown = false;
		}

		if (m_isButtonDown) {
			InputOnce ino = new InputOnce ();
			ino.type = InputType.Slice;
			ino.tap_point = m_TapPos;
			ino.second_point = Input.mousePosition;
			ino.slice_cb = SliceEnd;

			inputs.Add (ino);
		}
	}

	void SliceEnd()
	{
		m_TapPos = Input.mousePosition;
	}
}
