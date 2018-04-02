using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public class Intersector : MonoBehaviour
    {
        /// <summary>
        /// Generate a ray from a viewpoint to the bottom left corner of a
        /// flat, forward-facing plane.
        /// </summary>
        /// <param name="go"></param>
        /// <param name="viewpoint"></param>
        /// <returns></returns>
        public static Ray R00(GameObject go, Vector3 viewpoint)
        {
            Vector3 dir = BoundCalc.Corner00Flat(go) - viewpoint;
            Ray R00 = new Ray(viewpoint, dir);
            return R00;
        }

        /// <summary>
        /// Generate a ray from a viewpoint to the bottom left corner of a
        /// flat, rotated plane (rotation along x or y axis only). Assumed to
        /// work with the cloaking box application specifically.
        /// </summary>
        /// <param name="go"></param>
        /// <param name="viewpoint"></param>
        /// <param name="faceName"></param>
        /// <returns></returns>
        public static Ray BoxR00(GameObject go, Vector3 viewpoint, BoxFaceNames faceName)
        {
            Vector3 dir = BoundCalc.BoxCorner00(go, faceName) - viewpoint;
            Ray R00 = new Ray(viewpoint, dir);
            return R00;
        }

        public static Ray R01(GameObject go, Vector3 viewpoint)
        {
            Vector3 dir = BoundCalc.Corner01Flat(go) - viewpoint;
            Ray R01 = new Ray(viewpoint, dir);
            return R01;
        }

        /// <summary>
        /// Generate a ray from a viewpoint to the top left corner of a
        /// flat, rotated plane (rotation along x or y axis only). Assumed to
        /// work with the cloaking box application specifically.
        /// </summary>
        /// <param name="go"></param>
        /// <param name="viewpoint"></param>
        /// <param name="faceName"></param>
        /// <returns></returns>
        public static Ray BoxR01(GameObject go, Vector3 viewpoint, BoxFaceNames faceName)
        {
            Vector3 dir = BoundCalc.BoxCorner01(go, faceName) - viewpoint;
            Ray R01 = new Ray(viewpoint, dir);
            return R01;
        }

        public static Ray R10(GameObject go, Vector3 viewpoint)
        {
            Vector3 dir = BoundCalc.Corner10Flat(go) - viewpoint;
            Ray R10 = new Ray(viewpoint, dir);
            return R10;
        }

        /// <summary>
        /// Generate a ray from a viewpoint to the bottom right corner of a
        /// flat, rotated plane (rotation along x or y axis only). Assumed to
        /// work with the cloaking box application specifically.
        /// </summary>
        /// <param name="go"></param>
        /// <param name="viewpoint"></param>
        /// <param name="faceName"></param>
        /// <returns></returns>
        public static Ray BoxR10(GameObject go, Vector3 viewpoint, BoxFaceNames faceName)
        {
            Vector3 dir = BoundCalc.BoxCorner10(go, faceName) - viewpoint;
            Ray R10 = new Ray(viewpoint, dir);
            return R10;
        }

        public static Ray R11(GameObject go, Vector3 viewpoint)
        {
            Vector3 dir = BoundCalc.Corner11Flat(go) - viewpoint;
            Ray R11 = new Ray(viewpoint, dir);
            return R11;
        }

        /// <summary>
        /// Generate a ray from a viewpoint to the top right corner of a
        /// flat, rotated plane (rotation along x or y axis only). Assumed to
        /// work with the cloaking box application specifically.
        /// </summary>
        /// <param name="go"></param>
        /// <param name="viewpoint"></param>
        /// <param name="faceName"></param>
        /// <returns></returns>
        public static Ray BoxR11(GameObject go, Vector3 viewpoint, BoxFaceNames faceName)
        {
            Vector3 dir = BoundCalc.BoxCorner11(go, faceName) - viewpoint;
            Ray R11 = new Ray(viewpoint, dir);
            return R11;
        }
    }
}