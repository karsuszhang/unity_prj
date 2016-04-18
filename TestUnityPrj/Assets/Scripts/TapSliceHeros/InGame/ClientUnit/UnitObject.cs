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
		RegEvent ();
	}

	public virtual void Update()
	{
	}

	public void Release()
	{
		UnRegEvent ();
	}

	void RegEvent()
	{
		m_BattleUnit.EventStateEnter += OnStateEnter;
		m_BattleUnit.EventHPChanged += OnHPChanged;
	}

	void UnRegEvent()
	{
		m_BattleUnit.EventStateEnter -= OnStateEnter;
		m_BattleUnit.EventHPChanged -= OnHPChanged;
	}

	void OnStateEnter(UnitStateType type)
	{
	}

	void OnHPChanged(int changed)
	{
	}
}
