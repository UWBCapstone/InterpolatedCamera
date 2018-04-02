using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace InterpolatedCamera
{
#if UNITY_EDITOR
    [InitializeOnLoad]
    public class Startup
    {
        static Startup()
        {
            Debug.Log("Project Booting...");

            AddTags();
            AddLayers();
        }

        private static void AddTags()
        {
            string[] tags =
            {
                // Add tags as appropriate / needed here
            };

            foreach(string tag in tags)
            {
                TagsAndLayers.AddTag(tag);
            }

            List<string> tagList = TagManager.GetTagList();
            foreach(string tag in tagList)
            {
                TagsAndLayers.AddTag(tag);
            }
        }

        private static void AddLayers()
        {
            string[] layers =
            {
                // Add layers as appropriate / needed here
            };
            
            foreach(string layer in layers)
            {
                TagsAndLayers.AddLayer(layer);
            }
        }
    }
#endif
}