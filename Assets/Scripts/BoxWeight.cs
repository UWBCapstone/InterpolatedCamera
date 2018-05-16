using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterpolatedCamera
{
    public class BoxWeight
    {
        public float Weight;
        public float MinX;
        public float MaxX;
        public float MinY;
        public float MaxY;
        public GameObject ViewingCamera;

        public BoxWeight(
            float weight,
            float xMin,
            float xMax,
            float yMin,
            float yMax,
            GameObject viewingCamera)
        {
            Weight = weight;
            MinX = xMin;
            MaxX = xMax;
            MinY = yMin;
            MaxY = yMax;
            ViewingCamera = viewingCamera;
        }
    }
}