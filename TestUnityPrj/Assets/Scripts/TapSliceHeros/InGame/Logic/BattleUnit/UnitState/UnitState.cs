using System.Collections;

namespace InGameLogic
{
	public enum UnitStateType
	{
		Idle,
		Empowering,
		Attack,
		Rest,
	}
	public abstract class UnitState{
		public UnitStateType Type { get; private set; }
		protected BattleUnit m_Unit { get; private set; }

		public UnitState(UnitStateType type, BattleUnit unit)
		{
			this.Type = type;
			m_Unit = unit;
		}

		public virtual void Update()
		{
		}

		public virtual void EnterState()
		{
			m_Unit.EnterState (Type);
		}

		public virtual void LeaveState()
		{
			m_Unit.LeaveState (Type);
		}
	}
}