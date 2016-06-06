
using System.Collections;

namespace InGameLogic
{
	public class DamageData
	{
		public bool IsDamage = true;
		public int damage;
		public float flytime = 0f;
		public bool fly_able = false;
		public string res;
	}

	public class Damage{

		public bool IsOver{ get ; protected set;}
		public DamageData Data {get{ return m_Data; }}
		public float CurTime {get{ return m_CountTime; }}

        public BattleUnit Attacker{get{ return m_Attacker; }}
        public BattleUnit Receiver{get{ return m_Receiver; }}

		protected BattleUnit m_Attacker;
		protected BattleUnit m_Receiver;
		protected DamageData m_Data;
		protected bool m_Empowered;

		protected float m_CountTime = 0f;

		public Damage(BattleUnit attacker, BattleUnit receiver, DamageData data, bool empowered)
		{
			IsOver = false;

			m_Attacker = attacker;
			m_Receiver = receiver;
			m_Data = data;
			m_Empowered = empowered;
		}

		public void Update()
		{
			m_CountTime += LogicGame.LogicFrameTimeInSec;

			if (m_CountTime >= m_Data.flytime) {
				if (m_Data.IsDamage) {
					if (m_Receiver.CanDamage)
						m_Receiver.OnDamage (m_Empowered ? m_Data.damage * 10 : m_Data.damage, m_Attacker);
				} else {
					m_Receiver.Heal (m_Data.damage);
				}

				IsOver = true;
			}
		}
	}
}