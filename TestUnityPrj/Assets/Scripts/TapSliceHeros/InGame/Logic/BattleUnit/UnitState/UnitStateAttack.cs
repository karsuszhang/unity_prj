using System.Collections.Generic;
using System.Collections;

namespace InGameLogic
{
	public class UnitStateAttack : UnitState{

		float m_StateTime = 0f;
		public UnitStateAttack(BattleUnit bu):base(UnitStateType.Attack, bu)
		{
			
		}

		public override void EnterState ()
		{
			base.EnterState ();
			m_StateTime = 0f;

			BattleUnit target = FindTarget ();
			if (target != null) {
				DamageData data = new DamageData ();
				data.damage = m_Unit.OrgData.attack_power;
				Damage d = new Damage (this.m_Unit, target, data, false);
				m_Unit.Game.DamageManager.AddDamage (d);
			}
		}

		public override void Update ()
		{
			base.Update ();
			m_StateTime += LogicGame.LogicFrameTimeInSec;

			if (m_StateTime >= m_Unit.OrgData.attack_time)
				m_Unit.GoToState (UnitStateType.Idle);
		}

		BattleUnit FindTarget()
		{
			List<BattleUnit> targets = m_Unit.IsPlayerSide ? m_Unit.Game.Enemies : m_Unit.Game.Heros;
			foreach (BattleUnit bu in targets) {
				if (bu.CanDamage)
					return bu;
			}

			return null;
		}
	}
}