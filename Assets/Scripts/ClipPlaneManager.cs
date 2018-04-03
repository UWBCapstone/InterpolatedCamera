using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterpolatedCamera
{
    /// <summary>
    /// Generates the basic UV basis from a rendered image. The Calc methods 
    /// set the basic uv comparison points for (0,0), (0,1), (1,0) and (1,1).
    /// The Intersect method takes in an arbitrary ray and determines where
    /// it hits on the plane.
    /// </summary>
    public class ClipPlaneManager : MonoBehaviour
    {
        public PlaneRect clipPlane;
        public Vector3 pos;
        public Vector3 up;
        public Vector3 forward;
        public Vector3 right;
        public float deg;
        public float dis;
        public float hypotenuse;
        public float opposite;
        public float nearClipDis;
        public float aspect;

        public ClipPlaneManager() { }

        // Pass in the render texture camera
        public ClipPlaneManager(Camera cam)
        {
            //Debug.Log("Cam name = " + cam.name);

            pos = cam.transform.position;
            up = cam.transform.up;
            forward = cam.transform.forward;
            right = cam.transform.right;

            deg = cam.fieldOfView / 2;
            dis = cam.farClipPlane;
            hypotenuse = dis / Mathf.Cos(Mathf.Deg2Rad * deg);
            opposite = Mathf.Sin(Mathf.Deg2Rad * deg) * hypotenuse;
            nearClipDis = cam.nearClipPlane;
            aspect = cam.aspect;

            //Debug.Log("Right vector is " + right);
            //Debug.Log("Up vector is " + up);
            //Debug.Log("Opposite is " + opposite);
            //Debug.Log("Degree is " + deg);
            //Debug.Log("Distance is " + dis);
            //Debug.Log("Hypotenuse is " + hypotenuse);

            Debug.Log("ClipPlane Corner00 = " + calc00());
            Debug.Log("ClipPlane Corner01 = " + calc01());
            Debug.Log("ClipPlane Corner10 = " + calc10());
            Debug.Log("ClipPlane Corner11 = " + calc11());

            clipPlane = new PlaneRect(calc00(), calc11(), -forward, false);
        }

        private Vector3 calc00()
        {
            //return pos - right.normalized * opposite - up.normalized * opposite + forward * dis;
            return pos - right.normalized * opposite * aspect - up.normalized * opposite + forward * dis;
        }

        private Vector3 calc01()
        {
            return pos - right.normalized * opposite * aspect + up.normalized * opposite + forward * dis;
        }

        private Vector3 calc10()
        {
            return pos + right.normalized * opposite * aspect - up.normalized * opposite + forward * dis;
        }

        private Vector3 calc11()
        {
            return pos + right.normalized * opposite * aspect + up.normalized * opposite + forward * dis;
        }

        public RaycastHit Intersect(Ray ray, Vector3 origin)
        {
            return clipPlane.Intersect(ray, origin);
        }

        public void UpdateInfo(Camera cam)
        {
            pos = cam.transform.position;
            up = cam.transform.up;
            forward = cam.transform.forward;
            right = cam.transform.right;

            deg = cam.fieldOfView / 2;
            dis = cam.farClipPlane;
            hypotenuse = dis / Mathf.Cos(Mathf.Deg2Rad * deg);
            opposite = Mathf.Sin(Mathf.Deg2Rad * deg) * hypotenuse;
            nearClipDis = cam.nearClipPlane;
            aspect = cam.aspect;

            //Debug.Log("Right vector is " + right);
            //Debug.Log("Up vector is " + up);
            //Debug.Log("Opposite is " + opposite);
            //Debug.Log("Degree is " + deg);
            //Debug.Log("Distance is " + dis);
            //Debug.Log("Hypotenuse is " + hypotenuse);

            clipPlane = new PlaneRect(calc00(), calc11(), -forward, false);
        }
        
        public static List<ClipPlaneManager> SortClipPlanes(ClipPlaneManager[] clipPlanes)
        {
            List<ClipPlaneManager> planeList = new List<ClipPlaneManager>();
            foreach (ClipPlaneManager clipPlane in clipPlanes)
            {
                planeList.Add(clipPlane);
            }

            planeList.Sort(ClipPlaneXComparer.SortByX());

            return planeList;
        }

        #region Properties
        public PlaneRect ClipRect
        {
            get
            {
                return clipPlane;
            }
        }
        #endregion
    }
}