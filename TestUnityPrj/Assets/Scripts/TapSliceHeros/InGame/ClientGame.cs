using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InGameLogic;
using CommonUtil;

public class ClientGame : MonoBehaviour 
{

	LogicGame m_Game;
	InputMng m_InputMng = new InputMng();
	List<UnitObject> m_ClientObjects = new List<UnitObject>();
	Dictionary<StandPos, int> m_StandPos = new Dictionary<StandPos, int>();

	List<TapTaper> m_RegTaps = new List<TapTaper>();

	List<UnitObject> m_DeadObjs = new List<UnitObject> ();

	int m_MonsterCount = 0;

	TweenScale m_ComboLabel;
	GameObject m_AttUp;
	GameObject m_MFUp;
	// Use this for initialization
	void Start () {
		InitLogicGame ();
		InitClientObj ();

		InitUI ();
		m_InputMng.AddHandler (OnInput);
	}
	
	// Update is called once per frame
	void Update () {
		m_InputMng.Update ();
		m_Game.Update (MathTool.Second2MilliSec(Time.deltaTime));
	

		foreach (UnitObject uo in m_ClientObjects) {
			uo.Update ();
			if (uo.ReleaseInstantly)
				m_DeadObjs.Add (uo);
		}

		foreach (UnitObject du in m_DeadObjs) {
			m_ClientObjects.Remove (du);
			du.Release ();
		}

		m_DeadObjs.Clear ();
	}

	void OnDestory()
	{
		m_InputMng.Release ();
	}

	void InitLogicGame()
	{
		Test ();
		m_Game = new LogicGame ();
		LogicGameData data = new LogicGameData ();
		FillLogicData (data);
		m_Game.Init (data);

		m_Game.AddMonsterDataHandler = this.GetNewMonster;
		m_Game.EventNewMonsterAdd += this.OnNewMonsterAdd;
		m_Game.EventOnCombo += this.OnCombo;
	}

	void Test()
	{
		GameData.CurHeroes.Add (1);
		GameData.CurMonsters.Add (1001);
	}

	void FillLogicData(LogicGameData d)
	{
		foreach(int id in GameData.CurHeroes)
			d.Heroes.Add (ConfigMng.Instance.GetConfig<LogicHeroCfg>().GetHeroData(id).ToUnitData());

		foreach(int id in GameData.CurMonsters)
			d.Monsters.Add (ConfigMng.Instance.GetConfig<LogicHeroCfg>().GetHeroData(id).ToUnitData());
	}

	void InitClientObj()
	{
		foreach (BattleUnit bu in m_Game.Heros) {
			UnitObject uo = new UnitObject ();
			uo.Init (bu);
			m_ClientObjects.Add (uo);
		}

		foreach (BattleUnit bu in m_Game.Enemies) {
			UnitObject uo = new UnitObject ();
			uo.Init (bu);
			m_ClientObjects.Add (uo);
		}
	}

	void InitUI()
	{
		GameObject root = UIManager.Instance.AddUI ("UI/InGamePanel");	
		m_ComboLabel = root.transform.Find ("ComboLabel").gameObject.GetComponent<TweenScale> ();
		m_ComboLabel.gameObject.SetActive (false);
		m_ComboLabel.AddOnFinished (this.OnComboFinish);

		m_AttUp = root.transform.Find ("AttackUp").gameObject;
		m_MFUp = root.transform.Find ("MFUp").gameObject;
		m_AttUp.SetActive (false);
		m_MFUp.SetActive (false);
	}

	bool OnInput(InputOnce input)
	{
		if (input.type == InputType.Tap) {
			//CommonLogger.Log ("Tap");
			foreach (TapTaper t in m_RegTaps) {
				t.TapInput (input);
			}
		}

		if (input.type == InputType.Slice) {
			if((input.second_point - input.tap_point).magnitude >= 20)
			{
				CommonLogger.Log("Slice");
				if(input.slice_cb != null)
					input.slice_cb();
				return true;
			}
		}
		return false;
	}

	public void RegStandPos(StandPos p)
	{
		if (p != null && !m_StandPos.ContainsKey (p))
			m_StandPos [p] = -1;
	}

	public StandPos FindEmptyPos(StandType t)
	{
		foreach (var obj in m_StandPos) {
			if (obj.Value < 0 && obj.Key.Type == t)
				return obj.Key;
		}

		return null;
	}

	public void OccupyStandPos(StandPos p, int id)
	{
		if(m_StandPos.ContainsKey(p))
			m_StandPos[p] = id;
	}

	public void ReleaseStandPos(StandPos p)
	{
		if (m_StandPos.ContainsKey (p)) {
			m_StandPos [p] = -1;
		}
	}

	public void RegTapReceiver(TapTaper tap)
	{
		if (m_RegTaps.Contains (tap))
			return;

		m_RegTaps.Add (tap);
	}

	public void UnRegTapReceiver(TapTaper tap)
	{
		m_RegTaps.Remove (tap);
	}

	public int GetTapDamage()
	{
		return 5;
	}

	public UnitData GetNewMonster()
	{
		m_MonsterCount++;
		UnitData data = ConfigMng.Instance.GetConfig<LogicHeroCfg> ().GetHeroData (1001).ToUnitData ();
		data.max_hp = Mathf.CeilToInt ((1f + m_MonsterCount / 10f) * data.max_hp);
		//data.attack_power = Mathf.CeilToInt((1f + (m_MonsterCount / 20f)) * data.attack_power);
		return data;
	}

	void OnNewMonsterAdd(BattleUnit bu)
	{
		UnitObject uo = new UnitObject ();
		uo.Init (bu);
		m_ClientObjects.Add (uo);
	}

	void OnCombo(int combo_num)
	{
		if (combo_num > 0) {
			m_ComboLabel.gameObject.SetActive (true);
			m_ComboLabel.ResetToBeginning ();
			m_ComboLabel.PlayForward ();
			m_ComboLabel.gameObject.GetComponent<UILabel> ().text = "Combo x " + combo_num;
			if (combo_num > 5)
				m_AttUp.SetActive (true);
			if (combo_num > 10)
				m_MFUp.SetActive (true);
			
		} else {
			m_AttUp.SetActive (false);
			m_MFUp.SetActive (false);
		}
	}

	void OnComboFinish()
	{
		m_ComboLabel.gameObject.SetActive (false);
	}
}
