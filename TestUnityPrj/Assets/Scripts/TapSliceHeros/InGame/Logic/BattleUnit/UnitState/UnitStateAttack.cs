using System.Collections.Generic;
using System.Collections;

namespace InGameLogic
{
	public class UnitStateAttack : UnitState{

		float m_StateTime = 0f;
		Skill m_UseSkill = null;
		public UnitStateAttack(BattleUnit bu):base(UnitStateType.Attack, bu)
		{
			
		}

		public override void EnterState ()
		{
			base.EnterState ();
			m_StateTime = 0f;
			m_UseSkill = m_Unit.ChoseSkill ();
			m_UseSkill.Begin ();
		}

		public override void LeaveState ()
		{
			base.LeaveState ();
			m_Unit.NextAttackEmpower = false;
		}

		public override void Update ()
		{
			base.Update ();
			m_StateTime += LogicGame.LogicFrameTimeInSec;

			m_UseSkill.Update ();

			if (m_StateTime >= m_Unit.OrgData.attack_time)
				m_Unit.GoToState (UnitStateType.Idle);
		}


	}
}