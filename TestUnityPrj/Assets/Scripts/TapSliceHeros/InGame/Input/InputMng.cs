using UnityEngine;
using System.Collections;

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
}

public class InputMng {

	public InputMng()
	{
		
	}

	public void Update () {
	
	}
}
