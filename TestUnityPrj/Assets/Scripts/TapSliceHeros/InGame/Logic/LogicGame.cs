using System.Collections;
using System.Collections.Generic;

namespace InGameLogic
{
	public class GameData
	{
	}

	public class LogicGame
	{
		const int LogicFrameTimeInMs = 50;

		private int m_LastMs = 0;
		private ulong m_TotalFrameCount = 0;

		public ulong CurLogicFrameCount{ get { return m_TotalFrameCount; } }

		protected List<BattleUnit> m_Heros = new List<BattleUnit> ();
		protected List<BattleUnit> m_Enemies = new List<BattleUnit> ();
		
		public LogicGame()
		{
		}

		public void Init(GameData data)
		{
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