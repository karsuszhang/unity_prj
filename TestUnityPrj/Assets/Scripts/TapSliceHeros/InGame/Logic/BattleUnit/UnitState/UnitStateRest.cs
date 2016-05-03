
using System.Collections;

namespace InGameLogic
{
	public class UnitStateRest : UnitState {

		int frame_count = 0;
		public UnitStateRest(BattleUnit bu) : base(UnitStateType.Rest, bu)
		{
		}

		public override void EnterState ()
		{
			base.EnterState ();
			frame_count = 0;
			m_Unit.NextAttackEmpower = false;
		}

		public override void Update ()
		{
			base.Update ();
			frame_count++;
			if (frame_count >= LogicGame.LogicFrameCountPerSecond) {
				m_Unit.Heal (m_Unit.OrgData.hp_recover_rate_per_second);
				frame_count -= LogicGame.LogicFrameCountPerSecond;
			}
			
			if (m_Unit.HP >= m_Unit.OrgData.max_hp)
				m_Unit.GoToState (UnitStateType.Idle);
		}
	}
}