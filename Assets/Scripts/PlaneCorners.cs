using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterpolatedCamera
{
    public struct PlaneCorners
    {
        public Vector3 LowerLeft;
        public Vector3 UpperLeft;
        public Vector3 UpperRight;
        public Vector3 LowerRight;

        public Vector3 Center;

        public PlaneCorners(
            Vector3 lowerLeft,
            Vector3 upperLeft,
            Vector3 upperRight,
            Vector3 lowerRight)
        {
            LowerLeft = lowerLeft;
            UpperLeft = upperLeft;
            UpperRight = upperRight;
            LowerRight = lowerRight;
            Center = lowerLeft + ((upperRight - lowerLeft) / 2.0f);
        }
    }
}