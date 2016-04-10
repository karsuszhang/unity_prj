
using System.Collections;

namespace InGameLogic
{
	public class UnitStateAttack : UnitState{

		float m_StateTime = 0f;
		public UnitStateAttack(BattleUnit bu):base(UnitStateType.Attack, bu)
		{
			
		}

		public override void EnterState ()
		{
			base.EnterState ();
			m_StateTime = 0f;
		}

		public override void Update ()
		{
			base.Update ();
			m_StateTime += LogicGame.LogicFrameTimeInSec;

			if (m_StateTime >= m_Unit.OrgData.attack_time)
				m_Unit.GoToState (UnitStateType.Idle);
		}
	}
}