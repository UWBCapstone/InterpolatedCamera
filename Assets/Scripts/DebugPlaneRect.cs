using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterpolatedCamera
{
    public class DebugPlaneRect : MonoBehaviour
    {
        public Vector3 LowerLeft;
        public Vector3 LowerRight;
        public Vector3 UpperLeft;
        public Vector3 UpperRight;

        private PlaneRect planeRect;

        public void SetCorners(Vector3 p00, Vector3 p01, Vector3 p11, Vector3 p10)
        {
            LowerLeft = p00;
            LowerRight = p10;
            UpperLeft = p01;
            UpperRight = p11;
        }

        public void Update()
        {
            if(planeRect != null)
            {
                LowerLeft = planeRect.Corner00;
                LowerRight = planeRect.Corner10;
                UpperLeft = planeRect.Corner01;
                UpperRight = planeRect.Corner11;
            }
        }

        public void AssociatePlaneRect(PlaneRect pr)
        {
            this.planeRect = pr;
        }

        public void Translate(Vector3 dir)
        {
            if(planeRect != null)
            {
                planeRect.Translate(dir);
            }
        }

        public Vector3 Center
        {
            get
            {
                return planeRect.center;
            }
        }
    }
}