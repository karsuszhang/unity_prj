﻿using System.Collections;
using System.Collections.Generic;

namespace InGameLogic
{
	public delegate UnitData GetNewMonsterData();
	public delegate void OnNewMonsterAdd(BattleUnit bu);

	public class LogicGameData
	{
		public List<UnitData> Heroes = new List<UnitData>();
		public List<UnitData> Monsters = new List<UnitData>();
	}

	public class LogicGame
	{
		public const int LogicFrameTimeInMs = 50;
		public const float LogicFrameTimeInSec = LogicFrameTimeInMs / 1000f;
		public const int LogicFrameCountPerSecond = 1000 / LogicFrameTimeInMs;

		public const float AddMonsterWaitTime = 2f;

		private int m_LastMs = 0;
		private ulong m_TotalFrameCount = 0;
		private float m_NewMonsterTimeCount = 0f;

		public ulong CurLogicFrameCount{ get { return m_TotalFrameCount; } }
		public List<BattleUnit> Heros { get { return m_Heros; } }
		public List<BattleUnit> Enemies { get { return m_Enemies; } }
		public DamageMng DamageManager { get { return m_DamageMng; } }
		public GetNewMonsterData AddMonsterDataHandler = null;
		public event OnNewMonsterAdd EventNewMonsterAdd;

		protected List<BattleUnit> m_Heros = new List<BattleUnit> ();
		protected List<BattleUnit> m_Enemies = new List<BattleUnit> ();

		protected DamageMng m_DamageMng = new DamageMng ();

		public LogicGame()
		{
		}

		public void Init(LogicGameData data)
		{
			foreach (UnitData u in data.Heroes) {
				BattleUnit bu = new BattleUnit (this);
				bu.Init (u, true);
				Heros.Add (bu);
			}

			foreach (UnitData e in data.Monsters) {
				BattleUnit bu = new BattleUnit (this);
				bu.Init (e, false);
				Enemies.Add (bu);
			}
		}

		public void Update(int time_ms)
		{
			int update_time = m_LastMs + time_ms;

			while (update_time >= LogicFrameTimeInMs) {
				update_time -= LogicFrameTimeInMs;
				m_TotalFrameCount++;

				UpdateLogic ();
			}

			m_LastMs = update_time;
		}

		protected virtual void UpdateLogic()
		{
			foreach (BattleUnit u in m_Heros)
				u.Update ();

			foreach (BattleUnit e in m_Enemies)
				e.Update ();

			m_DamageMng.Update ();

			for (int i = 0; i < m_Enemies.Count; ) {
				if (m_Enemies [i].IsDead)
					m_Enemies.RemoveAt (i);
				else
					i++;
			}

			if (m_Enemies.Count == 0) {
				m_NewMonsterTimeCount += LogicFrameTimeInSec;
				if (m_NewMonsterTimeCount >= AddMonsterWaitTime)
					AddEnemy ();
			}
				
		}

		void AddEnemy()
		{
			m_NewMonsterTimeCount = 0f;
			if (AddMonsterDataHandler == null) {
				CommonUtil.CommonLogger.LogWarning ("No AddEnemy Handler");
				return;
			}

			UnitData ud = AddMonsterDataHandler ();
			BattleUnit bu = new BattleUnit (this);
			bu.Init (ud, false);
			Enemies.Add (bu);
			if (EventNewMonsterAdd != null)
				EventNewMonsterAdd (bu);
		}

		public void UnitDead(BattleUnit u)
		{
			if (u.IsPlayerSide)
				CommonUtil.CommonLogger.LogError ("Shouldn't be here, heroes are not support to die now!");
			else {
			}
		}
	}
}