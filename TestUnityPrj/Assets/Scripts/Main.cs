using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommonUtil;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {

		start_frame = Time.frameCount;

		TaskMng.Instance.StartTreadTask (this.testTask);

		CommonLogger.LogWarning ("test");

		CommonLogger.LogError ("second test use event");
	}
	int start_frame = 0;
	// Update is called once per frame
	void Update () {
		if (t == 10000000) {
			Debug.Log ("Test start at frame " + start_frame + " end at " + Time.frameCount);
			t++;
		}
	
	}

	int t = 0;
	bool testTask()
	{
		if (t < 10000000) {
			t++;
			return false;
		}
		return true;
	}
}
