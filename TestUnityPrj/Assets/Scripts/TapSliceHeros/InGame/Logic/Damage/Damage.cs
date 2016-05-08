
using System.Collections;

namespace InGameLogic
{
	public class DamageData
	{
		public bool IsDamage = true;
		public int damage;
	}

	public class Damage{

		public bool IsOver{ get ; protected set;}

		protected BattleUnit m_Attacker;
		protected BattleUnit m_Receiver;
		protected DamageData m_Data;
		protected bool m_Empowered;

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
			if (m_Data.IsDamage) {
				if (m_Receiver.CanDamage)
					m_Receiver.OnDamage (m_Data.damage, m_Attacker);
			} else {
				m_Receiver.Heal (m_Data.damage);
			}

			IsOver = true;
		}
	}
}