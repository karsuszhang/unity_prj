using System.Collections.Generic;
using System.Collections;

namespace InGameLogic
{
	public class UnitStateAttack : UnitState{

		float m_StateTime = 0f;
		bool attacked = false;
		public UnitStateAttack(BattleUnit bu):base(UnitStateType.Attack, bu)
		{
			
		}

		public override void EnterState ()
		{
			base.EnterState ();
			m_StateTime = 0f;
			attacked = false;
		}

		public override void LeaveState ()
		{
			base.LeaveState ();
			m_Unit.NextAttackEmpower = false;
		}

		public override void Update ()
		{
			base.Update ();
			m_StateTime += LogicGame.LogicFrameTimeInSec;

			if (!attacked && m_StateTime >= m_Unit.OrgData.attack_point) {
				BattleUnit target = m_Unit.FindTarget ();
				if (target != null) {
					DamageData data = new DamageData ();
					data.damage = m_Unit.OrgData.attack_power;
					if (m_Unit.IsPlayerSide && m_Unit.NextAttackEmpower)
						data.damage *= 10;
					
					Damage d = new Damage (this.m_Unit, target, data, false);
					m_Unit.Game.DamageManager.AddDamage (d);

					//CommonUtil.CommonLogger.Log ("Attack Time " + m_StateTime);
				}
				attacked = true;
			}

			if (m_StateTime >= m_Unit.OrgData.attack_time)
				m_Unit.GoToState (UnitStateType.Idle);
		}


	}
}