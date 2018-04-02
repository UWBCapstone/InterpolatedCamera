using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterpolatedCamera
{
    public class ViewingCamManager : MonoBehaviour
    {
        public WebCamManager webCamManager;
        public GameObject[] ViewingCameras;
        public ClipPlaneManager[] ClipPlanes;
        
        public void Update()
        {
            if(ViewingCameras.Length != webCamManager.NumWebCams)
            {
                DestroyViewingCameras();
                InitViewingCameras();
            }

            for(int i = 0; i < ViewingCameras.Length; i++)
            {
                UpdateViewingCameraClipPlane(i);
            }
        }

        public void InitViewingCameras()
        {
            ViewingCameras = new GameObject[webCamManager.NumWebCams];
            ClipPlanes = new ClipPlaneManager[ViewingCameras.Length];

            for (int i = 0; i < ViewingCameras.Length; i++)
            {
                ViewingCameras[i] = GenerateViewingCamera(i);
                associateClipPlane(i);
            }

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
        private void associateClipPlane(int camIndex)
        {
            ClipPlaneManager clipPlane = ViewingCameras[camIndex].AddComponent<ClipPlaneManager>();
            UpdateViewingCameraClipPlane(camIndex);

            // Add miscellaneous scripts
            GameObject viewingCam = ViewingCameras[camIndex];
            UVCalc uvCalc = viewingCam.AddComponent<UVCalc>();
            uvCalc.SetRepresentativePlane(viewingCam.GetComponent<Camera>());
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