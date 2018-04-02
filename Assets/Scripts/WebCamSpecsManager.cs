using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterpolatedCamera
{
    public enum WebcamDeviceNames
    {
        NULL,
        LOGITECH_C920
    };

    public struct WebCamSpecs
    {
        public float HorizontalFOV;
        public float VerticalFOV;
        public float NearClippingPlane;
        public float FarClippingPlane;
        public WebcamDeviceNames WebcamDeviceName;
        public string DeviceName;

        public WebCamSpecs(
            float horizontalFOV, 
            float verticalFOV, 
            float nearClippingPlane, 
            float farClippingPlane, 
            WebcamDeviceNames webcamDeviceName,
            string deviceName
            )
        {
            HorizontalFOV = horizontalFOV;
            VerticalFOV = verticalFOV;
            NearClippingPlane = nearClippingPlane;
            FarClippingPlane = farClippingPlane;
            WebcamDeviceName = webcamDeviceName;
            DeviceName = deviceName;
        }
    }
    
    public static class WebCamSpecsManager
    {
        static WebCamSpecsManager()
        {

        }

        public static WebCamSpecs GetSpecs(WebcamDeviceNames deviceName)
        {
            WebCamSpecs camSpecs = new WebCamSpecs();

            float HorizontalFOV;
            float VerticalFOV;
            float NearClippingPlane;
            float FarClippingPlane;
            WebcamDeviceNames WebcamDeviceName;
            string DeviceName;

            switch (deviceName)
            {
                case WebcamDeviceNames.LOGITECH_C920:
                    // http://therandomlab.blogspot.com/2013/03/logitech-c920-and-c910-fields-of-view.html
                    HorizontalFOV = 70.42f;
                    VerticalFOV = 43.30f;
                    NearClippingPlane = 0.3f;
                    FarClippingPlane = 1000;
                    WebcamDeviceName = WebcamDeviceNames.LOGITECH_C920;
                    DeviceName = WebcamDeviceName.ToString();

                    camSpecs = new WebCamSpecs(
                        HorizontalFOV,
                        VerticalFOV,
                        NearClippingPlane,
                        FarClippingPlane,
                        WebcamDeviceName,
                        DeviceName);

                    break;
                default:
                    camSpecs = new WebCamSpecs();
                    Debug.LogError("Webcam Device name not recognized. To add webcam device, please modify WebCamSpecs class.");
                    break;
            }

            return camSpecs;
        }

        public static WebcamDeviceNames WebCamDeviceToSpecsName(WebCamDevice webcam)
        {
            string deviceName = webcam.name;

            if (deviceName.ToLower().Contains("LogitechC920".ToLower()))
            {
                return WebcamDeviceNames.LOGITECH_C920;
            }
            else
            {
                Debug.LogError("Unfamiliar webcam device encountered: " + deviceName);
                return WebcamDeviceNames.NULL;
            }
        }

        public static void AssignSpecsToCamera(Camera cam, WebCamSpecs specs)
        {

            cam.fieldOfView = specs.VerticalFOV;
            cam.aspect = (float)((double)specs.HorizontalFOV / (double)specs.VerticalFOV);
            cam.near = specs.NearClippingPlane;
            cam.far = specs.FarClippingPlane;
        }
    }
}