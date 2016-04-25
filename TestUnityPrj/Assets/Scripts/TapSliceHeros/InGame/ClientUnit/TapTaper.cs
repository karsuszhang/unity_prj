using UnityEngine;
using System.Collections;

public delegate void TapFull(TapTaper t);

public class TapTaper : MonoBehaviour {

	public int TapOkCount = 5;
	public TapFull TapOKCB = null;

	private int m_TapCount = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable()
	{
		GameHelper.Game.RegTapReceiver (this);
		m_TapCount = 0;
	}

	void OnDisable()
	{
		GameHelper.Game.UnRegTapReceiver (this);
	}

	public bool TapInput(InputOnce input)
	{
		if (input.type == InputType.Tap) {
			if (CommonUtil.UIManager.Instance.MainCameraHolder.TapOnCollider (gameObject.GetComponent<Collider> (), input.tap_point)) {
				m_TapCount++;
				if (m_TapCount >= TapOkCount && TapOKCB != null)
					TapOKCB (this);
				return true;
			}
		}

		return false;
	}
}
