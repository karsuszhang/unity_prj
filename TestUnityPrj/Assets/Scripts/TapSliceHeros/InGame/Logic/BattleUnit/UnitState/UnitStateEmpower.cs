
using System.Collections;

namespace InGameLogic
{
	public class UnitStateEmpower : UnitState {

		float m_StateTime = 0f;
		public UnitStateEmpower(BattleUnit bu) : base(UnitStateType.Empowering, bu)
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
			if (m_StateTime >= m_Unit.OrgData.empower_time)
				m_Unit.GoToState (UnitStateType.Attack);
		}
			
	}
}