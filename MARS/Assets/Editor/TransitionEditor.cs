using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Animator))]
public class TransitionEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();
		//base.OnInspectorGUI();

		Animator editor = (Animator)target;
		if (GUILayout.Button ("Boo ya")) 
		{
			UnityEditorInternal.AnimatorController ac = (UnityEditorInternal.AnimatorController)editor.runtimeAnimatorController;
			var sm = ac.GetLayer (0).stateMachine;

			for (int i = 0; i < sm.stateCount; i++) 
			{
				Debug.Log("i : " + sm.name);
				UnityEditorInternal.State state = sm.GetState (i);
				var ts = sm.GetTransitionsFromState (state);
				for (int j = 0; j < ts.Length; j++) 
				{
					ts[j].duration = 0f;
					ts[j].offset = 0f;
				}
			}
		}
	}
}
