using UnityEngine;
using System.Collections;
using InGameLogic;

public class UnitObject{

	protected BattleUnit m_BattleUnit;
	public UnitObject()
	{
	}

	public virtual void Init(BattleUnit bu)
	{
		m_BattleUnit = bu;
	}

	public virtual void Update()
	{
	}
}
