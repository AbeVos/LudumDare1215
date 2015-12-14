using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(StageManager))]
public class StageEditor : Editor
{
    public override void OnInspectorGUI()
    {
        StageManager stageTarget = (StageManager) target;

        EditorGUILayout.LabelField("Enemy Prefabs", EditorStyles.boldLabel);
        stageTarget.dronePrefab = EditorGUILayout.ObjectField("Drone Prefab", stageTarget.dronePrefab, typeof(GameObject), false) as GameObject;
        stageTarget.turretPrefab = EditorGUILayout.ObjectField("Turret Prefab", stageTarget.turretPrefab, typeof(GameObject), false) as GameObject;

        EditorGUILayout.Space();
        stageTarget.buildingPrefab = EditorGUILayout.ObjectField("Building Prefab", stageTarget.buildingPrefab, typeof(GameObject), false) as GameObject;

        stageTarget.difficultyCurve = EditorGUILayout.CurveField(stageTarget.difficultyCurve);
    }
}
