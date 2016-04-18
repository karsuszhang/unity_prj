using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class ClientHeroData
{
	public int logic_id;
	public string res_path;
}

public class ClientHeroCfg : ConfigBase 
{
	public Dictionary<int, ClientHeroData> HeroClientData { get; private set; }
	public Dictionary<int, ClientHeroData> MonsterClientData { get; private set; }

	public ClientHeroData GetHeroData(int id)
	{
		if (HeroClientData.ContainsKey (id))
			return HeroClientData [id];
		return null;
	}

	public ClientHeroData GetMonsterData(int id)
	{
		if (MonsterClientData.ContainsKey (id))
			return MonsterClientData [id];
		return null;
	}

	protected override string GetConfigPath ()
	{
		return "Config/ClientHero.cfg";
	}

	protected override bool ParseConfig ()
	{
		try
		{
			HeroClientData = new Dictionary<int, ClientHeroData>();
			MonsterClientData = new Dictionary<int, ClientHeroData>();

			XmlDocument doc = new XmlDocument ();
			doc.LoadXml (m_XmlString);
			XmlNode root = doc.LastChild;
			foreach(XmlNode tag in root.ChildNodes)
			{
				if(tag.InnerText == "Heroes")
				{
					foreach(XmlNode h in tag.ChildNodes)
					{
						ClientHeroData hd = new ClientHeroData();

						ReadData(h, hd);
						HeroClientData[hd.logic_id] = hd;
					}
				}
				else if(tag.InnerText == "Monsters")
				{
					foreach(XmlNode h in tag.ChildNodes)
					{
						ClientHeroData hd = new ClientHeroData();

						ReadData(h, hd);
						MonsterClientData[hd.logic_id] = hd;
					}
				}
			}
			return true;
		}
		catch(System.Exception ex) {
			m_Error = ex.ToString ();
			return true;
		}
	}

	void ReadData(XmlNode node, ClientHeroData data)
	{
		foreach(XmlNode a in node.ChildNodes)
		{
			if(a.Name == "logic_id")
				data.logic_id = int.Parse(a.InnerText);
			else if(a.Name == "res")
				data.res_path = a.InnerText;
		}
	}
}
