using UnityEngine;
using System.Collections;

public class HealLabel : MonoBehaviour {
	const float PosYMove = 80f;

	public int DamageShow{
		get { return _Damage; }
		set {
			_Damage = value;
			m_Label.text = string.Format("+{0}", _Damage);
		}
	}

	private int _Damage;
	private UILabel m_Label;

	void Awake()
	{
		m_Label = gameObject.GetComponent<UILabel> ();
		if (m_Label == null) {
			CommonUtil.CommonLogger.LogError ("HealLabel Gameobj has no Label: " + gameObject.name);
		}
	}

	// Use this for initialization
	void Start () {
		this.GetComponent<UILabel> ().depth = DynamicDepth.DepthMostFront.ToInt();
	}

	public void Reposition(Vector3 pos)
	{
		this.transform.localPosition = pos;

		TweenPosition tp = gameObject.GetComponent<TweenPosition> ();
		if (tp == null) {
			CommonUtil.CommonLogger.LogError ("HealLabel Gameobj has no PosTween: " + gameObject.name);
			return;
		}

		tp.from = this.transform.localPosition;
		tp.to = tp.from + new Vector3 (0, PosYMove, 0);

		tp.AddOnFinished (Finished);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Finished()
	{
		GameObject.Destroy (gameObject);
	}
}
