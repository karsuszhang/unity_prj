using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InGameLogic;
using CommonUtil;

public delegate void OnTaperGroupDone(TaperGroup group);
public class TaperGroup
{
	public OnTaperGroupDone DoneHandler;
	public int Group;
	public TapType Type;

	List<TapTaper> m_Tapers = new List<TapTaper>();
	int m_CurTapCount = 0;

	public TaperGroup(int group_id, TapType type)
	{
		this.Group = group_id;
		this.Type = type;
	}

	public void Active(bool active)
	{
		m_CurTapCount = 0;
		foreach (TapTaper t in m_Tapers) {
			t.gameObject.SetActive (active);
			if (active)
				t.TapOKCB = this.OnTapOK;
			else
				t.TapOKCB = null;
		}

		if (!active)
			DoneHandler = null;
	}

	void OnTapOK(TapTaper tap)
	{
		m_CurTapCount++;
		if (m_CurTapCount >= m_Tapers.Count && DoneHandler != null)
			DoneHandler (this);
	}

	public void AddTaper(TapTaper t)
	{
		m_Tapers.Add (t);
	}
}

public class UnitObject{

    public enum SpecialPosType
    {
        HitPos,
        FirePos,
    }

	public BattleUnit LogicUnit
	{
		get{ return m_BattleUnit; }
	}

	public bool ReleaseInstantly = false;

	protected BattleUnit m_BattleUnit;
	protected GameObject m_ModelObj = null;
	protected Animator m_AniCtrl = null;

	private int m_FrameBloodChange = 0;

	protected List<TapTaper> m_Taps = new List<TapTaper> ();
	protected int m_TapedCount = 0;
	protected int m_EmpowerTapCount = 0;

	protected Dictionary<TapType, List<TaperGroup>> m_TapGroup = new Dictionary<TapType, List<TaperGroup>>();
	protected TaperGroup m_CurEmpowerGroup = null;
	protected TaperGroup m_CurDamageGroup = null;

	protected BloodBar m_BloodBar = null;
    protected UIProgressBar m_StateProgress = null;
    protected UIProgressBar m_EmpowerProgress = null;

	protected float m_DeadTimeCount = 0f;

	protected StandPos m_StandPos = null;
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
			m_StandPos = p;
            m_StandPos.SetTapHero(this);
		}

		InitTaps ();

		m_BloodBar = (UIManager.Instance.AddUI ("UI/BloodBar") as GameObject).GetComponent<BloodBar>();
        m_StateProgress = (UIManager.Instance.AddUI("UI/StateProgressBar") as GameObject).GetComponent<UIProgressBar>();
        m_EmpowerProgress = (UIManager.Instance.AddUI("UI/EmpowerProgressBar") as GameObject).GetComponent<UIProgressBar>();
        CheckStateProgressBarShow();
		UpdateBloodBarPos ();
	}

	public virtual void Update()
	{

		ShowBloodChange ();
		UpdateBloodBarPos ();
		if (m_BattleUnit.IsDead) {
			m_DeadTimeCount += Time.deltaTime;
			if (m_DeadTimeCount >= LogicGame.AddMonsterWaitTime) {
				ReleaseInstantly = true;
			}
		}
	}

	public void Release()
	{
		UnRegEvent ();
		if (m_ModelObj != null)
			GameObject.Destroy (m_ModelObj);
		if (m_BloodBar != null)
			GameObject.Destroy (m_BloodBar.gameObject);
	}

	void RegEvent()
	{
		m_BattleUnit.EventStateEnter += OnStateEnter;
		m_BattleUnit.EventStateLeave += OnStateLeave;
		m_BattleUnit.EventHPChanged += OnHPChanged;
		m_BattleUnit.EventUnitDead += OnUnitDead;
	}

	void UnRegEvent()
	{
		m_BattleUnit.EventStateEnter -= OnStateEnter;
		m_BattleUnit.EventStateLeave -= OnStateLeave;
		m_BattleUnit.EventHPChanged -= OnHPChanged;
		m_BattleUnit.EventUnitDead -= OnUnitDead;
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

        CheckStateProgressBarShow();
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

	void InitTaps()
	{
		TapTaper[] taps = m_ModelObj.GetComponentsInChildren<TapTaper> ();
		foreach (TapTaper t in taps) {
			t.TapOnceHandler = this.OnTapDirectly;
			t.gameObject.SetActive (false);
			TaperGroup tg = FindTapGroup (t.Type, t.Group);
			if (tg != null)
				tg.AddTaper (t);
			else {
				tg = new TaperGroup (t.Group, t.Type);
				tg.AddTaper (t);
				m_TapGroup[t.Type].Add (tg);
			}
		}
	}

	TaperGroup FindTapGroup(TapType type, int group_id)
	{
		if (!m_TapGroup.ContainsKey (type)) {
			m_TapGroup [type] = new List<TaperGroup> ();
			return null;
		}

		foreach (TaperGroup tg in m_TapGroup[type]) {
			if (tg.Type == type && tg.Group == group_id)
				return tg;
		}

		return null;
	}

	void EnableTaps(bool enable)
	{
		if (enable) {
			m_CurEmpowerGroup = FindAGroup (TapType.Empower);

			if(!m_BattleUnit.IsPlayerSide)
				m_CurDamageGroup = FindAGroup (TapType.Hurt);

			if (m_CurDamageGroup != null)
				m_CurDamageGroup.Active (true);
			if (m_CurEmpowerGroup != null) {
				m_CurEmpowerGroup.Active (true);
				m_CurEmpowerGroup.DoneHandler = this.OnTapGroupDone;
			}
		} else {
			if (m_CurDamageGroup != null)
				m_CurDamageGroup.Active (false);
			if (m_CurEmpowerGroup != null) {
				m_CurEmpowerGroup.Active (false);
				m_CurEmpowerGroup.DoneHandler = null;
			}

			m_CurDamageGroup = null;
			m_CurEmpowerGroup = null;
		}
	}

	TaperGroup FindAGroup(TapType type)
	{
		if (m_TapGroup.ContainsKey (type)) {
			int index = Random.Range (0, m_TapGroup [type].Count);
			return m_TapGroup [type] [index];
		}

		//CommonLogger.LogWarning (string.Format ("{0} has no {1} TapGroup", m_ModelObj.name, type.ToString ()));
		return null;
	}

	void OnTapGroupDone(TaperGroup tg)
	{
		m_BattleUnit.EmpowerOK ();
	}

	void OnUnitDead()
	{
		CommonLogger.Log (m_ModelObj.name + " Dead");
		m_AniCtrl.SetTrigger ("Dead");
		GameHelper.Game.ReleaseStandPos (this.m_StandPos);
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
        m_StateProgress.gameObject.transform.localPosition = m_BloodBar.gameObject.transform.localPosition + new Vector3(0f, 40f, 0f);
        m_EmpowerProgress.gameObject.transform.localPosition = m_BloodBar.gameObject.transform.localPosition + new Vector3(-80, -80, 0f);
        if (m_StateProgress.gameObject.activeSelf)
        {
            m_StateProgress.value = m_BattleUnit.GetStatePercentLeft();
        }
        if (m_EmpowerProgress.gameObject.activeSelf)
        {
            m_EmpowerProgress.value = m_BattleUnit.GetStatePercentLeft();
        }
	}

	void OnTapDirectly()
	{
		this.LogicUnit.OnDamage (GameHelper.Game.GetTapDamage (), null);
	}

    public Vector3 GetSpecialPos(SpecialPosType type)
    {
        switch (type)
        {
            case SpecialPosType.FirePos:
                return m_ModelObj.transform.position;
            case SpecialPosType.HitPos:
                return m_ModelObj.transform.position;
        }

        CommonLogger.LogError("Asking unexist UnitPos " + type.ToString());
        return Vector3.zero;
    }

    public void OuterTaper()
    {
        m_BattleUnit.Tap();
    }

    protected void CheckStateProgressBarShow()
    {
        bool show_state_progress = (m_BattleUnit.CurState.Type == UnitStateType.Idle) || (!m_BattleUnit.IsPlayerSide && m_BattleUnit.CurState.Type == UnitStateType.Empowering);
        m_StateProgress.gameObject.SetActive(show_state_progress);

        m_EmpowerProgress.gameObject.SetActive(m_BattleUnit.IsPlayerSide && m_BattleUnit.CurState.Type == UnitStateType.Empowering);
    }
}
