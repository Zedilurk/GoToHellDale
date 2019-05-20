using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Launcher))]
public class ProjectileLauncherEditorScript : Editor
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void OnInspectorGUI()
    {
        Launcher myTarget = (Launcher)target;
        Color originalBackgroundColor = GUI.backgroundColor;

        foreach (FiringGroup firingGroup in myTarget.FiringGroups)
        {
            GUILayout.BeginVertical();
            GUI.backgroundColor = Color.grey;

            foreach (Shot shot in firingGroup.Shots)
            {
                GUILayout.BeginHorizontal();

                shot.Direction = EditorGUILayout.Vector2Field("", shot.Direction);
                shot.Speed = EditorGUILayout.FloatField(shot.Speed);
                shot.ProjectileType = (ProjectileType)EditorGUILayout.EnumPopup(shot.ProjectileType);
                shot.TargetByTag = EditorGUILayout.TextField(shot.TargetByTag);

                if (GUILayout.Button("X"))
                    firingGroup.RemoveShot(shot);

                GUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add Shot"))
                firingGroup.AddShot();

            GUI.backgroundColor = originalBackgroundColor;
            GUILayout.EndVertical();
        }

        if (GUILayout.Button("Add Firing Group"))
            myTarget.FiringGroups.Add(new FiringGroup());

        serializedObject.Update();
        serializedObject.ApplyModifiedProperties();

        //myTarget.experience = EditorGUILayout.IntField("Experience", myTarget.experience);
        //EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
    }
}
