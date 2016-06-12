
using System.Collections;

namespace InGameLogic
{
	public class UnitStateEmpower : UnitState {
		public bool EmpowerDone
		{
			get {
				return _EmpowerDone;
			}
			set{
				if (!_EmpowerDone && value && !m_Unit.IsPlayerSide)
					m_Unit.Game.IncreseBlockEnemy ();
				_EmpowerDone = value;
				}
		}

		private bool _EmpowerDone;

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
					if (m_Unit.HasTarget()){
						m_Unit.NextAttackEmpower = EmpowerDone;
						m_Unit.GoToState (UnitStateType.Attack);
					} else
						m_Unit.GoToState (UnitStateType.Idle);
				} else {
					if (EmpowerDone) {
						//m_Unit.Game.IncreseBlockEnemy ();
						m_Unit.GoToState (UnitStateType.Idle);
					} else {
						m_Unit.Game.ResetBlockEnemy ();
						m_Unit.GoToState (UnitStateType.Attack);
					}
				}
			}
		}
		
        public override float GetTotalTime()
        {
            return m_Unit.OrgData.empower_time;
        }
	}
}