using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InGameLogic;

public class ClientDamageMng
{
	List<DamageObj> m_Damages = new List<DamageObj>();
	
	public void Init()
	{
		GameHelper.Game.LogicGame.DamageManager.OnDamageGen += this.OnDamageGen;
	}


	private void OnDamageGen(Damage d)
	{
		DamageObj o = new DamageObj ();
		o.Init (d);

		m_Damages.Add (o);
	}


	public void Update()
	{
		for (int i = 0; i < m_Damages.Count ;) {
			m_Damages [i].Update ();

			if (m_Damages [i].IsOver) {
				m_Damages [i].Release ();
				m_Damages.RemoveAt (i);
			} else
				i++;
		}
	}
}
