﻿
using System.Collections;

namespace InGameLogic
{
	public class UnitStateEmpower : UnitState {
		public bool EmpowerDone;

		float m_StateTime = 0f;
		public UnitStateEmpower(BattleUnit bu) : base(UnitStateType.Empowering, bu)
		{
		}

		public override void EnterState ()
		{
			base.EnterState ();
			m_StateTime = 0f;
			EmpowerDone = false;
		}

		public override void Update ()
		{
			base.Update ();
			m_StateTime += LogicGame.LogicFrameTimeInSec;
			if (m_StateTime >= m_Unit.OrgData.empower_time) {
				if (m_Unit.IsPlayerSide) {
					m_Unit.NextAttackEmpower = EmpowerDone;
					m_Unit.GoToState (UnitStateType.Attack);
				} else {
					if (EmpowerDone)
						m_Unit.GoToState (UnitStateType.Idle);
					else
						m_Unit.GoToState (UnitStateType.Attack);
				}
			}
		}
			
	}
}