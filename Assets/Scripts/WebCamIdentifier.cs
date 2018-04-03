using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterpolatedCamera
{
    public class WebCamIdentifier : MonoBehaviour
    {
        public string DeviceNameString;
        public WebCamDevice WebCam;
        public WebCamTexture WebCamFeed;
        public WebCamManager webCamManager;
        public WebcamDeviceNames DeviceName;
        private int camIndex;

        /// <summary>
        /// Retrieves an instance of a WebCamManager and the camera index 
        /// to be used to fetch information about a camera.
        /// </summary>
        /// <param name="managerInstance"></param>
        /// <param name="camIndex"></param>
        public void SetInfo(WebCamManager managerInstance, int camIndex)
        {
            if (camIndex < managerInstance.NumWebCams)
            {
                webCamManager = managerInstance;
                this.camIndex = camIndex;
                DeviceNameString = managerInstance.WebCams[camIndex].name;
                WebCam = managerInstance.WebCams[camIndex];
                WebCamFeed = managerInstance.VideoFeeds[camIndex];
                DeviceName = WebCamSpecsManager.WebCamDeviceToSpecsName(WebCam);
            }
        }

        //public void FixedUpdate()
        //{
        //    if (camIndex < webCamManager.NumWebCams)
        //    {
        //        DeviceName = webCamManager.WebCams[camIndex].name;
        //        WebCam = webCamManager.WebCams[camIndex];
        //        WebCamFeed = webCamManager.VideoFeeds[camIndex];
        //    }
        //}
    }
}