using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
[CustomEditor(typeof(ProjectileInspector))]
public class ProjectileInspector : Editor
{
	public override void OnInspectorGUI()
	{
		EditorGUILayout.Vector3Field("Testy", Vector3.one);
		if( GUI.changed )
			EditorUtility.SetDirty(target);
	}
}

