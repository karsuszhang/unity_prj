using UnityEngine;
using System.Collections;

namespace CommonUtil
{
	public class MainCamera : MonoBehaviour {

		// Use this for initialization
		void Start () {
			UIManager.Instance.MainCamera = this.gameObject.GetComponent<Camera>();
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}