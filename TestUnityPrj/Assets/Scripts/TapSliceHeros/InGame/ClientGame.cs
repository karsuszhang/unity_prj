using UnityEngine;
using System.Collections;
using InGameLogic;
using CommonUtil;

public class ClientGame : MonoBehaviour 
{
	LogicGame m_Game;
	// Use this for initialization
	void Start () {
		m_Game = new LogicGame ();
		GameData data = new GameData ();
		m_Game.Init (data);
	}
	
	// Update is called once per frame
	void Update () {
		m_Game.Update (MathTool.Second2MilliSec(Time.deltaTime));
	
	}
}
