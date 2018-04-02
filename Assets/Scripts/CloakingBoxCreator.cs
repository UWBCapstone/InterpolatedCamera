using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public enum BoxFaceNames
    {
        Front,
        Back,
        Left,
        Right,
        Top,
        Bottom
    }

    public class CloakingBoxCreator : MonoBehaviour
    {
        public static Mesh cubeMesh;

        public GameObject ViewingCamera;
        public GameObject RenderTextureCamera;
        private Vector3 boxFaceDimensions = new Vector3(.5f, .5f, .0000001f);

        public GameObject GenerateCloakingBox(Vector3 position)
        {
            GameObject cloakingBox = generateCloakingBoxObject();
            cloakingBox.transform.position = position;

            // Attach it to the UVManager's tracking
            GameObject.Find("UVManager").GetComponent<UVCalc>().CloakingBoxes.Add(cloakingBox);

            return cloakingBox;
        }

        private GameObject generateCloakingBoxObject()
        {
            // Make the cloaking box and disable it so it doesn't show 
            // immediately while loading
            GameObject cloakingBox = new GameObject();
            cloakingBox.SetActive(false);
            cloakingBox.name = "CloakingBox";

            // Set box layer to invisible to avoid having it seen by the render
            // texture camera
            cloakingBox.layer = LayerManager.GetLayerMask(CloakLayers.Invisible);

            // Generate the box faces
            GameObject frontFace = generateCloakingBoxFaceObject(BoxFaceNames.Front);
            GameObject backFace = generateCloakingBoxFaceObject(BoxFaceNames.Back);
            GameObject leftFace = generateCloakingBoxFaceObject(BoxFaceNames.Left);
            GameObject rightFace = generateCloakingBoxFaceObject(BoxFaceNames.Right);
            GameObject topFace = generateCloakingBoxFaceObject(BoxFaceNames.Top);
            GameObject bottomFace = generateCloakingBoxFaceObject(BoxFaceNames.Bottom);

            // Set the faces as children of the cloaking box
            frontFace.transform.parent = cloakingBox.transform;
            backFace.transform.parent = cloakingBox.transform;
            leftFace.transform.parent = cloakingBox.transform;
            rightFace.transform.parent = cloakingBox.transform;
            topFace.transform.parent = cloakingBox.transform;
            bottomFace.transform.parent = cloakingBox.transform;

            // Reactivate the cloaking box components and the box itself
            frontFace.SetActive(true);
            backFace.SetActive(true);
            leftFace.SetActive(true);
            rightFace.SetActive(true);
            topFace.SetActive(true);
            bottomFace.SetActive(true);
            cloakingBox.SetActive(true);

            return cloakingBox;
        }

        private GameObject generateCloakingBoxFaceObject(BoxFaceNames faceName)
        {
            GameObject boxFace = generateBaseCloakingBoxFaceObject();
            Material boxFaceMat = generateBoxFaceMaterial(faceName);
            var mr = boxFace.GetComponent<MeshRenderer>();
            if(mr != null)
            {
                mr.material = boxFaceMat;
            }

            // Adjust the box face according to which face it is supposed to be
            // (pos, rot)
            boxFace.transform.position = getLocalFaceTranslation(faceName);
            RotateFace(boxFace, faceName);

            // Set name
            setCloakingBoxFaceName(boxFace, faceName);

            return boxFace;
        }

        private GameObject generateBaseCloakingBoxFaceObject()
        {
            GameObject boxFace = new GameObject();
            boxFace.SetActive(false);

            // Generate the mesh
            var mf = boxFace.AddComponent<MeshFilter>();
            if(cubeMesh == null)
            {
                cacheCubeMesh();
            }
            mf.mesh = cubeMesh;
            boxFace.transform.localScale = boxFaceDimensions;

            // Add box's mesh renderer (material needs to be generated and 
            // set correctly by determining what box face it's gonna be 
            // associated with)
            var mr = boxFace.AddComponent<MeshRenderer>();
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            // Add necessary scripts
            boxFace.AddComponent<BoundCalc>();
            ShaderSetter ss = boxFace.AddComponent<ShaderSetter>();
            ss.Viewer = ViewingCamera;

            // Set box layer to invisible to avoid having it seen by the render
            // texture camera
            boxFace.layer = LayerManager.GetLayerMask(CloakLayers.Invisible);

            return boxFace;
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

        private Material generateBoxFaceMaterial(BoxFaceNames faceName)
        {
            Material boxFaceMat = new Material(Shader.Find("Custom/BoxFaceShader"));

            // Get the render texture to be shown
            if (RenderTextureCamera != null)
            {
                Camera renderCam = RenderTextureCamera.GetComponent<Camera>();
                if (renderCam != null)
                {
                    Texture RT = renderCam.targetTexture;
                    boxFaceMat.SetTexture("_MainTex", RT);
                }
                else
                {
                    Debug.LogError("Render Texture Camera has no camera component.");
                }
            }
            else
            {
                Debug.LogError("Need to assign render camera (RenderTextureCamera object).");
            }

            // Set the name appropriately based on whatever face it's going to 
            // be associated with
            switch (faceName)
            {
                case BoxFaceNames.Front:
                    boxFaceMat.name = "FrontMat";
                    break;
                case BoxFaceNames.Back:
                    boxFaceMat.name = "BackMat";
                    break;
                case BoxFaceNames.Left:
                    boxFaceMat.name = "LeftMat";
                    break;
                case BoxFaceNames.Right:
                    boxFaceMat.name = "RightMat";
                    break;
                case BoxFaceNames.Top:
                    boxFaceMat.name = "TopMat";
                    break;
                case BoxFaceNames.Bottom:
                    boxFaceMat.name = "BottomMat";
                    break;
            }

            return boxFaceMat;
        }

        private Vector3 getLocalFaceTranslation(BoxFaceNames faceName)
        {
            Vector3 faceTranslation = new Vector3();

            switch (faceName)
            {
                case BoxFaceNames.Front:
                    faceTranslation = new Vector3(0, 0, -boxFaceDimensions.x / 2f);
                    break;
                case BoxFaceNames.Back:
                    faceTranslation = new Vector3(0, 0, boxFaceDimensions.x / 2f);
                    break;
                case BoxFaceNames.Left:
                    faceTranslation = new Vector3(-boxFaceDimensions.x / 2f, 0, 0);
                    break;
                case BoxFaceNames.Right:
                    faceTranslation = new Vector3(boxFaceDimensions.x / 2f, 0, 0);
                    break;
                case BoxFaceNames.Top:
                    faceTranslation = new Vector3(0, boxFaceDimensions.y / 2f, 0);
                    break;
                case BoxFaceNames.Bottom:
                    faceTranslation = new Vector3(0, -boxFaceDimensions.y / 2f, 0);
                    break;
            }

            return faceTranslation;
        }

        public void RotateFace(GameObject boxFace, BoxFaceNames faceName)
        {
            // Similar method in BoundCalc -> GetQuaternion...
            if(boxFace != null)
            {
                switch (faceName)
                {
                    case BoxFaceNames.Front:
                        boxFace.transform.Rotate(new Vector3(0, 1, 0), 180f);
                        break;
                    case BoxFaceNames.Back:
                        boxFace.transform.Rotate(new Vector3(0, 1, 0), 0f);
                        break;
                    case BoxFaceNames.Left:
                        boxFace.transform.Rotate(new Vector3(0, 1, 0), -90f);
                        break;
                    case BoxFaceNames.Right:
                        boxFace.transform.Rotate(new Vector3(0, 1, 0), 90f);
                        break;
                    case BoxFaceNames.Top:
                        boxFace.transform.Rotate(new Vector3(1, 0, 0), 90f);
                        boxFace.transform.Rotate(new Vector3(0, 0, 1), 180f);
                        break;
                    case BoxFaceNames.Bottom:
                        boxFace.transform.Rotate(new Vector3(1, 0, 0), -90f);
                        boxFace.transform.Rotate(new Vector3(0, 0, 1), 180f);
                        break;
                }
            }
        }

        private void setCloakingBoxFaceName(GameObject boxFace, BoxFaceNames faceName)
        {
            switch (faceName)
            {
                case BoxFaceNames.Front:
                    boxFace.name = "FrontFace";
                    break;
                case BoxFaceNames.Back:
                    boxFace.name = "BackFace";
                    break;
                case BoxFaceNames.Left:
                    boxFace.name = "LeftFace";
                    break;
                case BoxFaceNames.Right:
                    boxFace.name = "RightFace";
                    break;
                case BoxFaceNames.Top:
                    boxFace.name = "TopFace";
                    break;
                case BoxFaceNames.Bottom:
                    boxFace.name = "BottomFace";
                    break;
            }
        }

        public static BoxFaceNames GetCloakingBoxFaceName(GameObject boxFace)
        {
            if (boxFace.name.Contains("FrontFace"))
            {
                return BoxFaceNames.Front;
            }
            else if (boxFace.name.Contains("BackFace"))
            {
                return BoxFaceNames.Back;
            }
            else if (boxFace.name.Contains("LeftFace"))
            {
                return BoxFaceNames.Left;
            }
            else if (boxFace.name.Contains("RightFace"))
            {
                return BoxFaceNames.Right;
            }
            else if (boxFace.name.Contains("TopFace"))
            {
                return BoxFaceNames.Top;
            }
            else if (boxFace.name.Contains("BottomFace"))
            {
                return BoxFaceNames.Bottom;
            }
            else
            {
                Debug.LogError("BoxFaceName not found.");
                return 0;
            }
        }
    }
}
