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
		TestInput ();
	}

	public void OnStartButtonClicked()
	{
		CommonLogger.Log ("Goes To Game");
		UnityEngine.SceneManagement.SceneManager.LoadScene ("InGame");
	}

	void TestInput()
	{
		#if UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.Space))	{
			TweenScale ts = UIManager.Instance.Root.gameObject.transform.FindChild("ComboLabel").GetComponent<TweenScale>();
			ts.ResetToBeginning();
			ts.PlayForward();
		}
		#endif
	}
}
