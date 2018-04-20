using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterpolatedCamera
{
    public class TableShaderSetter : MonoBehaviour
    {
        public Camera TableCamera;

        public Vector2 LowerLeftUV = new Vector2();
        public Vector2 UpperLeftUV = new Vector2();
        public Vector2 UpperRightUV = new Vector2();
        public Vector2 LowerRightUV = new Vector2();

        public void Update()
        {
            List<Vector2> uvs = CalculateUVs();
            SetUVs(uvs);
            SetShader();
        }

        public void SetShader()
        {
            var mr = gameObject.GetComponent<MeshRenderer>();
            //var tm = GameObject.Find("TableManager").GetComponent<TableManager>();
            var tm = GameObject.Find("TableManager").GetComponent<TableManagerNew>();

            mr.material.SetVector("_LLUV", new Vector4(LowerLeftUV.x, LowerLeftUV.y, tm.LowerLeftZ, 0));
            mr.material.SetVector("_ULUV", new Vector4(UpperLeftUV.x, UpperLeftUV.y, tm.UpperLeftZ, 0));
            mr.material.SetVector("_URUV", new Vector4(UpperRightUV.x, UpperRightUV.y, tm.UpperRightZ, 0));
            mr.material.SetVector("_LRUV", new Vector4(LowerRightUV.x, LowerRightUV.y, tm.LowerRightZ, 0));
        }

        // Calculate UVs from 
        public void SetUVs(List<Vector2> uvs)
        {
            //LowerLeftUV = uvs[0];
            //UpperLeftUV = uvs[1];
            //UpperRightUV = uvs[2];
            //LowerRightUV = uvs[3];

            LowerLeftUV = uvs[0];
            UpperLeftUV = uvs[1];
            UpperRightUV = uvs[uvs.Count / 2];
            LowerRightUV = uvs[uvs.Count / 2 + 1];
        }

        /// <summary>
        /// Calculate the viewport values of the uvs based on the render camera
        /// </summary>
        public List<Vector2> CalculateUVs()
        {
            List<Vector2> uvs = new List<Vector2>();

            var mf = gameObject.GetComponent<MeshFilter>();
            Mesh m = mf.mesh;

            List<Vector3> vertices = new List<Vector3>(m.vertices);
            vertices = GetWorldCoordinates(vertices);

            // Calculate the uvs from the vertices
            foreach(var vertex in vertices)
            {
                uvs.Add(GetViewportSpace(vertex));
            }

            return uvs;
        }

        public List<Vector3> GetWorldCoordinates(List<Vector3> localCoordinates)
        {
            List<Vector3> worldCoordinates = new List<Vector3>();

            foreach (var local in localCoordinates)
            {
                worldCoordinates.Add(gameObject.transform.TransformPoint(local));
            }

            return worldCoordinates;
        }

        public Vector2 GetViewportSpace(Vector3 vertex)
        {
            return TableCamera.WorldToViewportPoint(vertex);
        }
    }
}