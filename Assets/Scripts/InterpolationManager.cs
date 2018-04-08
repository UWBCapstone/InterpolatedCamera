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
        //public Texture2DArray textureArray; // array of the images from the webcams / viewing cameras
        public Texture2D[] textureArray;
        public const int maxCameras = 8; // Hard coded in shader due to texture array usage
        public GameObject MainCamera;
        public GameObject aggregateClipPlane;
        public string CamIndicesToIgnore = "1";
        private List<int> CamIndicesToIgnoreList;

        public void Start()
        {
            //AssignToAggregateClipPlaneShader();
            Invoke("AssignToAggregateClipPlaneShader", 2.0f);
            CamIndicesToIgnoreList = ParseCamIndicesToIgnore(CamIndicesToIgnore);
        }

        private List<int> ParseCamIndicesToIgnore(string commaSeparatedListOfIndices)
        {
            List<int> indicesToIgnore = new List<int>();
            string[] indexStringArray = commaSeparatedListOfIndices.Split(new char[1] { ',' });
            foreach(string indexString in indexStringArray)
            {
                int index;
                if(int.TryParse(indexString.Trim(), out index))
                {
                    if (!indicesToIgnore.Contains(index))
                    {
                        indicesToIgnore.Add(index);
                        if (viewingCamManager.ViewingCameras.Length > index)
                        {
                            //Debug.Log("Ignoring input from camera " + index + " (" + viewingCamManager.ViewingCameras[index].GetComponent<WebCamIdentifier>().DeviceNameString + ")");
                        }
                    }
                }
            }

            return indicesToIgnore;
        }

        public void Update()
        {
            // Update cam indices to ignore
            CamIndicesToIgnoreList = ParseCamIndicesToIgnore(CamIndicesToIgnore);

            // Update textures
            AggregateClipPlane canvas = aggregateClipPlane.GetComponent<AggregateClipPlane>();
            for(int i = 0; i < viewingCamManager.ViewingCameras.Length; i++)
            {
                if (!CamIndicesToIgnoreList.Contains(i))
                {
                    WebCamTexture feed = viewingCamManager.ViewingCameras[i].GetComponent<WebCamIdentifier>().WebCamFeed;
                    if (textureArray != null
                        && textureArray.Length > i)
                    {
                        Graphics.CopyTexture(feed, textureArray[i]);
                    }
                    if (canvas.TextureArray != null
                        && canvas.TextureArray.Length > i)
                    {
                        Graphics.CopyTexture(feed, canvas.TextureArray[i]);
                    }
                }
            }

            // Update UVs
            Vector2[][] uvs = GenerateUVArray();
            canvas.SetUVArray(uvs);
        }

        public void AssignToAggregateClipPlaneShader()
        {
            //AggregateClipPlane canvas = aggregateClipPlane.GetComponent<AggregateClipPlane>();
            //Vector2[][] uvArray = GenerateUVArray();
            //canvas.SetUVArray(uvArray);
            ////Texture2DArray texArray = GenerateTextureArray();
            //Texture2D[] texArray = GenerateTextureArray();
            //canvas.SetTextures(texArray);
            //textureArray = texArray;

            AssignToAggregateClipPlaneShader(CamIndicesToIgnoreList);
        }

        public void AssignToAggregateClipPlaneShader(List<int> camIndicesToIgnore)
        {
            AggregateClipPlane canvas = aggregateClipPlane.GetComponent<AggregateClipPlane>();
            Vector2[][] uvArray = GenerateUVArray();
            Texture2D[] texArray = GenerateTextureArray();

            foreach(int index in camIndicesToIgnore)
            {
                uvArray[index] = GenerateBlankUV();
                texArray[index] = GenerateBlackTexture();
            }

            canvas.SetUVArray(uvArray);
            canvas.SetTextures(texArray);
            textureArray = texArray;
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
            //ClipPlaneManager viewingCamClipPlane = viewingCamManager.ClipPlanes[camIndex];
            //Vector3 origin = MainCamera.transform.position;
            Vector3 origin = viewingCam.transform.position;

            //PlaneRect aggregatePlane = aggregateClipPlane.GetComponent<AggregateClipPlane>().GenerateAggregatePlaneRect(viewingCamManager.ClipPlanes);
            Vector3[] aggregatePlaneVertices;
            bool interpolatedPlaneInitialized = 
                aggregateClipPlane.transform.childCount == 1 
                && aggregateClipPlane.transform.GetChild(0).name.Equals(aggregateClipPlane.GetComponent<AggregateClipPlane>().PlaneName);
            if (interpolatedPlaneInitialized)
            {
                GameObject interpolatedPlane = aggregateClipPlane.transform.GetChild(0).gameObject;
                DebugPlaneRect dpr = interpolatedPlane.GetComponent<DebugPlaneRect>();
                aggregatePlaneVertices = new Vector3[4]
                {
                    dpr.LowerLeft,
                    dpr.UpperLeft,
                    dpr.UpperRight,
                    dpr.LowerRight
                };

                Debug.Log("Taking UV values from Interpolated plane debug plane rectangle.");
            }
            else
            {
                PlaneRect aggregatePlane = aggregateClipPlane.GetComponent<AggregateClipPlane>().GenerateAggregatePlaneRect();
                aggregatePlaneVertices = new Vector3[4]
                {
                aggregatePlane.Corner00,
                aggregatePlane.Corner01,
                aggregatePlane.Corner11,
                aggregatePlane.Corner10
                };

                Debug.Log("Taking UV values from uninitialized plane calculations.");
            }

            Debug.Log("aggregatePlaneVertices[0] = " + aggregatePlaneVertices[0]);
            Debug.Log("aggregatePlaneVertices[1] = " + aggregatePlaneVertices[1]);
            Debug.Log("aggregatePlaneVertices[2] = " + aggregatePlaneVertices[2]);
            Debug.Log("aggregatePlaneVertices[3] = " + aggregatePlaneVertices[3]);

            // Calculate the UV values of the aggregate plane in terms of the 
            // UV for the clip plane of the given viewing camera clip plane. 
            // This makes it so that the four corners of the aggregate plane 
            // can be passed in as UV values and compared to each of the 
            // textures in the texture array to determine easily if a given uv 
            // value for the aggregate plane corresponds to a point on any of 
            // the independent texture planes
            //Vector2 LL = uvCalc.UVOffCanvas(origin, viewingCamClipPlane.clipPlane.Corner00);
            //Vector2 UL = uvCalc.UVOffCanvas(origin, viewingCamClipPlane.clipPlane.Corner01);
            //Vector2 UR = uvCalc.UVOffCanvas(origin, viewingCamClipPlane.clipPlane.Corner11);
            //Vector2 LR = uvCalc.UVOffCanvas(origin, viewingCamClipPlane.clipPlane.Corner10);
            Vector2 LL = uvCalc.UVOffCanvas(origin, aggregatePlaneVertices[0]); // lower left
            Vector2 UL = uvCalc.UVOffCanvas(origin, aggregatePlaneVertices[1]); // upper left
            Vector2 UR = uvCalc.UVOffCanvas(origin, aggregatePlaneVertices[2]); // upper right
            Vector2 LR = uvCalc.UVOffCanvas(origin, aggregatePlaneVertices[3]); // lower right

            UVs[0] = LL;
            UVs[1] = UL;
            UVs[2] = UR;
            UVs[3] = LR;

            // update values shown on uvCalc script of viewing camera
            uvCalc.LowerLeft = LL;
            uvCalc.UpperLeft = UL;
            uvCalc.UpperRight = UR;
            uvCalc.LowerRight = LR;

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

        public Texture2D[] GenerateTextureArray()
        {
            Texture2D[] texArray = new Texture2D[maxCameras];

            int stop1 = (maxCameras > viewingCamManager.ViewingCameras.Length)
                ? viewingCamManager.ViewingCameras.Length
                : maxCameras;
            int stop2 = (maxCameras > viewingCamManager.ViewingCameras.Length)
                ? maxCameras
                : viewingCamManager.ViewingCameras.Length;

            // Copy webcam textures into Texture2DArray
            for (int i = 0; i < stop1; i++)
            {
                Texture2D tex = GenerateTexture(i);
                texArray[i] = tex;
            }
            if (stop2 > viewingCamManager.ViewingCameras.Length)
            {
                for (int i = stop1; i < stop2; i++)
                {
                    Texture2D blackTex = GenerateBlackTexture();
                    texArray[i] = blackTex;
                }
            }

            return texArray;
        }

        public Texture2D GenerateTexture(int camIndex)
        {
            GameObject viewingCam = viewingCamManager.ViewingCameras[camIndex];
            WebcamDeviceNames deviceName = viewingCam.GetComponent<WebCamIdentifier>().DeviceName;
            WebCamSpecs specs = WebCamSpecsManager.GetSpecs(deviceName);
            WebCamTexture webFeed = viewingCam.GetComponent<WebCamIdentifier>().WebCamFeed;

            int width = specs.HorizontalResolution;
            int height = specs.VerticalResolution;

            // https://forum.unity.com/threads/webcamtexture-texture2d.154057/
            // Tie the texture to the webcam feed texture associated with a viewing camera
            Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);

            ////var ptr = webFeed.GetNativeTexturePtr();
            ////Debug.Log(ptr);
            ////Texture2D exTex = Texture2D.CreateExternalTexture(width, height, TextureFormat.RGBA32, false, false, ptr);
            ////Debug.Log("Created external texture");
            //tex.UpdateExternalTexture(webFeed.GetNativeTexturePtr());
            //tex.Apply();
            Graphics.CopyTexture(webFeed, tex);
            tex.name = webFeed.name;

            return tex;
        }

        //public Texture2DArray GenerateTextureArray()
        //{
        //    textureArray = new Texture2DArray(1920, 1080, maxCameras, TextureFormat.RGBA32, false);
        //    textureArray.name = "WebCamFeedTextureArray";
        //    textureArray.wrapMode = TextureWrapMode.Clamp;

        //    int stop1 = (maxCameras > viewingCamManager.ViewingCameras.Length)
        //        ? viewingCamManager.ViewingCameras.Length
        //        : maxCameras;
        //    int stop2 = (maxCameras > viewingCamManager.ViewingCameras.Length)
        //        ? maxCameras
        //        : viewingCamManager.ViewingCameras.Length;

        //    // Copy webcam textures into Texture2DArray
        //    for (int i = 0; i < stop1; i++)
        //    {
        //        GameObject viewingCam = viewingCamManager.ViewingCameras[i];
        //        WebCamTexture tex = viewingCam.GetComponent<WebCamIdentifier>().WebCamFeed;
        //        //Graphics.CopyTexture(tex, 0, 0, textureArray, i, 0);
        //        textureArray.
        //        tex.GetNativeTexturePtr()
        //        mytex.UpdateExternalTexture()
        //    }
        //    if (stop2 > viewingCamManager.ViewingCameras.Length)
        //    {
        //        Texture2D blackTex = GenerateBlackTexture();

        //        // Fill remaining array spaces with black images with offset UV associations
        //        for(int i = stop1; i < stop2; i++)
        //        {
        //            Graphics.CopyTexture(blackTex, 0, 0, textureArray, i, 0);
        //        }
        //    }

        //    return textureArray;
        //}

        private Texture2D GenerateBlackTexture()
        {
            Texture2D BlackTex = new Texture2D(1920, 1080);
            Color[] colors = new Color[BlackTex.width * BlackTex.height];
            BlackTex.SetPixels(colors);
            BlackTex.Apply();
            BlackTex.name = "BlankTex";

            return BlackTex;
        }
    }
}