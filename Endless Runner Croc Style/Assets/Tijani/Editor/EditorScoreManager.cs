using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScoreManager))]
public class EditorScoreManager : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Delete Highscore"))
        {
            ScoreManager.Instance.DeleteHighscore();
        }

        EditorGUILayout.EndHorizontal();
    }
}
