using CommonUtil;
using System.Collections;
using System.Collections.Generic;

namespace InGameLogic
{
	public delegate void OnUnitStateEnter(UnitStateType type);
	public delegate void OnUnitStateLeave(UnitStateType type);
	public delegate void OnUnitHPChanged(int change);
	public delegate void OnUnitDead();

	public class UnitData
	{
		public int unit_id;
		public float idle_time;
		public float empower_time;
		public float attack_time;

		public int max_hp;
		public int hp_recover_rate_per_second = 10;

		public List<SkillData> skills = new List<SkillData>();
	}

	public class BattleUnit 
	{
		protected LogicGame m_Game = null;
		protected UnitData m_OrgData;

		public LogicGame Game { get { return m_Game; } }
		public bool IsDead
		{
			get{ return _isDead; }
			set {
				_isDead = value;
				if (value) {
					m_Game.UnitDead (this);
					if (EventUnitDead != null)
						EventUnitDead ();
				}
			}
		}
		private bool _isDead = false;
		public bool IsPlayerSide = true;

		public int ID {get{ return OrgData.unit_id; }}
		public UnitState CurState { get { return m_CurState; } }
		public UnitData OrgData { get { return m_OrgData; } }

		internal bool NextAttackEmpower = false;

		private List<Skill> m_Skills = new List<Skill>();

		public bool CanDamage
		{
			get {
				if (IsPlayerSide)
					return (m_CurState != null && m_CurState.Type != UnitStateType.Rest);
				else
					return !IsDead;
			}
		}

		#region RuntimeData
		public int HP 
		{ 
			get { return _hp; }
			protected set {
				int diff = value - _hp;
				_hp = System.Math.Max (System.Math.Min (OrgData.max_hp, value), 0);

				if (EventHPChanged != null)
					EventHPChanged (diff);
			}
		}
		private int _hp;
		#endregion

		//event for client obj
		#region Event2Outside
		public event OnUnitStateEnter	EventStateEnter;
		public event OnUnitStateLeave	EventStateLeave;
		public event OnUnitHPChanged	EventHPChanged;
		public event OnUnitDead EventUnitDead;
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
			InitStartData ();
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

			//CommonLogger.Log(string.Format("Unit {0} enter state {1}", OrgData.unit_id, type.ToString()));
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

		void InitStartData()
		{
			HP = OrgData.max_hp;
			foreach (SkillData sd in OrgData.skills) {
				Skill s = new Skill ();
				s.Init (sd, this);
				m_Skills.Add (s);
			}
		}

		public void OnDamage(int damage, BattleUnit attacker)
		{
			if (!CanDamage)
				return;

			//CommonLogger.Log (string.Format ("Unit {0} receive damage {1} from Unit {2}", OrgData.unit_id, damage, attacker != null ? attacker.OrgData.unit_id.ToString() : "PlayerHand"));
			HP -= damage;

			if (HP == 0) {
				if (!IsPlayerSide)
					IsDead = true;
				else
					GoToState (UnitStateType.Rest);
			}

		}

		public void Heal(int hp)
		{
			HP += hp;
		}

		public void EmpowerOK()
		{
			if (CurState.Type == UnitStateType.Empowering) {
				UnitStateEmpower ue = CurState as UnitStateEmpower;
				ue.EmpowerDone = true;
			}
		}

		public bool HasTarget()
		{
			List<BattleUnit> targets = this.IsPlayerSide ? this.Game.Enemies : this.Game.Heros;
			foreach (BattleUnit bu in targets) {
				if (bu.CanDamage)
					return true;
			}

			return false;
		}

		public Skill ChoseSkill()
		{
			int index = Game.Random (0, m_Skills.Count - 1);
			return m_Skills [index];
		}

        public void Tap()
        {
            if (CurState == null || CurState.Type == UnitStateType.Rest)
                return;

            if (CurState.Type != UnitStateType.Empowering)
            {
                CurState.SpeedUpTime();
            }
            else
            {
            }
        }
	}
}