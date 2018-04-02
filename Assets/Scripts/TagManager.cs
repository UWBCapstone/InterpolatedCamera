using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterpolatedCamera
{
    public enum InterpolatedCameraTags
    {
        ViewingCamera
    };

    public class TagManager : MonoBehaviour
    {
        public string ViewingCameraTag = "ViewingCamera";

        public static string ViewingCameraTag_s = "ViewingCamera";

        void Update()
        {
            if (!ViewingCameraTag_s.Equals(ViewingCameraTag))
            {
                ViewingCameraTag_s = ViewingCameraTag;
            }
        }

        public static string GetViewingCameraTag()
        {
            return ViewingCameraTag_s;
        }

        public static List<string> GetTagList()
        {
            List<string> tagNameList = new List<string>();
            tagNameList.Add(ViewingCameraTag_s);

            return tagNameList;
        }
    }
}