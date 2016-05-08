using System.Collections;
using System.Collections.Generic;
using CommonUtil;

namespace InGameLogic
{
	public enum SkillType
	{
		Damage,
		Heal,
	}

	public enum EnemyType
	{
		Single,
		Mutiple,
		All,
	}

	public class SkillData
	{
		public int SkillID = 0;
		public SkillType Type = SkillType.Damage;
		public EnemyType EnemyNum = EnemyType.Single;
		public int Damage = 0;
		public float ReleaseTime = 0f;
	}
	public class Skill{

		protected SkillData m_Data = null;
		protected BattleUnit m_Unit = null;

		protected bool Used = false;
		protected float m_TimeCount = 0;
		public Skill()
		{
		}

		public void Init(SkillData data, BattleUnit unit)
		{
			m_Data = data;
			m_Unit = unit;
		}

		public void Begin()
		{
			m_TimeCount = 0;
			Used = false;
		}

		public void Update()
		{
			m_TimeCount += LogicGame.LogicFrameTimeInSec;
			if (m_TimeCount >= m_Data.ReleaseTime && !Used) {
				GenDamage ();
			}
		}

		protected List<BattleUnit> FindTarget()
		{
			List<BattleUnit> targets = (m_Unit.IsPlayerSide && (this.m_Data.Type == SkillType.Damage)) ? m_Unit.Game.Enemies : m_Unit.Game.Heros;
			if (this.m_Data.EnemyNum == EnemyType.All) {
				return targets;
			} else if (this.m_Data.EnemyNum == EnemyType.Single) {
				List<BattleUnit> ret = new List<BattleUnit> ();
				ret.Add (targets [m_Unit.Game.Random (0, targets.Count - 1)]);
				return ret;
			} else {
				CommonLogger.LogError ("Not Implement EnemyNum:" + this.m_Data.EnemyNum.ToString ());
				return new List<BattleUnit> ();
			}
		}

		protected void GenDamage()
		{
			List<BattleUnit> targets = FindTarget ();
			foreach (BattleUnit t in targets) {
				DamageData dd = new DamageData ();
				dd.damage = this.m_Data.Damage;
				dd.IsDamage = m_Data.Type == SkillType.Damage;
				if (m_Unit.IsPlayerSide && m_Unit.NextAttackEmpower)
					dd.damage *= 10;
				
				Damage d = new Damage (m_Unit, t, dd, m_Unit.NextAttackEmpower);
				m_Unit.Game.DamageManager.AddDamage (d);
			}

			Used = true;
		}
	}
}