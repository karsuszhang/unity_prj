using UnityEngine;
using System.Collections;

public class TapAlphaTween : AnimatedCurve {

	public float AlphaStart = 0f;
	public float AlphaEnd = 1f;

	Color m_OrgColor;
	MeshRenderer m_MeshRender;
	protected override void Init ()
	{
		base.Init ();
		m_MeshRender = GetComponent<MeshRenderer> ();
		if (m_MeshRender != null && m_MeshRender.material != null)
			m_OrgColor = m_MeshRender.material.color;
	}

	protected override void OnUpdate (float factor, bool isFinished)
	{
		float a = AlphaStart * (1f - factor) + AlphaEnd * factor;

		if (m_MeshRender != null && m_MeshRender.material != null) {
			m_MeshRender.material.color = new Color (m_OrgColor.r, m_OrgColor.g, m_OrgColor.b, a);
		}
	}
}
