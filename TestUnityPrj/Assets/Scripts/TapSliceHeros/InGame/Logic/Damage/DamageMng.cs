using System.Collections.Generic;
using System.Collections;

namespace InGameLogic
{
	public class DamageMng{

		List<Damage> m_Damages = new List<Damage> ();
		public DamageMng()
		{
		}

		public void Update()
		{
			foreach (Damage d in m_Damages)
				d.Update ();

			for (int i = 0; i < m_Damages.Count; ) {
				if (m_Damages [i].IsOver)
					m_Damages.RemoveAt (i);
				else
					i++;
			}
		}
	}
}