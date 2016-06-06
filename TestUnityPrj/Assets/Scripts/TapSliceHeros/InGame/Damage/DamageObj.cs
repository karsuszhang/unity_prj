using UnityEngine;
using System.Collections;

public class DamageObj
{
	public bool IsOver{ get ; protected set;}

	protected GameObject m_DamageEffect;
    protected InGameLogic.Damage m_LogicDamage;

    protected Vector3 m_FromPos;
    protected Vector3 m_DestPos;

	public DamageObj()
	{
		IsOver = false;
	}

	public void Release()
	{
        if (m_DamageEffect != null)
        {
            CommonUtil.ResourceMng.Instance.Release(m_DamageEffect);
        }
	}

	public void Init(InGameLogic.Damage d)
	{
		IsOver = false;

        m_LogicDamage = d;

        if (m_LogicDamage.Data.fly_able)
        {
            m_DamageEffect = CommonUtil.ResourceMng.Instance.GetResource (d.Data.res, CommonUtil.ResourceType.Effect) as GameObject;
            if (m_DamageEffect == null) 
            {
                CommonUtil.CommonLogger.LogError("damage res " + d.Data.res + " unexist");
            }

            m_FromPos = GameHelper.Game.FindUnit(d.Attacker).GetSpecialPos(UnitObject.SpecialPosType.FirePos);
            m_DestPos = GameHelper.Game.FindUnit(d.Receiver).GetSpecialPos(UnitObject.SpecialPosType.HitPos);
        }
		
	}

	public void Update()
	{
        if (m_LogicDamage.IsOver)
        {
            this.IsOver = true;
        }

        if (m_DamageEffect != null)
        {
            m_DamageEffect.transform.position = Interpolator.LinerInter(m_FromPos, m_DestPos, m_LogicDamage.CurTime / m_LogicDamage.Data.flytime);
        }
	}
}
