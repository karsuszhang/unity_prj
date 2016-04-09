using UnityEngine;
using System.Collections;
using InGameLogic;

public class ClientGame : MonoBehaviour 
{
	LogicGame m_Game;
	// Use this for initialization
	void Start () {
		m_Game = new LogicGame ();
	
	}
	
	// Update is called once per frame
	void Update () {
		m_Game.Update ();
	
	}
}
