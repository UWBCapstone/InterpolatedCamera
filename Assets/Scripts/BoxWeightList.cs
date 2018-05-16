using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterpolatedCamera
{
    public class BoxWeightList : MonoBehaviour
    {
        public GameObject ViewingCamera;
        private KeyValuePair<float, float> xRange_m;
        private KeyValuePair<float, float> yRange_m;
        public List<BoxWeight> boxWeights;
        private KeyValuePair<int, int> splits_m = new KeyValuePair<int, int>(4, 4); // Has to be hard-coded due to the associated shader's assumption of a 16-length box filter

        public void Start()
        {
            boxWeights = new List<BoxWeight>();
            // Be sure that SetViewingCamera is called either by the parent or someone else.
        }
        
        public void SetViewingCamera(GameObject viewingCam)
        {
            ViewingCamera = viewingCam;
        }

        public void Awake()
        {
            setRanges();
            if(ViewingCamera == null)
            {
                Debug.LogError("Don't forget to set the ViewingCamera field in BoxWeightList!");
            }
        }

        private void setRanges()
        {
            var clipPlane = ViewingCamera.GetComponent<ClipPlaneManager>().clipPlane;
            xRange_m = new KeyValuePair<float, float>(clipPlane.Corner00.x, clipPlane.Corner11.x);
            yRange_m = new KeyValuePair<float, float>(clipPlane.Corner00.y, clipPlane.Corner11.y);
        }

        public bool UpdateBoxWeights(List<float> weights)
        {
            int expectedWeightCount = splits_m.Key * splits_m.Value;
            if(weights.Count != expectedWeightCount)
            {
                Debug.LogError("Invalid weight list passed. Shader has to assume a preset weight amount. Expected weight count = " + expectedWeightCount);
            }

            if (boxWeights != null)
            {
                for (int i = 0; i < boxWeights.Count; i++)
                {
                    boxWeights[i].Weight = weights[i];
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Clean()
        {
            if(boxWeights != null)
            {
                // Check to see if the list is of the appropriate size
                if(boxWeights.Count != (splits_m.Key * splits_m.Value))
                {
                    boxWeights.Clear();
                    // Split the x Range by the x splits
                    // Calculate the x values to use
                    // Split the y Range by the y splits
                    // Calculate the y values to use

                    for(int xIndex = 0; xIndex < splits_m.Key; xIndex++)
                    {
                        for(int yIndex = 0; yIndex < splits_m.Value; yIndex++)
                        {
                            float xDiff = (xRange_m.Value - xRange_m.Key) / splits_m.Key;
                            float xMin = xRange_m.Key + xDiff * xIndex;
                            float xMax = xMin + xDiff;
                            float yDiff = (yRange_m.Value - yRange_m.Key) / splits_m.Key;
                            float yMin = yRange_m.Key + yDiff * yIndex;
                            float yMax = yMin + yDiff;
                            BoxWeight bw = new BoxWeight(0, xMin, xMax, yMin, yMax, ViewingCamera);
                        }
                    }
                }
            }
        }

        #region Properties
        public float xMin
        {
            get
            {
                return xRange_m.Key;
            }
        }
        public float xMax
        {
            get
            {
                return xRange_m.Value;
            }
        }
        public float yMin
        {
            get
            {
                return yRange_m.Key;
            }
        }
        public float yMax
        {
            get
            {
                return yRange_m.Value;
            }
        }
        public int xCount
        {
            get
            {
                return splits_m.Key;
            }
        }
        public int yCount
        {
            get
            {
                return splits_m.Value;
            }
        }
        public int Size
        {
            get
            {
                return boxWeights.Count;
            }
        }
        #endregion
    }
}