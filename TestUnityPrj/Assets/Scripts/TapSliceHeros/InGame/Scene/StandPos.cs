using UnityEngine;
using System.Collections;

public enum StandType
{
	Player,
	Enemy,
}
public class StandPos : MonoBehaviour {

	[SerializeField]
	public StandType Type;

	// Use this for initialization
	void Awake () {
		GameHelper.Game.RegStandPos (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetTapHero(UnitObject uo)
    {
        TapTaper t = gameObject.GetComponent<TapTaper>();
        if (t != null && uo != null)
        {
            t.TapOnceHandler = uo.OuterTaper;
        }
    }
}
