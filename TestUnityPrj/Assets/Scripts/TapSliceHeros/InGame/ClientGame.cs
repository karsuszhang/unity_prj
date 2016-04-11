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
		m_Game = new LogicGame ();
		GameData data = new GameData ();
		FillTestData (data);
		m_Game.Init (data);
	}

	void FillTestData(GameData d)
	{
		UnitData ud = new UnitData ();
		ud.attack_time = 1.5f;
		ud.empower_time = 2f;
		ud.idle_time = 2f;
		ud.unit_id = 1;
		ud.max_hp = 100;
		ud.attack_power = 5;
		d.Heroes.Add (ud);

		ud = new UnitData ();
		ud.attack_time = 1.5f;
		ud.empower_time = 2f;
		ud.idle_time = 2f;
		ud.unit_id = 1001;
		ud.max_hp = 50;
		ud.attack_power = 20;
		d.Monsters.Add (ud);
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
