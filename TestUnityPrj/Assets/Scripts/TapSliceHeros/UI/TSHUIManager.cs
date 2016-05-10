using UnityEngine;
using System.Collections;
using CommonUtil;

public enum DynamicDepth
{
	DepthMostFront = 100,
}

public static class UIExt
{
	public static int ToInt(this DynamicDepth d)
	{
		return (int)d;
	}
}

public class TSHUIManager : UIManager {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
