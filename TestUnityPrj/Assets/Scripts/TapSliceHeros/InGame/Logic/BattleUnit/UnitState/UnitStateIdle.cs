
using System.Collections;

namespace InGameLogic
{
	public class UnitStateIdle : UnitState {

		float m_StateTime = 0f;
		public UnitStateIdle(BattleUnit bu) : base(UnitStateType.Idle, bu)
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
			bool has_target = (m_Unit.IsPlayerSide && m_Unit.Game.Enemies.Count > 0) ||
			                  (!m_Unit.IsPlayerSide && m_Unit.Game.Heros.Count > 0);
			
			if (m_StateTime >= m_Unit.OrgData.idle_time && has_target)
				#if UNITY_5
				m_Unit.GoToState (UnitStateType.Empowering);
				#else
				m_Unit.GoToState(UnitStateType.Attack);
				#endif
		}
	}
}