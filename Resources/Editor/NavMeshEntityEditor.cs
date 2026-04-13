using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NavMeshEntity))]

public class NavMeshEntityEditor : Editor
{
    private NavMeshEntity navMeshEntity  { get { return target as NavMeshEntity; } }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        /*
        if (GUILayout.Button("GenerateParticle"))
        {
            entity.GenerateParticle();
        }*/
    }
}
