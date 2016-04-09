
using System.Collections;

namespace InGameLogic
{
	public delegate void OnUnitStateEnter(UnitStateType type);
	public delegate void OnUnitStateLeave(UnitStateType type);

	public class UnitData
	{
		public bool is_player_side = false;
	}

	public class BattleUnit 
	{
		protected LogicGame m_Game = null;
		protected UnitData m_OrgData;

		public bool IsDead = false;

		#region Event2Outside
		public event OnUnitStateEnter EventStateEnter;
		public event OnUnitStateLeave EventStateLeave;
		#endregion

		public BattleUnit(LogicGame game)
		{
			m_Game = game;
		}

		public void Init(UnitData data)
		{
			m_OrgData = data;
		}

		public void Update()
		{
		}

		public void EnterState(UnitStateType type)
		{
			if (EventStateEnter != null)
				EventStateEnter (type);
		}

		public void LeaveState(UnitStateType type)
		{
			if (EventStateLeave != null)
				EventStateLeave (type);
		}
	}
}