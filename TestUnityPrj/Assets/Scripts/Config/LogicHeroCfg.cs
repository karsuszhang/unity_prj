using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class LogicHeroData
{
	public int id;
	public float idle_time;
	public float empower_time;
	public float attack_time;
	public int max_hp;
	public int hp_recover_rate;
	public List<int> skills;

	public InGameLogic.UnitData ToUnitData()
	{
		InGameLogic.UnitData ud = new InGameLogic.UnitData ();
		ud.unit_id = id;
		ud.idle_time = idle_time;
		ud.empower_time = empower_time;
		ud.attack_time = attack_time;
		ud.max_hp = max_hp;
		ud.hp_recover_rate_per_second = hp_recover_rate;
		foreach (int sid in skills) {
			ud.skills.Add (ConfigMng.Instance.GetConfig<LogicSkillCfg> ().GetSkillData (sid));
		}

		return ud;
	}
}
public class LogicHeroCfg : ConfigBase 
{
	public Dictionary<int, LogicHeroData> HeroLogicData {get ; private set;} 

	public LogicHeroData GetHeroData(int id)
	{
		if (HeroLogicData.ContainsKey (id))
			return HeroLogicData [id];

		return null;
	}

	protected override string GetConfigPath ()
	{
		return "Config/LogicHero.cfg";
	}

	protected override bool ParseConfig ()
	{
		try
		{
			HeroLogicData = new Dictionary<int, LogicHeroData>();

			XmlDocument doc = new XmlDocument ();
			doc.LoadXml (m_XmlString);
			XmlNode root = doc.LastChild;
			foreach(XmlNode hero in root.ChildNodes)
			{
				LogicHeroData hd = new LogicHeroData();
				foreach(XmlNode attr in hero.ChildNodes)
				{
					if(attr.Name == "id")
					{
						hd.id = int.Parse(attr.InnerText);
					}
					else if(attr.Name == "idle_time")
					{
						hd.idle_time = float.Parse(attr.InnerText);
					}
					else if(attr.Name == "empower_time")
					{
						hd.empower_time = float.Parse(attr.InnerText);
					}
					else if(attr.Name == "attack_time")
					{
						hd.attack_time = float.Parse(attr.InnerText);
					}
					else if(attr.Name == "max_hp")
					{
						hd.max_hp = int.Parse(attr.InnerText);
					}
					else if(attr.Name == "hp_recover")
					{
						hd.hp_recover_rate = int.Parse(attr.InnerText);
					}
					else if(attr.Name == "Skills")
					{
						hd.skills = new List<int>();
						foreach(XmlNode sid in attr.ChildNodes)
						{
							hd.skills.Add(int.Parse(sid.InnerText));
						}
					}
				}
				if(HeroLogicData.ContainsKey(hd.id))
					m_Error += string.Format("Hero id {0} repeated \n", hd.id);

				HeroLogicData[hd.id] = hd;
			}
			return true;
		}
		catch(System.Exception ex) {
			m_Error = ex.ToString ();
			return true;
		}
	}
}
