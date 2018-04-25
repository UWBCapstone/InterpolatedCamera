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

            //var vtm = GameObject.Find("ViewportTableManager").GetComponent<ViewportTableManager>();
            //var wtm = GameObject.Find("WorldTableManager").GetComponent<WorldTableManager>();

            //float[] depths = new float[4];

            //if (gameObject.name.Equals(vtm.TableName))
            //{
            //    depths[0] = vtm.LowerLeftZ;
            //    depths[1] = vtm.UpperLeftZ;
            //    depths[2] = vtm.UpperRightZ;
            //    depths[3] = vtm.LowerRightZ;
            //}
            //else if (gameObject.name.Equals(wtm.TableName))
            //{
            //    depths[0] = wtm.LowerLeft.z; // assumes camera is at 0,0,0
            //    depths[1] = wtm.UpperLeft.z;
            //    depths[2] = wtm.UpperRight.z;
            //    depths[3] = wtm.LowerRight.z;
            //}

            float[] depths = GetDepths();

            mr.material.SetVector("_LLUV", new Vector4(LowerLeftUV.x, LowerLeftUV.y, depths[0], 0));
            mr.material.SetVector("_ULUV", new Vector4(UpperLeftUV.x, UpperLeftUV.y, depths[1], 0));
            mr.material.SetVector("_URUV", new Vector4(UpperRightUV.x, UpperRightUV.y, depths[2], 0));
            mr.material.SetVector("_LRUV", new Vector4(LowerRightUV.x, LowerRightUV.y, depths[3], 0));
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
            const float defaultDepth = 0.001f;

            float depth = vertex.z;
            float zeroMin = 0 - float.Epsilon;
            float zeroMax = 0 + float.Epsilon;

            if (depth < zeroMax && depth > zeroMin)
            {
                return TableCamera.WorldToViewportPoint(new Vector3(vertex.x, vertex.y, defaultDepth));
            }
            else
            {
                // gets funky when you pass in a negative depth... can't have any table corners with negative depth
                return TableCamera.WorldToViewportPoint(vertex);
            }
        }

        public float[] GetDepths()
        {
            float[] depths = new float[4];
            var mf = gameObject.GetComponent<MeshFilter>();
            Mesh m = mf.mesh;

            Vector3 ll = m.vertices[0];
            Vector3 ul = m.vertices[1];
            Vector3 ur = m.vertices[m.vertices.Length / 2];
            Vector3 lr = m.vertices[m.vertices.Length / 2 + 1];

            float floatZeroMin = 0 - float.Epsilon;
            float floatZeroMax = 0 + float.Epsilon;

            depths[0] = (ll.z < floatZeroMax && ll.z > floatZeroMin) ? 0.001f : ll.z;
            depths[1] = (ul.z < floatZeroMax && ul.z > floatZeroMin) ? 0.001f : ul.z;
            depths[2] = (ur.z < floatZeroMax && ur.z > floatZeroMin) ? 0.001f : ur.z;
            depths[3] = (lr.z < floatZeroMax && lr.z > floatZeroMin) ? 0.001f : lr.z;

            return depths;
        }
    }
}