using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterpolatedCamera
{

    /// <summary>
    /// Associates a box with each texture that covers the entire texture. The filter is applied to the entire texture and calculated accordingly.
    /// </summary>
    public class BoxFilterManager : MonoBehaviour
    {
        [HideInInspector]
        public List<GameObject> ViewingCams;
        [HideInInspector]
        public Material AggregatePlaneMaterial;
        public ViewingCamManager viewingCamManager;
        public Dictionary<GameObject, BoxWeightList> boxWeightListDictionary;

        // Use this for initialization
        void Start()
        {
            ViewingCams = new List<GameObject>();
            // Create box weight lists for all of the viewing cameras
            boxWeightListDictionary = new Dictionary<GameObject, BoxWeightList>();
        }

        // Update is called once per frame
        void Update()
        {
            manageViewingCams();
        }

        private void manageViewingCams()
        {
            if (viewingCamManager.ViewingCameras.Length != ViewingCams.Count)
            {
                ViewingCams.Clear();
                ViewingCams.AddRange(viewingCamManager.ViewingCameras);
                RecalibrateBoxWeightLists();
            }
        }

        public void RecalibrateBoxWeightLists()
        {
            List<GameObject> keysToRemove = new List<GameObject>();
            var keys = boxWeightListDictionary.Keys;
            foreach(var key in keys)
            {
                if (!ViewingCams.Contains(key))
                {
                    keysToRemove.Add(key);
                }
            }

            foreach(var key in keysToRemove)
            {
                GameObject.Destroy(boxWeightListDictionary[key]); // Remove the component
                boxWeightListDictionary.Remove(key);
            }

            List<GameObject> keysToAdd = new List<GameObject>();
            foreach(var cam in ViewingCams)
            {
                if (!ViewingCams.Contains(cam))
                {
                    var bwl = AddBoxWeightList(cam);
                    boxWeightListDictionary.Add(cam, bwl);
                }
            }

            Debug.Log("Don't forget to update the values!");
        }

        public BoxWeightList AddBoxWeightList(GameObject viewingCam)
        {
            var bwl = gameObject.AddComponent<BoxWeightList>();
            bwl.SetViewingCamera(viewingCam);
            return bwl;
        }
        
        public void SetCornersForAllViewingCams()
        {

        }

        public void SetCorners(GameObject viewingCam)
        {

        }

        //public Vector4 WeightToVector4()
        //{

        //}

        public void CalculateWeights()
        {
            // 
        }
    }
}