using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TapAlphaTween))]
public class TapAlphaTweenEditor : AnimatedCurveEditor {
	public override void OnInspectorGUI ()
	{
		GUILayout.Space(6f);
		NGUIEditorTools.SetLabelWidth(120f);

		TapAlphaTween tw = target as TapAlphaTween;
		GUI.changed = false;

		float from = EditorGUILayout.Slider("From", tw.AlphaStart, 0f, 1f);
		float to = EditorGUILayout.Slider("To", tw.AlphaEnd, 0f, 1f);

		if (GUI.changed)
		{
			NGUIEditorTools.RegisterUndo("AnimateCurve Change", tw);
			tw.AlphaStart = from;
			tw.AlphaEnd = to;
			NGUITools.SetDirty(tw);
		}

		DrawCommonProperties();
	}
}

