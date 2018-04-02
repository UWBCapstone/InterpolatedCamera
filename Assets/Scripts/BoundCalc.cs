//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace CloakingBox {
//    public enum CornerNames
//    {
//        BottomLeft,
//        BottomRight,
//        TopLeft,
//        TopRight
//    }

//    public class BoundCalc : MonoBehaviour {
//        public Vector3 MyExtents;

//        public void Awake()
//        {
//            MyExtents = BoundCalc.Extents(gameObject);
//        }

//        public void Update()
//        {
//            MyExtents = BoundCalc.Extents(gameObject);
//        }

//        /// <summary>
//        /// Get the bottom left corner of a front-facing plane. (i.e. no rotation)
//        /// </summary>
//        /// <param name="go"></param>
//        /// <returns></returns>
//        public static Vector3 Corner00Flat(GameObject go)
//        {
//            Vector3 cent = Center(go);
//            Vector3 extents = Extents(go);
//            Vector3 P00 =
//                new Vector3(
//                    cent.x - extents.x,
//                    cent.y - extents.y,
//                    cent.z);
//            return P00;
//        }

//        /// <summary>
//        /// Get the bottom left corner of a rotated plane. Can only have been 
//        /// rotated along the x or y axis, and assumes it will be working with
//        /// a box for the cloaking box.
//        /// </summary>
//        /// <param name="go"></param>
//        /// <param name="faceName"></param>
//        /// <returns></returns>
//        public static Vector3 BoxCorner00(GameObject go, BoxFaceNames faceName)
//        {
//            Vector3 cent = Center(go);
//            Vector3 extents = Extents(go);

//            //Vector3 Dir00 =
//            //    new Vector3(
//            //        -extents.x,
//            //        -extents.y,
//            //        0);

//            Vector3 Dir00 = new Vector3(extents.x, -extents.y, 0);
//            if(faceName == BoxFaceNames.Top
//                || faceName == BoxFaceNames.Bottom)
//            {
//                Dir00 = new Vector3(extents.x, extents.y, 0);
//            }
            
//            // If the object was rotated around the x or y values, rotate the
//            // direction vector from the center to the desired corner around the
//            // angle (determined from the face chosen), and add that to the center
//            // position of the extents.
//            Dir00 = GetRotatedDirection(faceName, Dir00);
//            Vector3 P00 = cent + Dir00;

//            return P00;
//        }
    
//    /// <summary>
//    /// Get the top left corner of a front-facing plane. (i.e. no rotation)
//    /// </summary>
//    /// <param name="go"></param>
//    /// <returns></returns>
//        public static Vector3 Corner01Flat(GameObject go)
//        {
//            Vector3 cent = Center(go);
//            Vector3 extents = Extents(go);
//            Vector3 P01 =
//                new Vector3(
//                    cent.x - extents.x,
//                    cent.y + extents.y,
//                    cent.z);
//            return P01;
//        }

//        /// <summary>
//        /// Get the top left corner of a rotated plane. Can only have been 
//        /// rotated along the x or y axis, and assumes it will be working with
//        /// a box for the cloaking box.
//        /// </summary>
//        /// <param name="go"></param>
//        /// <param name="faceName"></param>
//        /// <returns></returns>
//        public static Vector3 BoxCorner01(GameObject go, BoxFaceNames faceName)
//        {
//            Vector3 cent = Center(go);
//            Vector3 extents = Extents(go);

//            //Vector3 Dir01 =
//            //    new Vector3(
//            //        -extents.x,
//            //        extents.y,
//            //        0);

//            Vector3 Dir01 = new Vector3(extents.x, extents.y, 0);
//            if(faceName == BoxFaceNames.Top
//                || faceName == BoxFaceNames.Bottom)
//            {
//                Dir01 = new Vector3(extents.x, -extents.y, 0);
//            }

//            // If the object was rotated around the x or y values, rotate the
//            // direction vector from the center to the desired corner around the
//            // angle (determined from the face chosen), and add that to the center
//            // position of the extents.
//            Dir01 = GetRotatedDirection(faceName, Dir01);
//            Vector3 P01 = cent + Dir01;

//            return P01;
//        }

//        /// <summary>
//        /// Get the bottom right corner of a front-facing plane. (i.e. no rotation)
//        /// </summary>
//        /// <param name="go"></param>
//        /// <returns></returns>
//        public static Vector3 Corner10Flat(GameObject go)
//        {
//            Vector3 cent = Center(go);
//            Vector3 extents = Extents(go);
//            Vector3 P00 =
//                new Vector3(
//                    cent.x + extents.x,
//                    cent.y - extents.y,
//                    cent.z);
//            return P00;
//        }

//        /// <summary>
//        /// Get the bottom right corner of a rotated plane. Can only have been 
//        /// rotated along the x or y axis, and assumes it will be working with
//        /// a box for the cloaking box.
//        /// </summary>
//        /// <param name="go"></param>
//        /// <param name="faceName"></param>
//        /// <returns></returns>
//        public static Vector3 BoxCorner10(GameObject go, BoxFaceNames faceName)
//        {
//            Vector3 cent = Center(go);
//            Vector3 extents = Extents(go);

//            //Vector3 Dir10 =
//            //    new Vector3(
//            //        extents.x,
//            //        -extents.y,
//            //        0);

//            Vector3 Dir10 = new Vector3(-extents.x, -extents.y, 0);
//            if(faceName == BoxFaceNames.Top
//                || faceName == BoxFaceNames.Bottom)
//            {
//                Dir10 = new Vector3(-extents.x, extents.y, 0);
//            }

//            // If the object was rotated around the x or y values, rotate the
//            // direction vector from the center to the desired corner around the
//            // angle (determined from the face chosen), and add that to the center
//            // position of the extents.
//            Dir10 = GetRotatedDirection(faceName, Dir10);
//            Vector3 P10 = cent + Dir10;

//            return P10;
//        }

//        /// <summary>
//        /// Get the top right corner of a front-facing plane. (i.e. no rotation)
//        /// </summary>
//        /// <param name="go"></param>
//        /// <returns></returns>
//        public static Vector3 Corner11Flat(GameObject go)
//        {
//            Vector3 cent = Center(go);
//            Vector3 extents = Extents(go);
//            Vector3 P00 =
//                new Vector3(
//                    cent.x + extents.x,
//                    cent.y + extents.y,
//                    cent.z);
//            return P00;
//        }

//        /// <summary>
//        /// Get the top right corner of a rotated plane. Can only have been 
//        /// rotated along the x or y axis, and assumes it will be working with
//        /// a box for the cloaking box.
//        /// </summary>
//        /// <param name="go"></param>
//        /// <param name="faceName"></param>
//        /// <returns></returns>
//        public static Vector3 BoxCorner11(GameObject go, BoxFaceNames faceName)
//        {
//            Vector3 cent = Center(go);
//            Vector3 extents = Extents(go);

//            //Vector3 Dir11 =
//            //    new Vector3(
//            //        extents.x,
//            //        extents.y,
//            //        0);

//            Vector3 Dir11 = new Vector3(-extents.x, extents.y, 0);
//            if(faceName == BoxFaceNames.Top
//                || faceName == BoxFaceNames.Bottom)
//            {
//                Dir11 = new Vector3(-extents.x, -extents.y, 0);
//            }

//            // If the object was rotated around the x or y values, rotate the
//            // direction vector from the center to the desired corner around the
//            // angle (determined from the face chosen), and add that to the center
//            // position of the extents.
//            Dir11 = GetRotatedDirection(faceName, Dir11);
//            Vector3 P11 = cent + Dir11;

//            return P11;
//        }

//        public static Vector3 Center(GameObject go)
//        {
//            if (go != null)
//            {
//                if (go.GetComponent<MeshFilter>() == null)
//                {
//                    return go.transform.position;
//                }
//                else
//                {
//                    Mesh mesh = go.GetComponent<MeshFilter>().mesh;
//                    Bounds bounds = mesh.bounds;
//                    return bounds.center + go.transform.position;
//                }
//            }
//            else
//            {
//                return Vector3.zero;
//            }
//        }

//        public static Vector3 Extents(GameObject go)
//        {
//            if (go != null)
//            {
//                if (go.GetComponent<MeshFilter>() == null)
//                {
//                    return Vector3.one * 0.01f;
//                }
//                else
//                {
//                    Mesh mesh = go.GetComponent<MeshFilter>().mesh;
//                    Bounds bounds = mesh.bounds;
//                    Vector3 actualExtents = bounds.extents;
//                    actualExtents.x *= go.transform.localScale.x;
//                    actualExtents.y *= go.transform.localScale.y;
//                    actualExtents.z *= go.transform.localScale.z;
//                    //Debug.Log("Extents = " + actualExtents);
//                    return actualExtents;
//                }
//            }
//            else
//            {
//                return Vector3.zero;
//            }
//        }

//        public static Quaternion GetQuaternionForDirectionRotation(BoxFaceNames faceName)
//        {
//            float rotationAngle = 0;
//            Vector3 axis = Vector3.zero;

//            switch (faceName)
//            {
//                case BoxFaceNames.Front:
//                    rotationAngle = 180f;
//                    axis = new Vector3(0, 1, 0);
//                    break;
//                case BoxFaceNames.Back:
//                    rotationAngle = 0f;
//                    axis = new Vector3(0, 1, 0);
//                    break;
//                case BoxFaceNames.Left:
//                    rotationAngle = -90f;
//                    axis = new Vector3(0, 1, 0);
//                    break;
//                case BoxFaceNames.Right:
//                    rotationAngle = 90f;
//                    axis = new Vector3(0, 1, 0);
//                    break;
//                case BoxFaceNames.Top:
//                    rotationAngle = 90f;
//                    axis = new Vector3(1, 0, 0);
//                    // rotationAngle2 = 150f;
//                    // axis2 = new Vector3(0, 1, 0);
//                    // rotationAngle3 = 30f;
//                    // axis3 = new Vector3(0, 0, 1);
//                    break;
//                case BoxFaceNames.Bottom:
//                    rotationAngle = -90f;
//                    axis = new Vector3(1, 0, 0);
//                    break;
//            }

//            Quaternion result = Quaternion.AngleAxis(rotationAngle, axis);

//            if (faceName == BoxFaceNames.Top
//                || faceName == BoxFaceNames.Bottom)
//            {
//                float rotationAngle2 = 180f;
//                Vector3 axis2 = new Vector3(0, 0, 1);
//                Quaternion rotation2 = Quaternion.AngleAxis(rotationAngle2, axis2);

//                //float rotationAngle3 = 30f;
//                //Vector3 axis3 = new Vector3(0, 0, 1);
//                //Quaternion rotation3 = Quaternion.AngleAxis(rotationAngle3, axis3);

//                //result = result * rotation2 * rotation3;
//                result = result * rotation2;
//            }

//            return result;
//        }

//        public static Vector3 GetRotatedDirection(BoxFaceNames faceName, Vector3 dir)
//        {
//            Quaternion rot = GetQuaternionForDirectionRotation(faceName);
//            Vector3 newDir = rot * dir;
//            return newDir;
//        }
//    }
//}