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
	}
	
	// Update is called once per frame
	void Update () {
		m_Game.Update (MathTool.Second2MilliSec(Time.deltaTime));
	
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

		d.Heroes.Add (ud);
	}

	void InitClientObj()
	{
	}
}
