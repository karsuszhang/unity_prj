using UnityEngine;
using System.Collections;
using InGameLogic;
using CommonUtil;

public class ClientGame : MonoBehaviour 
{
	
	LogicGame m_Game;
	InputMng m_InputMng = new InputMng();

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
	}

	bool OnInput(InputOnce input)
	{
		if (input.type == InputType.Tap) {
			CommonLogger.Log ("Tap");
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
}
