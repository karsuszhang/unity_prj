using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommonUtil;

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {
		CommonLogger.Log ("StartMain");
		ConfigMng.Instance.Init ();
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void OnStartButtonClicked()
	{
		CommonLogger.Log ("Goes To Game");
		UnityEngine.SceneManagement.SceneManager.LoadScene ("InGame");
	}
}
