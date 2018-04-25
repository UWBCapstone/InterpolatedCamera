using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace InterpolatedCamera
{
    [CustomEditor(typeof(WorldTableManager))]
    public class WorldTableManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            WorldTableManager wtm = (WorldTableManager)target;
            if (GUILayout.Button("Build Object"))
            {
                wtm.MakeTable();
            }
        }
    }
}
#endif