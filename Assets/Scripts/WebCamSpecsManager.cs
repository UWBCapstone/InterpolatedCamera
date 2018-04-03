using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterpolatedCamera
{
    public enum WebcamDeviceNames
    {
        NULL,
        LOGITECH_C920,
        ADESSO_CYBERTRACK_V10,
        LAPTOP_WEBCAM
    };

    public struct WebCamSpecs
    {
        public float HorizontalFOV;
        public float VerticalFOV;
        public float NearClippingPlane;
        public float FarClippingPlane;
        public WebcamDeviceNames WebcamDeviceName;
        public string DeviceName;
        public int HorizontalResolution;
        public int VerticalResolution;

        public WebCamSpecs(
            float horizontalFOV, 
            float verticalFOV, 
            float nearClippingPlane, 
            float farClippingPlane, 
            WebcamDeviceNames webcamDeviceName,
            string deviceName,
            int horizontalResolution,
            int verticalResolution
            )
        {
            HorizontalFOV = horizontalFOV;
            VerticalFOV = verticalFOV;
            NearClippingPlane = nearClippingPlane;
            FarClippingPlane = farClippingPlane;
            WebcamDeviceName = webcamDeviceName;
            DeviceName = deviceName;
            HorizontalResolution = horizontalResolution;
            VerticalResolution = verticalResolution;
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
            int HorizontalResolution;
            int VerticalResolution;

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
                    HorizontalResolution = 640;
                    VerticalResolution = 480;

                    camSpecs = new WebCamSpecs(
                        HorizontalFOV,
                        VerticalFOV,
                        NearClippingPlane,
                        FarClippingPlane,
                        WebcamDeviceName,
                        DeviceName,
                        HorizontalResolution,
                        VerticalResolution);

                    break;
                case WebcamDeviceNames.ADESSO_CYBERTRACK_V10:
                    HorizontalFOV = 25.0f;
                    VerticalFOV = 25.0f;
                    NearClippingPlane = 0.3f;
                    FarClippingPlane = 1000f;
                    WebcamDeviceName = WebcamDeviceNames.ADESSO_CYBERTRACK_V10;
                    DeviceName = WebcamDeviceName.ToString();
                    HorizontalResolution = 320;
                    VerticalResolution = 240;

                    camSpecs = new WebCamSpecs(
                        HorizontalFOV,
                        VerticalFOV,
                        NearClippingPlane,
                        FarClippingPlane,
                        WebcamDeviceName,
                        DeviceName,
                        HorizontalResolution,
                        VerticalResolution);

                    break;
                case WebcamDeviceNames.LAPTOP_WEBCAM:
                    HorizontalFOV = 75.0f;
                    VerticalFOV = 56.25f;
                    NearClippingPlane = 0.3f;
                    FarClippingPlane = 1000f;
                    WebcamDeviceName = WebcamDeviceNames.LAPTOP_WEBCAM;
                    DeviceName = WebcamDeviceName.ToString();
                    HorizontalResolution = 640;
                    VerticalResolution = 480;

                    camSpecs = new WebCamSpecs(
                        HorizontalFOV,
                        VerticalFOV,
                        NearClippingPlane,
                        FarClippingPlane,
                        WebcamDeviceName,
                        DeviceName,
                        HorizontalResolution,
                        VerticalResolution);

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

            if (deviceName.ToLower().Contains("HD Pro Webcam C920".ToLower()))
            {
                return WebcamDeviceNames.LOGITECH_C920;
            }
            else if (deviceName.ToLower().Contains("Adesso".ToLower())
                || deviceName.ToLower().Contains("USB2.0".ToLower()))
            {
                return WebcamDeviceNames.ADESSO_CYBERTRACK_V10;
            }
            else if(deviceName.ToLower().Contains("Integrated Webcam"))
            {
                return WebcamDeviceNames.LAPTOP_WEBCAM;
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