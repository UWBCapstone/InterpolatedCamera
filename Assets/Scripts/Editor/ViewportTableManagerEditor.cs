using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace InterpolatedCamera
{
    [CustomEditor(typeof(ViewportTableManager))]
    public class ViewportTableManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ViewportTableManager vtm = (ViewportTableManager)target;
            if (GUILayout.Button("Build Object"))
            {
                vtm.MakeTable();
            }
        }
    }
}
#endif