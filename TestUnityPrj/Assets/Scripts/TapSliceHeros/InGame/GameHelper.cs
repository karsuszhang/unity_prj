using UnityEngine;
using System.Collections;

public class GameHelper
{
	static public ClientGame Game
	{
		get {
			return GameObject.FindObjectOfType<ClientGame> ();
		}
	}
}
