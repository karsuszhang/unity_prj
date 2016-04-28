using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InGameLogic;
using CommonUtil;

public class UnitObject{

	public BattleUnit LogicUnit
	{
		get{ return m_BattleUnit; }
	}

	protected BattleUnit m_BattleUnit;
	protected GameObject m_ModelObj = null;
	protected Animator m_AniCtrl = null;

	private int m_FrameBloodChange = 0;

	protected List<TapTaper> m_Taps = new List<TapTaper> ();
	protected int m_TapedCount = 0;
	protected int m_EmpowerTapCount = 0;

	protected BloodBar m_BloodBar = null;
	public UnitObject()
	{
	}

	public virtual void Init(BattleUnit bu)
	{
		m_BattleUnit = bu;
		RegEvent ();

		string res;
		if (bu.IsPlayerSide) {
			res = ConfigMng.Instance.GetConfig<ClientHeroCfg> ().HeroClientData [bu.OrgData.unit_id].res_path;
		} else {
			res = ConfigMng.Instance.GetConfig<ClientHeroCfg> ().MonsterClientData [bu.OrgData.unit_id].res_path;
		}

		m_ModelObj = CommonUtil.ResourceMng.Instance.GetResource (res, CommonUtil.ResourceType.Model) as GameObject;
		if (m_ModelObj == null) {
			CommonLogger.LogError ("Model Can't loaded: " + res);
			return;
		}
		m_AniCtrl = m_ModelObj.GetComponent<Animator> ();

		StandPos p = GameHelper.Game.FindEmptyPos (bu.IsPlayerSide ? StandType.Player : StandType.Enemy);
		if (p != null) {
			m_ModelObj.transform.position = p.transform.position;
			m_ModelObj.transform.localRotation = p.transform.localRotation;
			GameHelper.Game.OccupyStandPos (p, bu.OrgData.unit_id);
		}

		m_Taps = new List<TapTaper>(m_ModelObj.GetComponentsInChildren<TapTaper> ());
		foreach (TapTaper t in m_Taps) {
			t.Init (this);
			if(t.Type == TapType.Empower)
				m_EmpowerTapCount++;
		}
		EnableTaps (false);

		m_BloodBar = (UIManager.Instance.AddUI ("UI/BloodBar") as GameObject).GetComponent<BloodBar>();
		UpdateBloodBarPos ();
	}

	public virtual void Update()
	{

		ShowBloodChange ();
		UpdateBloodBarPos ();
	}

	public void Release()
	{
		UnRegEvent ();
		if (m_ModelObj != null)
			GameObject.Destroy (m_ModelObj);
	}

	void RegEvent()
	{
		m_BattleUnit.EventStateEnter += OnStateEnter;
		m_BattleUnit.EventStateLeave += OnStateLeave;
		m_BattleUnit.EventHPChanged += OnHPChanged;
	}

	void UnRegEvent()
	{
		m_BattleUnit.EventStateEnter -= OnStateEnter;
		m_BattleUnit.EventStateLeave -= OnStateLeave;
		m_BattleUnit.EventHPChanged -= OnHPChanged;
	}

	void OnStateEnter(UnitStateType type)
	{
		if (m_AniCtrl != null) {
			switch (type) {
			case UnitStateType.Empowering:
				m_AniCtrl.SetTrigger ("Empower");
				EnableTaps (true);
				break;
			case UnitStateType.Attack:
				m_AniCtrl.SetTrigger ("Attack");
				break;
			case UnitStateType.Rest:
				m_AniCtrl.SetTrigger ("Rest");
				break;
			case UnitStateType.Idle:
				m_AniCtrl.SetTrigger ("Idle");
				break;
			}
		}
	}

	void OnStateLeave(UnitStateType type)
	{
		if (m_AniCtrl != null) {
			switch (type) {
			case UnitStateType.Empowering:
				m_AniCtrl.ResetTrigger ("Empower");
				EnableTaps (false);
				break;
			case UnitStateType.Attack:
				m_AniCtrl.ResetTrigger ("Attack");
				break;
			case UnitStateType.Rest:
				m_AniCtrl.ResetTrigger ("Rest");
				break;
			case UnitStateType.Idle:
				m_AniCtrl.ResetTrigger ("Idle");
				break;
			}
		}
	}

	void OnHPChanged(int changed)
	{
		m_FrameBloodChange += changed;

		if (m_BloodBar != null) {
			m_BloodBar.Value = m_BattleUnit.HP / (float)m_BattleUnit.OrgData.max_hp;
		}
	}

	void ShowBloodChange()
	{
		if (m_FrameBloodChange != 0) {
			
			Vector3 new_pos = GetBloodPointPos();
			GameObject label = null;
			if (m_FrameBloodChange > 0) {
				
				label = UIManager.Instance.AddUI("UI/HealLabel");
				HealLabel hl = label.GetComponent<HealLabel> ();
				hl.DamageShow = m_FrameBloodChange;
				hl.Reposition (new_pos);

			} else {

				label = UIManager.Instance.AddUI ("UI/DamageLabel");
				//CommonLogger.Log ("Pos before " + label.gameObject.transform.position.ToString ());
				DamageLabel dl = label.GetComponent<DamageLabel> ();
				dl.DamageShow = m_FrameBloodChange;
				dl.Reposition (new_pos);
				//CommonLogger.LogError ("Pos after " + label.gameObject.transform.position.ToString ());
			}
				
			m_FrameBloodChange = 0;
		}
	}

	void EnableTaps(bool enable)
	{
		foreach (TapTaper tt in m_Taps) {
			tt.gameObject.SetActive (enable);
			if (enable)
				tt.TapOKCB = this.OnTapFull;
			else
				tt.TapOKCB = null;
		}
		m_TapedCount = 0;
	}

	void OnTapFull(TapTaper t)
	{		
		//CommonLogger.Log ("Taper " + t.name + " ok ");
		m_TapedCount++;
		if (m_TapedCount >= m_EmpowerTapCount) {
			//CommonLogger.Log ("Empower Full");
			m_BattleUnit.EmpowerOK ();
		}
	}

	Vector3 GetBloodPointPos()
	{
		BloodPoint bp = m_ModelObj.GetComponentInChildren<BloodPoint> ();
		if (bp == null) {
			CommonUtil.CommonLogger.LogError (string.Format ("Model {0} has no BloodPoint", m_ModelObj.name));
			m_FrameBloodChange = 0;
			return Vector3.zero;
		}

		Vector3 new_pos = UIManager.Instance.GetScreenPos (bp.transform.position);

		return new_pos;
	}

	void UpdateBloodBarPos()
	{
		if (m_BloodBar == null)
			return;

		m_BloodBar.gameObject.transform.localPosition = GetBloodPointPos ();
	}
}
