using UnityEngine;
using System.Collections;

public delegate void TapFull(TapTaper t);

public enum TapType
{
	Empower,
	Hurt,
}

public class TapTaper : MonoBehaviour {

	public TapType Type = TapType.Empower;
	public int TapOkCount = 5;
	public TapFull TapOKCB = null;

	private int m_TapCount = 0;
	private UnitObject m_Unit = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Init(UnitObject uo)
	{
		m_Unit = uo;
	}

	void OnEnable()
	{
		GameHelper.Game.RegTapReceiver (this);
		m_TapCount = 0;
		if (GetComponent<MeshRenderer> () != null)
			GetComponent<MeshRenderer> ().enabled = true;
	}

	void OnDisable()
	{
		if(GameHelper.Game != null)
			GameHelper.Game.UnRegTapReceiver (this);
	}

	void DoTap()
	{
		if (Type == TapType.Empower) {
			m_TapCount++;
			if (m_TapCount >= TapOkCount && TapOKCB != null) {
				TapOKCB (this);
				if (GetComponent<MeshRenderer> () != null)
					GetComponent<MeshRenderer> ().enabled = false;
			}
		} else if (Type == TapType.Hurt) {
			if (m_Unit == null) {
				CommonUtil.CommonLogger.LogError ("Taper " + this.gameObject.name + " has no UnitObj");
				return;
			}

			m_Unit.LogicUnit.OnDamage (GameHelper.Game.GetTapDamage (), null);
		}
	}

	public bool TapInput(InputOnce input)
	{
		if (m_TapCount >= TapOkCount)
			return false;
		
		if (input.type == InputType.Tap) {
			if (CommonUtil.UIManager.Instance.MainCameraHolder.TapOnCollider (gameObject.GetComponent<Collider> (), input.tap_point)) {
				DoTap ();
				return true;
			}
		}

		return false;
	}
}
