using System.Collections;
using System.Collections.Generic;

namespace InGameLogic
{
	public class GameData
	{
		public List<UnitData> Heroes = new List<UnitData>();
		public List<UnitData> Monsters = new List<UnitData>();
	}

	public class LogicGame
	{
		public const int LogicFrameTimeInMs = 50;
		public const float LogicFrameTimeInSec = LogicFrameTimeInMs / 1000f;

		private int m_LastMs = 0;
		private ulong m_TotalFrameCount = 0;

		public ulong CurLogicFrameCount{ get { return m_TotalFrameCount; } }
		public List<BattleUnit> Heros { get { return m_Heros; } }
		public List<BattleUnit> Enemies { get { return m_Enemies; } }

		protected List<BattleUnit> m_Heros = new List<BattleUnit> ();
		protected List<BattleUnit> m_Enemies = new List<BattleUnit> ();
		
		public LogicGame()
		{
		}

		public void Init(GameData data)
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
		}
	}
}