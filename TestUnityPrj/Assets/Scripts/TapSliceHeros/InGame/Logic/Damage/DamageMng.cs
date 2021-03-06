﻿using System.Collections.Generic;
using System.Collections;

namespace InGameLogic
{
	public delegate void DamageGenerated(Damage d);
	public class DamageMng{

		public event DamageGenerated OnDamageGen;

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

		public void AddDamage(Damage d)
		{
			if (m_Damages.Contains (d)) {
				CommonUtil.CommonLogger.LogError ("Add an exist damage");
				return;
			}

			m_Damages.Add (d);
			if (OnDamageGen != null)
				OnDamageGen (d);
		}
	}
}