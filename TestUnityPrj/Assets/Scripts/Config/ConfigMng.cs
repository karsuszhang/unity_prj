using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ConfigBase
{
	protected ConfigInitOver m_Handler;
	protected string m_XmlString;
	protected string m_Error;

	public virtual void Init (ConfigInitOver handler)
	{
		this.m_Handler = handler;

		m_XmlString = (CommonUtil.ResourceMng.Instance.GetResource (GetConfigPath(), CommonUtil.ResourceType.TXT) as TextAsset).text;
		if (string.IsNullOrEmpty (m_XmlString) && handler != null)
			m_Handler (false, "Can't Open Cfg " + GetConfigPath(), this);
		else if (!string.IsNullOrEmpty (m_XmlString)) {
			CommonUtil.TaskMng.Instance.StartTreadTask (ParseConfig, OnParseTaskOver, null);
		}
	}

	protected virtual string GetConfigPath ()
	{
		return null;
	}

	protected virtual void OnParseTaskOver(object o)
	{
		if (m_Handler != null)
			m_Handler (true, m_Error, this);
	}

	protected virtual bool ParseConfig()
	{
		return true;
	}
}

public delegate void ConfigAllSet(bool ok);
public delegate void ConfigInitOver(bool suc, string error, ConfigBase config);

public class ConfigMng{
	private ConfigMng()
	{
		//Init ();
	}
		
	public static ConfigMng Instance{
		get
		{
			if(_instance == null)
				_instance = new ConfigMng();
			
			return _instance;
		}
	}
	static ConfigMng _instance;

	public event ConfigAllSet EventConfigAllLoaded;
	Dictionary<ConfigBase, bool> m_Configs = new Dictionary<ConfigBase, bool>();

	public void Init()
	{
		RegisterConfigs ();

		foreach (var obj in m_Configs) {
			obj.Key.Init (ConfigLoadOver);
		}
	}

	void RegisterConfigs()
	{
		m_Configs [new LogicHeroCfg ()] = false;
		m_Configs [new ClientHeroCfg ()] = false;
	}
		
	void ConfigLoadOver(bool suc, string error, ConfigBase config)
	{
		if (suc)
			m_Configs [config] = true;
		else {
			CommonUtil.CommonLogger.LogError (string.Format ("Config {0} Init Failed:{1}", config.GetType ().ToString (), error)); 
		}

		bool all_load = IsAllConfigInitOK ();

		if (all_load) {
			CommonUtil.CommonLogger.Log ("All Config Load OK");
			if(EventConfigAllLoaded != null)
				EventConfigAllLoaded (all_load);
		}	
	}

	public bool IsAllConfigInitOK()
	{
		bool all_load = true;
		foreach(var obj in m_Configs)
		{
			if(!obj.Value)
				all_load = false;
		}

		return all_load;
	}

	public T GetConfig<T>() where T : ConfigBase
	{
		ConfigBase ret = null;
		foreach (var obj in m_Configs) {
			if (obj.Key is T)
				ret = obj.Key;
		}

		if (ret == null) {
			CommonUtil.CommonLogger.LogError ("Request an unexist config type " + typeof(T));
			return default(T);
		}

		if (!m_Configs [ret])
			CommonUtil.CommonLogger.LogError ("Request config not init succeed : " + typeof(T));
		
		return (m_Configs [ret]) ? ((T)ret) : default(T);
	}
}
