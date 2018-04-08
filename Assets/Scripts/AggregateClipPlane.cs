using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterpolatedCamera
{
    public class AggregateClipPlane : MonoBehaviour
    {
        public GameObject MainCamera;
        public string PlaneName = "InterpolatedPlane";
        public ViewingCamManager viewingCamManager;
        public static Mesh cubeMesh;
        public Vector2[][] UVArray;
        private Vector4[][] ShaderUVArray;
        //public Texture2DArray TextureArray;
        public Texture2D[] TextureArray;
        public float PlaneShrinkFactor = 0.0f;
        public List<Vector2> UVList;
        public float PlanePullDistance = 0.05f;


        private bool initialized = false;

        // Use this for initialization
        void Start()
        {
            //Invoke("Init", 2.0f);
            Init();
        }

        private void FixedUpdate()
        {

        }

        public void Init()
        {
            //Debug.Log("Viewing cam clip plane corner01 = " + viewingCamManager.ViewingCameras[0].GetComponent<ClipPlaneManager>().ClipRect.Corner01);
            //Debug.Log("Clip planes array corner10 = " + viewingCamManager.ClipPlanes[0].clipPlane.Corner10);

            PlaneRect pr = GenerateAggregatePlaneRect();
            GameObject go = GenerateAggregateClipPlaneObject(pr);
            go.transform.parent = gameObject.transform;
            InvokeRepeating("RefreshShaderInfo", 1.0f, 2.0f);
            //Invoke("RefreshShaderInfo", 2.0f);
        }

        // Update is called once per frame
        void Update()
        {
            //print(gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.GetVector("_UV0"));
            GameObject planeObj = gameObject.transform.GetChild(0).gameObject;
            var mr = planeObj.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                for (int i = 0; i < ShaderUVArray.Length; i++)
                {
                    mr.material.SetVectorArray("_UV" + i.ToString(), ShaderUVArray[i]);
                }
            }
            else
            {
                Debug.LogError("Aggregate Clip Plane object did not construct object properly.");
            }
        }

        public void SetUVArray(Vector2[][] uvArray)
        {
            UVArray = uvArray;
            ShaderUVArray = ConvertUVValuesForShader(UVArray);

            UVList = new List<Vector2>();
            for(int i = 0; i < UVArray.Length; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    UVList.Add(UVArray[i][j]);
                }
            }

            //// Debugging
            //Vector4 left = Vector4.Lerp(ShaderUVArray[0][0], ShaderUVArray[0][1], 0.25f);
            //Vector4 right = Vector4.Lerp(ShaderUVArray[0][3], ShaderUVArray[0][2], 0.25f);
            //Vector4 bottom = Vector4.Lerp(ShaderUVArray[0][0], ShaderUVArray[0][3], 0.25f);
            //Vector4 top = Vector4.Lerp(ShaderUVArray[0][1], ShaderUVArray[0][2], 0.25f);

            //Debug.Log("Left lerp = " + left);
            //Debug.Log("Right lerp = " + right);
            //Debug.Log("Bottom lerp = " + bottom);
            //Debug.Log("Top lerp = " + top);
        }

        //public void SetTextureArray(Texture2DArray texArray)
        //{
        //    TextureArray = texArray;
        //}

        public void SetTextures(Texture2D[] texArray)
        {
            TextureArray = texArray;
        }

        public void RefreshShaderInfo()
        {
            GameObject planeObj = gameObject.transform.GetChild(0).gameObject;
            var mr = planeObj.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                for(int i = 0; i < TextureArray.Length; i++)
                {
                    mr.material.SetTexture("_Tex" + i.ToString(), TextureArray[i]);
                }
                //mr.material.SetTexture("_MainTex", TextureArray);

                //for (int i = 0; i < ShaderUVArray.Length; i++)
                //{
                //    mr.material.SetVectorArray("_UV" + i.ToString(), ShaderUVArray[i]);
                //}
            }
            else
            {
                Debug.LogError("Aggregate Clip Plane object did not construct object properly.");
            }
        }

        public Vector4[][] ConvertUVValuesForShader(Vector2[][] uvArray)
        {
            Vector4[][] newUVArray = new Vector4[uvArray.Length][];

            for(int i = 0; i < uvArray.Length; i++)
            {
                Vector2[] uvs = uvArray[i];
                Vector4[] newUVs = new Vector4[uvs.Length];
                for(int j = 0; j < uvs.Length; j++)
                {
                    newUVs[j] = new Vector4(uvs[j].x, uvs[j].y, 0, 0);
                }

                newUVArray[i] = newUVs;
            }

            return newUVArray;
        }

        private void cacheCubeMesh()
        {
            // Cache the cube mesh for easy access to generate the portal 
            // mesh later
            var tempCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cubeMesh = new Mesh();
            var tempMesh = tempCube.GetComponent<MeshFilter>().mesh;
            cubeMesh.SetVertices(new List<Vector3>(tempMesh.vertices));
            cubeMesh.SetTriangles(tempMesh.triangles, 0);

            // Copy over the uv values for the mesh, or the box faces 
            // will appear to be a single solid color
            var tempUVs = new List<Vector2>(tempMesh.uv);
            cubeMesh.SetUVs(0, tempUVs);

            GameObject.Destroy(tempCube);
        }

        public PlaneRect GenerateAggregatePlaneRect()
        {
            ClipPlaneManager clipPlane = new ClipPlaneManager(MainCamera.GetComponent<Camera>());
            // Ensure that plane calculations won't get messed up by overlaying 
            // a clipping plane in the same position as the camera, which 
            // causes NaN errors in UV calculations
            //clipPlane.clipPlane.Translate(new Vector3(0, 0, WebCamSpecsManager.DefaultFarClippingPlane));

            return clipPlane.clipPlane;
        }

        public GameObject GenerateAggregateClipPlaneObject(PlaneRect planeRect)
        {
            GameObject interpolatedPlane = new GameObject();

            // Generate the plane rectangle
            Vector3 pos;
            Vector3 forward;
            //PlaneRect planeRect = GenerateAggregatePlaneRect(clipPlanes, out pos, out forward);
            //PlaneRect planeRect = GenerateAggregatePlaneRect(clipPlanes);
            DebugPlaneRect dpr = interpolatedPlane.AddComponent<DebugPlaneRect>(); // Allows for easy access to see where the 
            dpr.SetCorners(planeRect.Corner00, planeRect.Corner01, planeRect.Corner11, planeRect.Corner10);
            dpr.AssociatePlaneRect(planeRect);

            // Add UVCalc script for easier UI control access and intersection calculation later
            var uvCalc = interpolatedPlane.AddComponent<UVCalc>();
            uvCalc.SetRepresentativePlane(MainCamera.GetComponent<Camera>());

            // Generate the mesh and object for the plane
            Mesh planeMesh = GeneratePlaneMesh(planeRect);
            var mf = interpolatedPlane.AddComponent<MeshFilter>();
            mf.mesh = planeMesh;
            var mr = interpolatedPlane.AddComponent<MeshRenderer>();

            // Set material
            Material planeMat = GeneratePlaneMaterial();
            mr.material = planeMat;

            // Assign meta attributes
            interpolatedPlane.name = PlaneName;

            // Adjust position and orientation of generated plane object
            // (Make it face the user (the forward used is the forward 
            // of the other clip plane cameras to the clip planes. 
            // The forward for this object has to be from the plane to 
            // the "camera" it is associated with))
            //aggregateClipPlane.transform.forward = -forward;
            //aggregateClipPlane.transform.position = pos + forward.normalized * FarClipDistance;
            interpolatedPlane.transform.position += -interpolatedPlane.transform.forward.normalized * PlanePullDistance; // -forward.normalized * 3.0f

            //Debug.Log("Aggregate calculated pos = " + aggregateClipPlane.transform.position);

            // set initialization flag appropriately
            if (planeRect != null
                && planeMesh != null
                && planeMat != null)
            {
                Debug.Log("Aggregate Clip Plane initialized");
                initialized = true;
            }
            else
            {
                GameObject.Destroy(interpolatedPlane);
            }

            return interpolatedPlane;
        }
        
        // NOTE: If generating dynamic aggregate clip plane as originally intended, InterpolationManager's GenerateUV needs to call this instead of GenerateAggregatePlaneRect
        public PlaneRect GenerateAggregatePlaneRect(ClipPlaneManager[] clipPlanes)//, out Vector3 pos, out Vector3 forward)
        {
            // NOTE: If generating dynamic aggregate clip plane as originally intended, InterpolationManager's GenerateUV needs to call this instead of GenerateAggregatePlaneRect

            // Adjust all clipPlanes and viewing cameras to be in their starting position
            // ERROR TESTING
            // This is a bit of a hack to circumvent an issue encountered in ViewingCamManager in InitViewingCameras
            for (int i = 0; i < viewingCamManager.ViewingCameras.Length; i++)
            {
                GameObject viewingCam = viewingCamManager.ViewingCameras[i];
                ClipPlaneManager clipPlane = viewingCam.GetComponent<ClipPlaneManager>();
                // ERROR TESTING - this fix doesn't work either...
                //viewingCam.transform.Translate(viewingCamManager.CamStartingPositions[i]);
                //clipPlane.transform.Translate(viewingCamManager.CamStartingPositions[i]);
            }

            if (clipPlanes != null
                && clipPlanes.Length > 0)
            {
                List<ClipPlaneManager> clipPlaneList = ClipPlaneManager.SortClipPlanes(clipPlanes);
                ClipPlaneManager[] sortedClipPlanes = clipPlaneList.ToArray();

                // Assumes all planes exist in the same plane and that they 
                // are all horizontally aligned
                float leftX = sortedClipPlanes[0].clipPlane.Corner00.x;
                float rightX = sortedClipPlanes[0].clipPlane.Corner01.x;
                float bottomY = sortedClipPlanes[0].clipPlane.Corner00.y;
                float topY = sortedClipPlanes[0].clipPlane.Corner10.y;
                float z = sortedClipPlanes[0].clipPlane.Corner00.z;

                //Debug.Log("Sorted clip planes length = " + sortedClipPlanes.Length);
                //Debug.Log("Sorted clip planes:");
                //Debug.Log("Sorted clip planes[0] corner00 = " + sortedClipPlanes[0].clipPlane.Corner00);
                //Debug.Log("Sorted clip planes[0] corner01 = " + sortedClipPlanes[0].clipPlane.Corner01);
                //Debug.Log("Sorted clip planes[0] corner11 = " + sortedClipPlanes[0].clipPlane.Corner11);
                //Debug.Log("Sorted clip planes[0] corner10 = " + sortedClipPlanes[0].clipPlane.Corner10);

                //Debug.Log("Unsorted clip planes[0] corner00 = " + clipPlanes[0].clipPlane.Corner00);
                //Debug.Log("Unsorted clip planes[0] corner01 = " + clipPlanes[0].clipPlane.Corner01);
                //Debug.Log("Unsorted clip planes[0] corner11 = " + clipPlanes[0].clipPlane.Corner11);
                //Debug.Log("Unsorted clip planes[0] corner10 = " + clipPlanes[0].clipPlane.Corner10);

                for (int i = 0; i < sortedClipPlanes.Length; i++)
                {
                    Vector3[] corners = new Vector3[4] {
                        sortedClipPlanes[i].clipPlane.Corner00,
                        sortedClipPlanes[i].clipPlane.Corner01,
                        sortedClipPlanes[i].clipPlane.Corner11,
                        sortedClipPlanes[i].clipPlane.Corner10
                    };

                    foreach(Vector3 corner in corners)
                    {
                        if (corner.x < leftX)
                        {
                            leftX = corner.x;
                            //Debug.Log(sortedClipPlanes[i].gameObject.name + "replaced leftX with: " + corner.x + " (" + corner + ")");
                        }
                        if (corner.x > rightX)
                        {
                            rightX = corner.x;
                            //Debug.Log(sortedClipPlanes[i].gameObject.name + "replaced rightX with: " + corner.x + " (" + corner + ")");
                        }
                        if (corner.y < bottomY)
                        {
                            bottomY = corner.y;
                            //Debug.Log(sortedClipPlanes[i].gameObject.name + "replaced bottomY with: " + corner.y + " (" + corner + ")");
                        }
                        if (corner.y > topY)
                        {
                            topY = corner.y;
                            //Debug.Log(sortedClipPlanes[i].gameObject.name + "replaced topY with: " + corner.y + " (" + corner + ")");
                        }
                    }
                }

                PlaneRect newPlane = new PlaneRect(
                    new Vector3(leftX, bottomY, z), // lower left
                    new Vector3(leftX, topY, z), // upper left
                    new Vector3(rightX, topY, z), // upper right
                    new Vector3(rightX, bottomY, z) // lower right
                    );

                //int farRightIndex = sortedClipPlanes.Length - 1;
                //int farLeftIndex = 0;
                //ClipPlaneManager farLeft = sortedClipPlanes[farLeftIndex];
                //ClipPlaneManager farRight = sortedClipPlanes[farRightIndex];
                //PlaneRect newPlane = new PlaneRect(
                //    farLeft.ClipRect.Corner00, // lower left
                //    farLeft.ClipRect.Corner01, // upper left
                //    farRight.ClipRect.Corner11, // upper right
                //    farRight.ClipRect.Corner10  // lower right
                //    );

                //Debug.Log("Far Left C00 = " + farLeft.ClipRect.Corner00);
                //Debug.Log("Far Left C01 = " + farLeft.ClipRect.Corner01);
                //Debug.Log("Far Right C11 = " + farRight.ClipRect.Corner11);
                //Debug.Log("Far Right C10 = " + farRight.ClipRect.Corner10);

                //Debug.Log("New Plane Corner00 = " + newPlane.Corner00);
                //Debug.Log("New Plane Corner01 = " + newPlane.Corner01);
                //Debug.Log("New Plane Corner11 = " + newPlane.Corner11);
                //Debug.Log("New Plane Corner10 = " + newPlane.Corner10);

                // Generate a slightly shrunken plane for our purposes
                PlaneRect shrunkenPlane = ShrinkPlaneRect(newPlane, PlaneShrinkFactor);

                //// Form anchor for new plane (as if the image was projected from 
                //// a camera at this point)
                //pos = farLeft.pos + ((farRight.pos - farLeft.pos) / 2.0f);
                //// ERROR TESTING - THIS NEEDS TO BE ADJUSTED TO ACCOUNT FOR STATES WHERE THE FORWARD DIRECTIONS ARE NOT THE SAME.
                //forward = farLeft.forward;

                return shrunkenPlane;
            }
            else
            {
                //pos = Vector3.zero;
                //forward = new Vector3(0, 0, 1);
                return null;
            }
        }

        /// <summary>
        /// Generates a new plane from a given plane by shrinking the corners in 
        /// to each other. This assumes a flat, level plane.
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public PlaneRect ShrinkPlaneRect(PlaneRect plane, float shrinkFactor)
        {
            // Generate diagonal vectors for shrinking points
            Vector3 ULtoLR = plane.Corner10 - plane.Corner01;
            Vector3 LRtoUL = -ULtoLR;
            Vector3 LLtoUR = plane.Corner11 - plane.Corner00;
            Vector3 URtoLL = -LLtoUR;

            // Adjust the points that will make the new plane
            Vector3 newLL = plane.Corner00 + LLtoUR * shrinkFactor;
            Vector3 newUL = plane.Corner01 + ULtoLR * shrinkFactor;
            Vector3 newUR = plane.Corner11 + URtoLL * shrinkFactor;
            Vector3 newLR = plane.Corner10 + LRtoUL * shrinkFactor;

            // Make the new plane
            PlaneRect newPlane = new PlaneRect(newLL, newUL, newUR, newLR);

            Debug.Log("Shrunken plane LL = " + newLL);
            Debug.Log("Original plane LL = " + plane.Corner00);
            Debug.Log("Shrunken plane UL = " + newUL);
            Debug.Log("Original plane UL = " + plane.Corner01);
            Debug.Log("Shrunken plane UR = " + newUR);
            Debug.Log("Original plane UR = " + plane.Corner11);
            Debug.Log("Shrunken plane LR = " + newLR);
            Debug.Log("Original plane LR = " + plane.Corner10);

            return newPlane;
        }

        public Mesh GeneratePlaneMesh(PlaneRect plane)
        {
            if (plane != null)
            {
                Mesh planeMesh = new Mesh();

                // Set mesh vertices
                List<Vector3> meshVertices = new List<Vector3>();
                meshVertices.Add(plane.Corner00); // lower left
                meshVertices.Add(plane.Corner01); // upper left
                meshVertices.Add(plane.Corner11); // upper right
                meshVertices.Add(plane.Corner10); // lower right

                //Debug.Log("Plane corner 00 = " + plane.Corner00);
                //Debug.Log("Plane corner 01 = " + plane.Corner01);
                //Debug.Log("Plane corner 11 = " + plane.Corner11);
                //Debug.Log("Plane corner 10 = " + plane.Corner10);

                // Set mesh triangles
                int[] meshTriangles = new int[6];
                meshTriangles[0] = 0;
                meshTriangles[1] = 1;
                meshTriangles[2] = 2;
                meshTriangles[3] = 0;
                meshTriangles[4] = 2;
                meshTriangles[5] = 3;

                planeMesh.SetVertices(meshVertices);
                planeMesh.SetTriangles(meshTriangles, 0);

                // Set the uv values
                List<Vector2> uvs = new List<Vector2>();
                uvs.Add(new Vector2(0, 0));
                uvs.Add(new Vector2(0, 1));
                uvs.Add(new Vector2(1, 1));
                uvs.Add(new Vector2(1, 0));
                //planeMesh.uv = uvs.ToArray();
                planeMesh.SetUVs(0, uvs);
                planeMesh.SetUVs(1, uvs);

                planeMesh.name = "PlaneMesh";

                return planeMesh;
            }
            else
            {
                Debug.LogError("Plane not created properly. Cannot construct plane mesh.");
                return cubeMesh;
            }
        }

        public Material GeneratePlaneMaterial()
        {
            Material aggregatePlaneMaterial = new Material(Shader.Find("Custom/InterpolatedCameraShader"));
            //Material aggregatePlaneMaterial = new Material(Shader.Find("Custom/InterpolatedShader2"));
            aggregatePlaneMaterial.name = "PlaneMat";

            // Set initial texture array values

            return aggregatePlaneMaterial;
        }
    }
}