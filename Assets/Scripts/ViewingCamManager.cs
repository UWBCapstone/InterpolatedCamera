using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterpolatedCamera
{
    public class ViewingCamManager : MonoBehaviour
    {
        public WebCamManager webCamManager;
        public WebcamDeviceNames[] IgnorableWebCams = new WebcamDeviceNames[3];
        public GameObject[] ViewingCameras;
        public ClipPlaneManager[] ClipPlanes;
        public Vector3[] CamStartingPositions = new Vector3[8]; // InterpolationManager.maxCameras
        
        public void Update()
        {
            if(ViewingCameras.Length != webCamManager.NumWebCams)
            {
                DestroyViewingCameras();
                InitViewingCameras();
            }

            for(int i = 0; i < ViewingCameras.Length; i++)
            {
                //UpdateViewingCameraClipPlane(i);
                GameObject viewingCam = ViewingCameras[i];
                UpdateViewingCameraClipPlane(viewingCam.GetComponent<ClipPlaneManager>(), viewingCam);
            }
        }

        public void InitViewingCameras()
        {
            ViewingCameras = new GameObject[webCamManager.NumWebCams];
            ClipPlanes = new ClipPlaneManager[ViewingCameras.Length];

            List<GameObject> viewingCameraList = new List<GameObject>();
            List<ClipPlaneManager> clipPlaneList = new List<ClipPlaneManager>();

            List<int> ignorableWebCamIndices = new List<int>();
            foreach(WebcamDeviceNames name in IgnorableWebCams)
            {
                ignorableWebCamIndices.AddRange(webCamManager.GetIndicesFor(name));
            }
            
            for (int i = 0; i < ViewingCameras.Length; i++)
            {
                if (ignorableWebCamIndices.Contains(i))
                {
                    continue;
                }

                //ViewingCameras[i] = GenerateViewingCamera(i);
                GameObject viewingCam = GenerateViewingCamera(i);
                ClipPlaneManager clipPlane = associateClipPlane(viewingCam);

                // ERROR TESTING - OFFSETTING THE CAMERA STARTING POSITION MESSES UP THE PLANERECT HORRENDOUSLY - WHY?
                viewingCam.transform.position = CamStartingPositions[i];
                clipPlane.ClipRect.Translate(CamStartingPositions[i]);

                viewingCameraList.Add(viewingCam);
                clipPlaneList.Add(clipPlane);

                //// ERROR TESTING - OFFSETTING THE CAMERA STARTING POSITION MESSES UP THE PLANERECT HORRENDOUSLY - WHY?
                //ViewingCameras[i].transform.position = CamStartingPositions[i];
                //ClipPlanes[i].ClipRect.Translate(CamStartingPositions[i]);
                ////ClipPlanes[i].transform.position = CamStartingPositions[i];

                //Debug.Log("Generated clip plane Corner00 = " + ClipPlanes[i].clipPlane.Corner00);
                //Debug.Log("Generated clip plane Corner01 = " + ClipPlanes[i].clipPlane.Corner01);
                //Debug.Log("Generated clip plane Corner11 = " + ClipPlanes[i].clipPlane.Corner11);
                //Debug.Log("Generated clip plane Corner10 = " + ClipPlanes[i].clipPlane.Corner10);
            }

            //ViewingCameras = viewingCameraList.ToArray();
            //ClipPlanes = clipPlaneList.ToArray();
            ViewingCameras = new GameObject[1];
            ViewingCameras[0] = viewingCameraList[0];
            ClipPlanes = clipPlaneList.ToArray();
        }

        public GameObject GenerateViewingCamera(int camIndex)
        {
            GameObject viewingCam = new GameObject();
            Camera cam = viewingCam.AddComponent<Camera>();
            viewingCam.AddComponent<GUILayer>();
            viewingCam.AddComponent<FlareLayer>();
            // Avoid adding an AudioListener, this messes with Unity
            //viewingCam.AddComponent<AudioListener>();
            viewingCam.name = "ViewingCamera_";
            viewingCam.tag = TagManager.GetViewingCameraTag();

            // Associate a webcam
            if (webCamManager.WebCams.Length > camIndex)
            {
                WebCamDevice webCamDevice = webCamManager.WebCams[camIndex];
                WebCamTexture webCamTexture = null;
                if(webCamManager.VideoFeeds.Length > camIndex)
                {
                    webCamTexture = webCamManager.VideoFeeds[camIndex];
                }

                WebcamDeviceNames webCamDeviceName = WebCamSpecsManager.WebCamDeviceToSpecsName(webCamDevice);
                WebCamSpecs specs = WebCamSpecsManager.GetSpecs(webCamDeviceName);

                // Assign attributes from webcam
                WebCamSpecsManager.AssignSpecsToCamera(cam, specs);
                WebCamIdentifier webcamID = viewingCam.AddComponent<WebCamIdentifier>();
                webcamID.SetInfo(webCamManager, camIndex);

                viewingCam.name = "ViewingCamera_" + "(" + camIndex.ToString() + ")";
            }

            cam.depth = -2;

            return viewingCam;
        }

        public void DestroyViewingCameras()
        {
            for(int i = 0; i < ViewingCameras.Length; i++)
            {
                GameObject.Destroy(ViewingCameras[i]);
            }
            ViewingCameras = new GameObject[0];
        }

        /// <summary>
        /// Must be called after initializing the viewing cameras.
        /// </summary>
        /// <param name="camIndex"></param>
        private ClipPlaneManager associateClipPlane(GameObject viewingCam)
        {
            //ClipPlaneManager clipPlane = ViewingCameras[camIndex].AddComponent<ClipPlaneManager>();
            //ClipPlanes[camIndex] = clipPlane;
            ClipPlaneManager clipPlane = viewingCam.AddComponent<ClipPlaneManager>();
            //UpdateViewingCameraClipPlane(camIndex);
            UpdateViewingCameraClipPlane(clipPlane, viewingCam);
            
            // Add miscellaneous scripts
            UVCalc uvCalc = viewingCam.AddComponent<UVCalc>();
            uvCalc.SetRepresentativePlane(viewingCam.GetComponent<Camera>());

            return clipPlane;
        }

        public void UpdateViewingCameraClipPlane(ClipPlaneManager clipPlane, GameObject viewingCam)
        {
            Camera cam = viewingCam.GetComponent<Camera>();
            if(cam != null)
            {
                clipPlane.UpdateInfo(cam);
            }
        }

        public void UpdateViewingCameraClipPlane(int camIndex)
        {
            if (ClipPlanes != null
                && ClipPlanes.Length > camIndex)
            {
                ClipPlaneManager clipPlane = ClipPlanes[camIndex];
                if (ViewingCameras != null
                    && ViewingCameras.Length > camIndex)
                {
                    Camera cam = ViewingCameras[camIndex].GetComponent<Camera>();
                    if (cam != null)
                    {
                        clipPlane.UpdateInfo(cam);
                    }
                }
            }
        }
    }
}