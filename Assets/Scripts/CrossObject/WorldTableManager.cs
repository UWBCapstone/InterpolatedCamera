using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterpolatedCamera
{
    public class WorldTableManager : MonoBehaviour
    {
        public Camera MainCamera;
        public Camera TableCamera;
        public GameObject WorldObjectParent;
        public int NumberOfVertices = 10000;
        public string TableName = "WorldTable";
        public Vector3 LowerLeft = new Vector3(0, 0, 0);
        public Vector3 UpperLeft = new Vector3(0, 0, 1);
        public Vector3 UpperRight = new Vector3(1, 0, 1);
        public Vector3 LowerRight = new Vector3(1, 0, 0);

        public void Awake()
        {
            //Table = MakeTable();
        }

        public GameObject MakeTable()
        {
            // Use the four points passed in
            GameObject generatedTable = new GameObject();
            generatedTable.SetActive(false);
            generatedTable.name = TableName;
            generatedTable.layer = LayerManager.GetLayerMask(CloakLayers.Table);

            // Generate mesh
            Mesh tableMesh = GenerateTableMesh();

            // Generate Material
            Material tableMaterial = GenerateTableMaterial();

            // Assign these to the object
            var mf = generatedTable.AddComponent<MeshFilter>();
            var mr = generatedTable.AddComponent<MeshRenderer>();
            mf.mesh = tableMesh;
            mr.material = tableMaterial;

            var mc = generatedTable.AddComponent<MeshCollider>();
            mc.sharedMesh = tableMesh;

            // Add custom scripts
            var tss = generatedTable.AddComponent<TableShaderSetter>();
            TableCamera.aspect = Camera.main.aspect;
            tss.TableCamera = TableCamera;

            // Set parent
            //generatedTable.transform.parent = gameObject.transform;
            generatedTable.transform.parent = WorldObjectParent.transform;

            generatedTable.SetActive(true);
            return generatedTable;
        }

        public Mesh GenerateTableMesh()
        {
            Mesh m = new Mesh();
            m.name = TableName;

            // Get the 3d points for the table
            List<Vector3> tableCorners = GetTableCorners(NumberOfVertices);
            List<int> tableTriangles = GetTableTriangles(tableCorners.Count);
            List<Vector2> tableUVs = GetTableUVs(tableCorners.Count);

            // Assign them to the mesh
            m.SetVertices(tableCorners);
            m.SetTriangles(tableTriangles.ToArray(), 0);
            m.SetUVs(0, tableUVs);
            m.SetUVs(1, tableUVs);

            // Return the mesh
            return m;
        }

        public List<Vector3> GetTableCorners(int numTotalVertices)
        {
            List<Vector3> cornerList = new List<Vector3>();
            
            Vector3 TopVector = UpperRight - UpperLeft;
            Vector3 BottomVector = LowerLeft - LowerRight;

            cornerList.Add(LowerLeft);
            cornerList.Add(UpperLeft);
            for (int i = 1; i < (numTotalVertices / 2) - 1; i++)
            {
                float width = (float)i / (numTotalVertices / 2.0f - 1);
                Vector3 v = UpperLeft + TopVector * (width);
                cornerList.Add(v);
            }
            cornerList.Add(UpperRight);
            cornerList.Add(LowerRight);
            for (int i = 1; i < (numTotalVertices / 2) - 1; i++)
            {
                float width = (float)i / (numTotalVertices / 2.0f - 1);
                Vector3 v = LowerRight + BottomVector * width;
                cornerList.Add(v);
            }

            return cornerList;
        }

        public Vector3 ConvertCornerToUVAndDepth(Vector3 vec)
        {
            Vector3 vp = MainCamera.WorldToViewportPoint(vec);
            return new Vector3(vp.x, vp.y, vec.z);
        }

        public Vector2 ConvertScreenToViewportSpace(Vector2 vec)
        {
            return new Vector2((vec.x + 1) / 2.0f, (vec.y + 1) / 2.0f);
        }

        public List<int> GetTableTriangles(int numVertices)
        {
            List<int> tableTriangles = new List<int>();

            //tableTriangles.AddRange(new int[3] { 0, 1, 2 });
            //tableTriangles.AddRange(new int[3] { 0, 2, 3 });

            //tableTriangles.AddRange(new int[3] { 0, 1, 11 });
            //tableTriangles.AddRange(new int[3] { 11, 1, 2 });
            //tableTriangles.AddRange(new int[3] { 11, 2, 10 });
            //tableTriangles.AddRange(new int[3] { 10, 2, 3 });
            //tableTriangles.AddRange(new int[3] { 10, 3, 9 });
            //tableTriangles.AddRange(new int[3] { 9, 3, 4 });
            //tableTriangles.AddRange(new int[3] { 9, 4, 8 });
            //tableTriangles.AddRange(new int[3] { 8, 4, 5 });
            //tableTriangles.AddRange(new int[3] { 8, 5, 7 });
            //tableTriangles.AddRange(new int[3] { 7, 5, 6 });

            int topIndex = 1;
            int bottomIndex = numVertices;
            for (int i = 0; i < (numVertices / 2) - 1; i++)
            {
                int[] triangle = new int[3] { bottomIndex % numVertices, topIndex, (bottomIndex - 1) % numVertices };
                bottomIndex--;
                int[] triangle2 = new int[3] { bottomIndex, topIndex, topIndex + 1 };
                topIndex++;

                tableTriangles.AddRange(triangle);
                tableTriangles.AddRange(triangle2);
            }

            return tableTriangles;
        }

        public List<Vector2> GetTableUVs(int numVertices)
        {
            List<Vector2> tableUVs = new List<Vector2>();

            Vector2 LLUV = new Vector2(0, 0);
            Vector2 ULUV = new Vector2(0, 1);
            Vector2 URUV = new Vector2(1, 1);
            Vector2 LRUV = new Vector2(1, 0);

            tableUVs.Add(LLUV);
            tableUVs.Add(ULUV);
            for (int i = 1; i < (numVertices / 2) - 1; i++)
            {
                float width = (float)i / (numVertices / 2.0f);
                Vector2 uv = new Vector2(width, 1);
                tableUVs.Add(uv);
            }
            //    for (float width = 0.2f; width < 0.9f; width += 0.2f)
            //{
            //    tableUVs.Add(new Vector2(width, 1));
            //}
            tableUVs.Add(URUV);
            tableUVs.Add(LRUV);
            for (int i = 1; i < (numVertices / 2) - 1; i++)
            {
                float width = (float)i / (numVertices / 2.0f);
                Vector2 uv = new Vector2(1 - width, 0);
                tableUVs.Add(uv);
            }
            //for (float width = 0.8f; width > 0.1f; width -= 0.2f)
            //{
            //    tableUVs.Add(new Vector2(width, 0));
            //}

            return tableUVs;
        }

        public Material GenerateTableMaterial()
        {
            Material m = new Material(Shader.Find("Custom/TableShader"));
            m.name = TableName;

            m.SetTexture("_MainTex", TableCamera.targetTexture);

            return m;
        }
    }
}