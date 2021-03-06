﻿using System.Collections;

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
        public float CurStateTime { get { return m_StateTime; } }

		protected BattleUnit m_Unit { get; private set; }
        protected float m_StateTime = 0f;

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

        public virtual float GetTotalTime()
        {
            return 0f;
        }

        public virtual void SpeedUpTime()
        {
            m_StateTime += 0.1f * GetTotalTime();
        }

		public static UnitState CreateState(UnitStateType type, BattleUnit bu)
		{
			UnitState ret = null;
			switch (type) {
			case UnitStateType.Idle:
				ret = new UnitStateIdle (bu);
				break;
			case UnitStateType.Attack:
				ret = new UnitStateAttack (bu);
				break;
			case UnitStateType.Rest:
				ret = new UnitStateRest (bu);
				break;
			case UnitStateType.Empowering:
				ret = new UnitStateEmpower (bu);
				break;
			}

			return ret;
		}
	}
}