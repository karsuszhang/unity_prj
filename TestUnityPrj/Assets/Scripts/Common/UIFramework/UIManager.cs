using UnityEngine;
using System.Collections;

namespace CommonUtil
{
	public class UIManager : MonoBehaviour {

		public static UIManager Instance
		{
			get {
				if (_instance == null) {
					ResourceMng.Instance.GetResource ("UI/UI Root", ResourceType.UI);

				}

				return _instance;
			}
		}

		protected static UIManager _instance = null;

		void Awake()
		{
			_Awake ();
		}

		void OnDestroy()
		{
			_OnDestroy ();
		}
			
		protected virtual void _Awake()
		{
			_instance = this;
		}

		protected virtual void _OnDestroy()
		{
			_instance = null;
		}

		public virtual void AddUI(string res, Transform parent = null)
		{
			GameObject sl = ResourceMng.Instance.GetResource(res, ResourceType.UI) as GameObject;
			Vector3 pos = sl.transform.localPosition;
			Vector3 scale = sl.transform.localScale;
			sl.transform.parent = parent == null ? this.transform : parent;
			sl.transform.localPosition = pos;
			sl.transform.localScale = scale;
		}
	}
}