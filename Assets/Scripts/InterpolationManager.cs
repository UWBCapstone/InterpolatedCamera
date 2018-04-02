using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterpolatedCamera
{
    /// <summary>
    /// The purpose of this class is to handle the construction of the uv arrays and shader setting towards the aggregate plane that is required for camera interpolation.
    /// </summary>
    public class InterpolationManager : MonoBehaviour
    {
        public ViewingCamManager viewingCamManager;
        public Texture2DArray textureArray; // array of the images from the webcams / viewing cameras
        public const int maxCameras = 8; // Hard coded in shader due to texture array usage
        public GameObject MainCamera;
        public GameObject aggregateClipPlane;

        public void AssignToAggregateClipPlaneShader()
        {
            AggregateClipPlane canvas = aggregateClipPlane.GetComponent<AggregateClipPlane>();
            Vector2[][] uvArray = GenerateUVArray();
            canvas.SetUVArray(uvArray);
            Texture2DArray texArray = GenerateTextureArray();
            canvas.SetTextureArray(texArray);
        }
        
        public Vector2[][] GenerateUVArray()
        {
            Vector2[][] UVArray = new Vector2[maxCameras][];

            int stop1 = (maxCameras > viewingCamManager.ViewingCameras.Length)
                ? viewingCamManager.ViewingCameras.Length
                : maxCameras;
            int stop2 = (maxCameras > viewingCamManager.ViewingCameras.Length)
                ? maxCameras
                : viewingCamManager.ViewingCameras.Length;

            for (int i = 0; i < stop1; i++)
            {
                UVArray[i] = GenerateUV(i);
            }
            if (stop2 >= viewingCamManager.ViewingCameras.Length)
            {
                for (int i = stop1; i < stop2; i++)
                {
                    UVArray[i] = GenerateBlankUV();
                }
            }

            return UVArray;
        }

        public Vector2[] GenerateUV(int camIndex)
        {
            // Assumes LL, UL, UR, LR
            Vector2[] UVs = new Vector2[4];
            GameObject viewingCam = viewingCamManager.ViewingCameras[camIndex];
            UVCalc uvCalc = viewingCam.GetComponent<UVCalc>();
            ClipPlaneManager viewingCamClipPlane = viewingCamManager.ClipPlanes[camIndex];
            Vector3 origin = MainCamera.transform.position;

            // Calculate the UV values of the aggregate plane in terms of the 
            // UV for the clip plane of the given viewing camera clip plane. 
            // This makes it so that the four corners of the aggregate plane 
            // can be passed in as UV values and compared to each of the 
            // textures in the texture array to determine easily if a given uv 
            // value for the aggregate plane corresponds to a point on any of 
            // the independent texture planes
            Vector2 LL = uvCalc.UVOffCanvas(origin, viewingCamClipPlane.clipPlane.Corner00);
            Vector2 UL = uvCalc.UVOffCanvas(origin, viewingCamClipPlane.clipPlane.Corner01);
            Vector2 UR = uvCalc.UVOffCanvas(origin, viewingCamClipPlane.clipPlane.Corner11);
            Vector2 LR = uvCalc.UVOffCanvas(origin, viewingCamClipPlane.clipPlane.Corner10);

            UVs[0] = LL;
            UVs[1] = UL;
            UVs[2] = UR;
            UVs[3] = LR;

            return UVs;
        }

        /// <summary>
        /// Used to fill the blank spaces on the hard-set array of UV values.
        /// </summary>
        /// <returns></returns>
        public Vector2[] GenerateBlankUV()
        {
            // Assuems LL, UL, UR, LR
            Vector2[] blankUVs = new Vector2[4];

            Vector2 LL = new Vector2(-20.0f, -20.0f);
            Vector2 UL = new Vector2(-20.0f, -19.0f);
            Vector2 UR = new Vector2(-19.0f, -19.0f);
            Vector2 LR = new Vector2(-19.0f, -20.0f);

            blankUVs[0] = LL;
            blankUVs[1] = UL;
            blankUVs[2] = UR;
            blankUVs[3] = LR;

            return blankUVs;
        }

        public Texture2DArray GenerateTextureArray()
        {
            textureArray = new Texture2DArray(1920, 1080, maxCameras, TextureFormat.RGBA32, false);
            textureArray.wrapMode = TextureWrapMode.Clamp;

            int stop1 = (maxCameras > viewingCamManager.ViewingCameras.Length)
                ? viewingCamManager.ViewingCameras.Length
                : maxCameras;
            int stop2 = (maxCameras > viewingCamManager.ViewingCameras.Length)
                ? maxCameras
                : viewingCamManager.ViewingCameras.Length;

            // Copy webcam textures into Texture2DArray
            for (int i = 0; i < stop1; i++)
            {
                GameObject viewingCam = viewingCamManager.ViewingCameras[i];
                WebCamTexture tex = viewingCam.GetComponent<WebCamIdentifier>().WebCamFeed;
                Graphics.CopyTexture(tex, 0, 0, textureArray, i, 0);
            }
            if (stop2 > viewingCamManager.ViewingCameras.Length)
            {
                // Fill remaining array spaces with black images with offset UV associations
                for(int i = stop1; i < stop2; i++)
                {
                    Graphics.CopyTexture(Texture2D.blackTexture, 0, 0, textureArray, i, 0);
                }
            }

            return textureArray;
        }
    }
}