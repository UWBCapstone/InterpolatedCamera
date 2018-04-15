using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterpolatedCamera
{
    public class TableManager : MonoBehaviour
    {
        public Camera MainCamera;
        public Camera TableCamera;
        public GameObject WorldObjectParent;
        public GameObject Table;
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
            List<Vector3> tableCorners = GetTableCorners();
            List<int> tableTriangles = GetTableTriangles();
            List<Vector2> tableUVs = GetTableUVs();

            // Assign them to the mesh
            m.SetVertices(tableCorners);
            m.SetTriangles(tableTriangles.ToArray(), 0);
            m.SetUVs(0, tableUVs);
            m.SetUVs(1, tableUVs);

            // Return the mesh
            return m;
        }

        public List<Vector3> GetTableCorners()
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

            // Add to corner list
            cornerList.Add(LL);
            cornerList.Add(UL);
            cornerList.Add(UR);
            cornerList.Add(LR);

            return cornerList;
        }

        public Vector2 ConvertScreenToViewportSpace(Vector2 vec)
        {
            return new Vector2((vec.x + 1)/2.0f, (vec.y + 1)/ 2.0f);
        }

        public List<int> GetTableTriangles()
        {
            List<int> tableTriangles = new List<int>();

            tableTriangles.AddRange(new int[3] { 0, 1, 2 });
            tableTriangles.AddRange(new int[3] { 0, 2, 3 });

            return tableTriangles;
        }

        public List<Vector2> GetTableUVs()
        {
            List<Vector2> tableUVs = new List<Vector2>();

            Vector2 LLUV = new Vector2(0, 0);
            Vector2 ULUV = new Vector2(0, 1);
            Vector2 URUV = new Vector2(1, 1);
            Vector2 LRUV = new Vector2(1, 0);

            tableUVs.Add(LLUV);
            tableUVs.Add(ULUV);
            tableUVs.Add(URUV);
            tableUVs.Add(LRUV);

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