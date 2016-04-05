using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test4Json
{
	public List<int> list_test = new List<int>();
	public Dictionary<int, string> dic_test = new Dictionary<int, string>();
	public string string_test = "this is test";
}

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Test4Json t = new Test4Json ();
		t.list_test.Add (4);
		t.dic_test [1] = "test1";

		string ret = JsonUtility.ToJson (t);
		Debug.Log (ret);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
