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
        public float FarClipDistance = 1000.0f;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

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

        public GameObject GenerateAggregateClipPlaneObject(ClipPlaneManager[] clipPlanes)
        {
            GameObject aggregateClipPlane = new GameObject();

            // Generate the plane rectangle
            Vector3 pos;
            Vector3 forward;
            PlaneRect planeRect = GenerateAggregatePlaneRect(clipPlanes, out pos, out forward);
            
            // Generate the mesh and object for the plane
            Mesh planeMesh = GeneratePlaneMesh(planeRect);
            var mf = aggregateClipPlane.AddComponent<MeshFilter>();
            mf.mesh = planeMesh;
            var mr = aggregateClipPlane.AddComponent<MeshRenderer>();

            // Set material
            Material planeMat = GeneratePlaneMaterial();
            mr.material = planeMat;

            // Assign meta attributes
            aggregateClipPlane.name = PlaneName;

            // Adjust position and orientation of generated plane object
            // (Make it face the user (the forward used is the forward 
            // of the other clip plane cameras to the clip planes. 
            // The forward for this object has to be from the plane to 
            // the "camera" it is associated with))
            aggregateClipPlane.transform.forward = -forward;
            aggregateClipPlane.transform.position = pos + forward.normalized * FarClipDistance;
        }

        public PlaneRect GenerateAggregatePlaneRect(ClipPlaneManager[] clipPlanes, out Vector3 pos, out Vector3 forward)
        {
            List<ClipPlaneManager> clipPlaneList = ClipPlaneManager.SortClipPlanes(clipPlanes);
            ClipPlaneManager[] sortedClipPlanes = clipPlaneList.ToArray();
            
            // Assumes all planes exist in the same plane and that they 
            // are all horizontally aligned
            int farRightIndex = sortedClipPlanes.Length - 1;
            int farLeftIndex = 0;
            ClipPlaneManager farLeft = sortedClipPlanes[farLeftIndex];
            ClipPlaneManager farRight = sortedClipPlanes[farRightIndex];
            PlaneRect newPlane = new PlaneRect(
                farLeft.ClipRect.Corner00, // lower left
                farLeft.ClipRect.Corner01, // upper left
                farRight.ClipRect.Corner11, // upper right
                farRight.ClipRect.Corner10  // lower right
                );

            // Generate a slightly shrunken plane for our purposes
            float planeShrinkFactor = 0.15f;
            PlaneRect shrunkenPlane = ShrinkPlaneRect(newPlane, planeShrinkFactor);
            
            // Form anchor for new plane (as if the image was projected from 
            // a camera at this point)
            pos = farLeft.pos + ((farRight.pos - farLeft.pos) / 2.0f);
            // ERROR TESTING - THIS NEEDS TO BE ADJUSTED TO ACCOUNT FOR STATES WHERE THE FORWARD DIRECTIONS ARE NOT THE SAME.
            forward = farLeft.forward;

            return shrunkenPlane;
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
            Vector3 ULtoLR = plane.Corner01 - plane.Corner10;
            Vector3 LRtoUL = -ULtoLR;
            Vector3 LLtoUR = plane.Corner11 - plane.Corner00;
            Vector3 URtoLL = -LLtoUR;

            // Adjust the points that will make the new plane
            Vector3 newLL = plane.Corner00 + LLtoUR * shrinkFactor;
            Vector3 newUL = plane.Corner10 + ULtoLR * shrinkFactor;
            Vector3 newUR = plane.Corner11 + URtoLL * shrinkFactor;
            Vector3 newLR = plane.Corner10 + LRtoUL * shrinkFactor;

            // Make the new plane
            PlaneRect newPlane = new PlaneRect(newLL, newUL, newUR, newLR);

            return newPlane;
        }

        public Mesh GeneratePlaneMesh(PlaneRect plane)
        {
            Mesh planeMesh = new Mesh();

            // Set mesh vertices
            List<Vector3> meshVertices = new List<Vector3>();
            meshVertices.Add(plane.Corner00); // lower left
            meshVertices.Add(plane.Corner01); // upper left
            meshVertices.Add(plane.Corner11); // upper right
            meshVertices.Add(plane.Corner10); // lower right

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

            return planeMesh;
        }

        public Material GeneratePlaneMaterial()
        {
            Material aggregatePlaneMaterial = new Material(Shader.Find("Custom/InterpolatedCameraShader"));

            // Set initial texture array values


            return aggregatePlaneMaterial;
        }
    }
}