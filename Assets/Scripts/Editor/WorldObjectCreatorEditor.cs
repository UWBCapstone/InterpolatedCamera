using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace InterpolatedCamera
{
    [CustomEditor(typeof(WorldObjectCreator))]
    public class WorldObjectCreatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            WorldObjectCreator myCreator = (WorldObjectCreator)target;
            if (GUILayout.Button("Build Object"))
            {
                myCreator.CreateWorldObject();
            }
        }
    }
}
#endif