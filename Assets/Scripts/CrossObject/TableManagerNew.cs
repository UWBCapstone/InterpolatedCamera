using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterpolatedCamera
{
    public class TableManagerNew: MonoBehaviour
    {
        public Camera MainCamera;
        public Camera TableCamera;
        public GameObject WorldObjectParent;
        public GameObject Table;
        public int NumberOfVertices = 10000;
        public string TableName = "Table";
        public Vector2 LowerLeftUV = new Vector2();
        public float LowerLeftZ = 0.3f;
        public Vector2 UpperLeftUV = new Vector2();
        public float UpperLeftZ = 1;
        public Vector2 UpperRightUV = new Vector2();
        public float UpperRightZ = 1;
        public Vector2 LowerRightUV = new Vector2();
        public float LowerRightZ = 0.3f;

        public void Awake()
        {
            Table = MakeTable();
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

            //// Convert the points to viewport space
            //Vector2 LLViewport = ConvertUVToViewportSpace(LowerLeftUV);
            //Vector2 ULViewport = ConvertUVToViewportSpace(UpperLeftUV);
            //Vector2 URViewport = ConvertUVToViewportSpace(UpperRightUV);
            //Vector2 LRViewport = ConvertUVToViewportSpace(LowerRightUV);

            Vector2 LLViewport = LowerLeftUV;
            Vector2 ULViewport = UpperLeftUV;
            Vector2 URViewport = UpperRightUV;
            Vector2 LRViewport = LowerRightUV;

            // Convert viewport Vector2s to world space Vector3s using depth as the z value
            Vector3 LL3 = new Vector3(LLViewport.x, LLViewport.y, LowerLeftZ);
            Vector3 UL3 = new Vector3(ULViewport.x, ULViewport.y, UpperLeftZ);
            Vector3 UR3 = new Vector3(URViewport.x, URViewport.y, UpperRightZ);
            Vector3 LR3 = new Vector3(LRViewport.x, LRViewport.y, LowerRightZ);

            // Convert viewport space to world space
            Vector3 LL = MainCamera.ViewportToWorldPoint(LL3);
            Vector3 UL = MainCamera.ViewportToWorldPoint(UL3);
            Vector3 UR = MainCamera.ViewportToWorldPoint(UR3);
            Vector3 LR = MainCamera.ViewportToWorldPoint(LR3);

            Vector3 TopVector = UR - UL;
            Vector3 BottomVector = LL - LR;

            // Add to corner list
            cornerList.Add(LL);
            cornerList.Add(UL);
            for(int i = 1; i < (numTotalVertices / 2) - 1; i++)
            {
                float width = (float)i / (numTotalVertices / 2.0f - 1);
                Vector3 v = UL + TopVector * (width);
                cornerList.Add(v);
            }
            //for(float width = 0.2f; width < 0.9f; width += 0.2f)
            //{
            //    Vector3 v = UL + TopVector * width;
            //    cornerList.Add(v);
            //    Debug.Log("Adding top vertex " + v);
            //}
            cornerList.Add(UR);
            cornerList.Add(LR);
            for (int i = 1; i < (numTotalVertices / 2) - 1; i++)
            {
                float width = (float)i / (numTotalVertices / 2.0f - 1);
                Vector3 v = LR + BottomVector * width;
                cornerList.Add(v);
            }
            //for(float width = 0.2f; width < 0.9f; width += 0.2f)
            //{
            //    cornerList.Add(LR + BottomVector * width);
            //    Debug.Log("Adding bottom vertex " + (LR + BottomVector * width));
            //}

            return cornerList;
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
            for(int i = 0; i < (numVertices / 2)-1; i++)
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