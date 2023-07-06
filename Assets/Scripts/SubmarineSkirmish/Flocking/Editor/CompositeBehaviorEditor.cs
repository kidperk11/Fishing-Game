using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CompositeBehavior))]
public class CompositeBehaviorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    //private SerializedProperty behaviorsProp;
    //private SerializedProperty weightsProp;

    //private void OnEnable()
    //{
    //    behaviorsProp = serializedObject.FindProperty("behaviors");
    //    weightsProp = serializedObject.FindProperty("weights");
    //}

    //public override void OnInspectorGUI()
    //{
    //    //setup
    //    CompositeBehavior cb = (CompositeBehavior)target;

    //    Rect r = EditorGUILayout.BeginHorizontal();
    //    r.height = EditorGUIUtility.singleLineHeight;

    //    //check for behaviors
    //    if (cb.Behaviors == null || cb.Behaviors.Length == 0)
    //    {
    //        EditorGUILayout.HelpBox("No behaviors in array.", MessageType.Warning);
    //        EditorGUILayout.EndHorizontal();
    //        r = EditorGUILayout.BeginHorizontal();
    //        r.height = EditorGUIUtility.singleLineHeight;
    //    }
    //    else
    //    {
    //        r.x = 30f;
    //        r.width = EditorGUIUtility.currentViewWidth - 95f;
    //        EditorGUI.LabelField(r, "Behaviors");
    //        r.x = EditorGUIUtility.currentViewWidth - 65f;
    //        r.width = 60f;
    //        EditorGUI.LabelField(r, "Weights");
    //        r.y += EditorGUIUtility.singleLineHeight * 1.2f;

    //        EditorGUI.BeginChangeCheck();
    //        for (int i = 0; i < cb.Behaviors.Length; i++)
    //        {
    //            r.x = 5f;
    //            r.width = 20f;
    //            EditorGUI.LabelField(r, i.ToString());
    //            r.x = 30f;
    //            r.width = EditorGUIUtility.currentViewWidth - 95f;
    //            cb.Behaviors[i] = (FlockBehavior)EditorGUI.ObjectField(r, cb.Behaviors[i], typeof(FlockBehavior), false);
    //            r.x = EditorGUIUtility.currentViewWidth - 65f;
    //            r.width = 60f;
    //            cb.Weights[i] = EditorGUI.FloatField(r, cb.Weights[i]);
    //            r.y += EditorGUIUtility.singleLineHeight * 1.1f;
    //        }
    //        if (EditorGUI.EndChangeCheck())
    //        {
    //            EditorUtility.SetDirty(cb);
    //        }
    //    }

    //    EditorGUILayout.EndHorizontal();

    //    EditorGUILayout.BeginHorizontal();
    //    if (GUILayout.Button("Add Behavior"))
    //    {
    //        AddBehavior(cb);
    //        EditorUtility.SetDirty(cb);
    //    }

    //    GUI.enabled = (cb.Behaviors != null && cb.Behaviors.Length > 0);

    //    if (cb.Behaviors != null && cb.Behaviors.Length > 0)
    //    {
    //        if (GUILayout.Button("Remove Behavior"))
    //        {
    //            RemoveBehavior(cb);
    //            EditorUtility.SetDirty(cb);
    //        }
    //    }

    //    GUI.enabled = true;

    //    EditorGUILayout.EndHorizontal();
    //}

    void AddBehavior(CompositeBehavior cb)
    {
        int oldCount = (cb.behaviors != null) ? cb.behaviors.Length : 0;
        FlockBehavior[] newBehaviors = new FlockBehavior[oldCount + 1];
        float[] newWeights = new float[oldCount + 1];
        for (int i = 0; i < oldCount; i++)
        {
            newBehaviors[i] = cb.behaviors[i];
            newWeights[i] = cb.weights[i];
        }
        newWeights[oldCount] = 1f;
        cb.behaviors = newBehaviors;
        cb.weights = newWeights;
    }

    void RemoveBehavior(CompositeBehavior cb)
    {
        int oldCount = cb.behaviors.Length;
        if (oldCount == 1)
        {
            cb.behaviors = null;
            cb.weights = null;
            return;
        }

        FlockBehavior[] newBehaviors = new FlockBehavior[oldCount - 1];
        float[] newWeights = new float[oldCount - 1];

        for (int i = 0; i < oldCount - 1; i++)
        {
            newBehaviors[i] = cb.behaviors[i];
            newWeights[i] = cb.weights[i];
        }

        cb.behaviors = newBehaviors;
        cb.weights = newWeights;

        Debug.Log("Behavior Removed");
    }
}