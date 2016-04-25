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
	// Use this for initialization
	void Start () {
		InitLogicGame ();
		InitClientObj ();

		m_InputMng.AddHandler (OnInput);
	}
	
	// Update is called once per frame
	void Update () {
		m_InputMng.Update ();
		m_Game.Update (MathTool.Second2MilliSec(Time.deltaTime));
	
		foreach (UnitObject uo in m_ClientObjects) {
			uo.Update ();
		}
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
}
