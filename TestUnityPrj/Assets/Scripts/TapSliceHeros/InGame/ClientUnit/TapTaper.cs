using UnityEngine;
using System.Collections;
using CommonUtil;

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
	private UISprite m_TapProgress = null;
	// Use this for initialization
	void Awake()
	{
		m_TapProgress = CommonUtil.UIManager.Instance.AddUI ("UI/TapProgress").GetComponent<UISprite> ();
		ShowProgress (false);
	}

	void Start () {
		//CommonLogger.Log (transform.lossyScale.ToString ());
	}
	
	// Update is called once per frame
	void Update () {
		UpdateProgress ();
	}

	void OnDestroy()
	{
		if (m_TapProgress != null)
			CommonUtil.ResourceMng.Instance.Release (m_TapProgress.gameObject);
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
		
		if (Type == TapType.Empower)
			ShowProgress (true);
	}

	void ShowProgress(bool show)
	{
		if (m_TapProgress == null)
			return;

		//CommonUtil.CommonLogger.Log ("Set Tap Progress " + show);
		if (show) {
			m_TapProgress.gameObject.SetActive (true);
			UpdateProgress ();
		} else {
			m_TapProgress.gameObject.SetActive (false);
		}
	}

	void UpdateProgress()
	{
		if (m_TapProgress == null || !m_TapProgress.gameObject.activeSelf)
			return;
		m_TapProgress.gameObject.transform.localPosition = CommonUtil.UIManager.Instance.GetScreenPos (this.transform.position);
		m_TapProgress.fillAmount = 1f - m_TapCount / (float)TapOkCount;

		Vector3 edge_pos = UIManager.Instance.GetScreenPos (this.transform.position + this.transform.lossyScale.x / 2f * UIManager.Instance.MainCamera.gameObject.transform.right);
		//CommonLogger.Log ((edge_pos - m_TapProgress.gameObject.transform.localPosition).magnitude.ToString());
		float scale = (edge_pos - m_TapProgress.gameObject.transform.localPosition).magnitude / 50f;
		m_TapProgress.gameObject.transform.localScale = new Vector3 (1.2f * scale, 1.2f * scale, 1f);
	}

	void OnDisable()
	{
		if(GameHelper.Game != null)
			GameHelper.Game.UnRegTapReceiver (this);
		ShowProgress (false);
	}

	void DoTap()
	{
		if (Type == TapType.Empower) {
			m_TapCount++;
			if (m_TapCount >= TapOkCount && TapOKCB != null) {
				TapOKCB (this);
				if (GetComponent<MeshRenderer> () != null)
					GetComponent<MeshRenderer> ().enabled = false;
				ShowProgress (false);
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
