using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class LogicSkillData
{
	public int SkillID = 0;
	public int Type = 0;
	public int EnemyNum = 0;
	public int Damage = 0;
	public float ReleaseTime = 0f;

	public InGameLogic.SkillData ToSkillData()
	{
		InGameLogic.SkillData sd = new InGameLogic.SkillData ();
		sd.SkillID = this.SkillID;
		sd.ReleaseTime = this.ReleaseTime;
		sd.Type = (InGameLogic.SkillType)this.Type;
		sd.EnemyNum = (InGameLogic.EnemyType)this.EnemyNum;
		sd.Damage = this.Damage;

		return sd;
	}
}

public class LogicSkillCfg : ConfigBase 
{
	Dictionary<int, LogicSkillData> m_Skills = new Dictionary<int, LogicSkillData>();

	public InGameLogic.SkillData GetSkillData(int id)
	{
		if (m_Skills.ContainsKey (id)) {
			return m_Skills [id].ToSkillData ();
		}

		CommonUtil.CommonLogger.Log ("Can't Find LogicSkillData Raw: " + id.ToString ());
		return new InGameLogic.SkillData();
	}

	protected override string GetConfigPath ()
	{
		return "Config/LogicSkill.cfg";
	}

	protected override bool ParseConfig ()
	{
		try
		{
			m_Skills = new Dictionary<int, LogicSkillData>();

			XmlDocument doc = new XmlDocument ();
			doc.LoadXml (m_XmlString);
			XmlNode root = doc.LastChild;

			foreach(XmlNode skill in root)
			{
				LogicSkillData data =  new LogicSkillData();
				foreach(XmlNode attr in skill.ChildNodes)
				{
					if(attr.Name == "id")
					{
						data.SkillID = int.Parse(attr.InnerText);
					}
					else if(attr.Name == "release_time")
					{
						data.ReleaseTime = float.Parse(attr.InnerText);
					}
					else if(attr.Name == "type")
					{
						data.Type = int.Parse(attr.InnerText);
					}
					else if(attr.Name == "enemy_time")
					{
						data.EnemyNum = int.Parse(attr.InnerText);
					}
					else if(attr.Name == "damage")
					{
						data.Damage = int.Parse(attr.InnerText);
					}
				}

				if(m_Skills.ContainsKey(data.SkillID))
					m_Error += string.Format("LogicSkillCfg Repeat ID {0}\n", data.SkillID);

				m_Skills[data.SkillID] = data;
			}

			return true;
		}
		catch(System.Exception ex) {
			m_Error = ex.ToString ();
			return true;
		}
	}
}
