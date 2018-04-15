﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterpolatedCamera
{
    public class UVCalc : MonoBehaviour
    {
        public struct UVBounds
        {
            public Vector2 LowerLeft;
            public Vector2 UpperLeft;
            public Vector2 UpperRight;
            public Vector2 LowerRight;
        };

        //public double ScaleFactor = 1.0;

        public Vector2 LowerLeft;
        public Vector2 LowerRight;
        public Vector2 UpperLeft;
        public Vector2 UpperRight;

        // public GameObject MainCamera;
        //public List<GameObject> CloakingBoxes;

        private ClipPlaneManager representativePlane;

        public void Awake()
        {
            LowerLeft = Vector2.zero;
            LowerRight = Vector2.zero;
            UpperLeft = Vector2.zero;
            UpperRight = Vector2.zero;

            representativePlane = new ClipPlaneManager();
        }

        public void Update()
        {

        }

        public void SetRepresentativePlane(Camera cam)
        {
            GameObject tempCam = new GameObject();
            tempCam.SetActive(false);
            Camera representativeCam = tempCam.AddComponent<Camera>();
            representativeCam.fieldOfView = 175.0f;
            representativeCam.farClipPlane = cam.farClipPlane;
            representativeCam.nearClipPlane = cam.nearClipPlane;
            representativeCam.transform.position = cam.transform.position;
            representativeCam.transform.forward = cam.transform.forward;
            representativeCam.transform.up = cam.transform.up;
            representativeCam.transform.right = cam.transform.right;

            //representativePlane = new ClipPlaneManager(cam);
            representativePlane = new ClipPlaneManager(representativeCam);

            GameObject.Destroy(tempCam);
        }

        public static Vector2 UV(PlaneRect canvas, Vector3 pointOnCanvas)
        {
            Vector3 rightComp = pointOnCanvas - canvas.Corner00;
            float dot = Vector3.Dot(rightComp, canvas.Right);
            float u = dot / canvas.width;
            Vector3 upComp = pointOnCanvas - canvas.Corner00;
            dot = Vector3.Dot(upComp, canvas.Up);
            float v = dot / canvas.height;

            return new Vector2(u, v);
        }

        /// <summary>
        /// Check for UV value for something that would hit on the plane if it was extended out.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="origin"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector2 UVOffCanvas(Vector3 origin, Vector3 point)
        {
            Vector3 dir = point - origin;
            Ray ray = new Ray(origin, dir);
            RaycastHit rh = representativePlane.Intersect(ray, origin);
            
            ClipPlaneManager clipPlane = gameObject.GetComponent<ClipPlaneManager>();

            //Debug.Log("rh.point = " + rh.point);
            //Debug.Log("UV00 = " + UV(clipPlane.ClipRect, rh.point));

            return UV(clipPlane.ClipRect, rh.point);
        }

        public RaycastHit IntersectRepresentativePlane(Ray ray, Vector3 origin)
        {
            return representativePlane.Intersect(ray, origin);
        }

        ///// <summary>
        ///// Associates UV coordinates with corners of the box face.
        ///// </summary>
        ///// <param name="boxFace"></param>
        ///// <returns></returns>
        //public UVBounds ClipUV(GameObject boxFace)
        //{
        //    ClipPlaneManager clipPlane = new ClipPlaneManager(RenderTextureCamera.GetComponent<Camera>());


        //    Vector3 mainCamOrigin = MainCamera.transform.position;
        //    //Vector3 mainCamOrigin = MainCamera.transform.position + MainCamera.GetComponent<Camera>().nearClipPlane * MainCamera.transform.position;
        //    //GameObject boxFace = RenderTextureCamera.transform.parent.gameObject;

        //    //mainCamOrigin = AdjustViewpoint();


        //    //Ray r00 = Intersector.R00(boxFace, mainCamOrigin);
        //    //Ray r01 = Intersector.R01(boxFace, mainCamOrigin);
        //    //Ray r10 = Intersector.R10(boxFace, mainCamOrigin);
        //    //Ray r11 = Intersector.R11(boxFace, mainCamOrigin);
        //    BoxFaceNames faceName = CloakingBoxCreator.GetCloakingBoxFaceName(boxFace);
        //    Ray r00 = Intersector.BoxR00(boxFace, mainCamOrigin, faceName);
        //    Ray r01 = Intersector.BoxR01(boxFace, mainCamOrigin, faceName);
        //    Ray r10 = Intersector.BoxR10(boxFace, mainCamOrigin, faceName);
        //    Ray r11 = Intersector.BoxR11(boxFace, mainCamOrigin, faceName);

        //    //Debug.Log("Ray for point 00 = " + r00);

        //    RaycastHit rh00 = clipPlane.Intersect(r00, mainCamOrigin);
        //    RaycastHit rh01 = clipPlane.Intersect(r01, mainCamOrigin);
        //    RaycastHit rh10 = clipPlane.Intersect(r10, mainCamOrigin);
        //    RaycastHit rh11 = clipPlane.Intersect(r11, mainCamOrigin);

        //    //Debug.Log("Raycasthit (r00) distance = " + rh00.distance);
        //    //Debug.Log("Raycasthit (r01) distance = " + rh01.distance);
        //    //Debug.Log("Raycasthit (r10) distance = " + rh10.distance);
        //    //Debug.Log("Raycasthit (r11) distance = " + rh11.distance);

        //    UVBounds bounds = new UVBounds()
        //    {
        //        LowerLeft = UV(clipPlane.ClipRect, rh00.point),
        //        UpperLeft = UV(clipPlane.ClipRect, rh01.point),
        //        UpperRight = UV(clipPlane.ClipRect, rh11.point),
        //        LowerRight = UV(clipPlane.ClipRect, rh10.point)
        //    };

        //    //Debug.Log("00 point = " + rh00.point);
        //    //Debug.Log("01 point = " + rh01.point);
        //    //Debug.Log("10 point = " + rh10.point);
        //    //Debug.Log("11 point = " + rh11.point);

        //    if (faceName == BoxFaceNames.Left)
        //    {
        //        Debug.DrawLine(mainCamOrigin, rh00.point, Color.red);
        //        Debug.DrawLine(mainCamOrigin, rh01.point, Color.green);
        //        Debug.DrawLine(mainCamOrigin, rh10.point, Color.blue);
        //        Debug.DrawLine(mainCamOrigin, rh11.point, Color.white);
        //    }
        //    return bounds;
        //}

        //public Vector3 AdjustViewpoint()
        //{
        //    Vector3 mainCamOrigin = MainCamera.transform.position;
        //    GameObject boxFace = RenderTextureCamera.transform.parent.gameObject;

        //    Vector3 delta = (boxFace.transform.position - mainCamOrigin) * ((float)(ScaleFactor - 1));
        //    Vector3 adjustedOrigin = mainCamOrigin + delta;

        //    //GameObject DebugSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    //DebugSphere.transform.Translate(adjustedOrigin);
        //    return adjustedOrigin;
        //}
    }
}