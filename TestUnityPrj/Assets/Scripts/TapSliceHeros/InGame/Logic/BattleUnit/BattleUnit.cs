using CommonUtil;
using System.Collections;
using System.Collections.Generic;

namespace InGameLogic
{
	public delegate void OnUnitStateEnter(UnitStateType type);
	public delegate void OnUnitStateLeave(UnitStateType type);

	public class UnitData
	{
		public int unit_id;
		public float idle_time;
		public float empower_time;
		public float attack_time;
	}

	public class BattleUnit 
	{
		protected LogicGame m_Game = null;
		protected UnitData m_OrgData;

		public bool IsDead = false;
		public bool IsPlayerSide = true;

		public UnitState CurState { get { return m_CurState; } }
		public UnitData OrgData { get { return m_OrgData; } }

		#region Event2Outside
		public event OnUnitStateEnter EventStateEnter;
		public event OnUnitStateLeave EventStateLeave;
		#endregion

		#region State
		private Dictionary<UnitStateType, UnitState> m_States = new Dictionary<UnitStateType, UnitState>();
		private UnitState m_CurState = null;
		#endregion

		public BattleUnit(LogicGame game)
		{
			m_Game = game;
		}

		public void Init(UnitData data, bool is_player_side)
		{
			m_OrgData = data;
			IsPlayerSide = is_player_side;

			InitStates ();
		}

		public void Update()
		{
			if (m_CurState != null)
				m_CurState.Update ();
		}

		public void EnterState(UnitStateType type)
		{
			if (EventStateEnter != null)
				EventStateEnter (type);

			CommonLogger.Log(string.Format("Unit {0} enter state {1}", OrgData.unit_id, type.ToString()));
		}

		public void LeaveState(UnitStateType type)
		{
			if (EventStateLeave != null)
				EventStateLeave (type);
		}

		public void GoToState(UnitStateType type)
		{
			if (!m_States.ContainsKey (type)) {
				CommonLogger.LogError ("GoTo Unexist State " + type.ToString());
				return;
			}

			if (m_CurState != null)
				m_CurState.LeaveState ();

			m_CurState = m_States [type];
			m_CurState.EnterState ();
		}

		void InitStates()
		{
			m_States [UnitStateType.Idle] = UnitState.CreateState (UnitStateType.Idle, this);
			m_States [UnitStateType.Attack] = UnitState.CreateState (UnitStateType.Attack, this);
			m_States [UnitStateType.Rest] = UnitState.CreateState (UnitStateType.Rest, this);
			m_States [UnitStateType.Empowering] = UnitState.CreateState (UnitStateType.Empowering, this);
		
			GoToState (UnitStateType.Idle);
		}
	}
}