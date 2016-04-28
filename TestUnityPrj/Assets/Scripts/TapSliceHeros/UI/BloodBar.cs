using UnityEngine;
using System.Collections;

public class BloodBar : MonoBehaviour {

	public float Value {set{ gameObject.GetComponent<UIProgressBar> ().value = value; }}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
