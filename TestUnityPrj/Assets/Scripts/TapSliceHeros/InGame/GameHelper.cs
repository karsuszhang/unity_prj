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

public class Interpolator
{
    public static Vector3 LinerInter(Vector3 from, Vector3 to, float ratio)
    {
        ratio = Mathf.Min(ratio, 1f);
        ratio = Mathf.Max(0f, ratio);

        return from * (1 - ratio) + to * ratio;
    }
}