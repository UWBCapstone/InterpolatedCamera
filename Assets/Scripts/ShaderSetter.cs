using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{

    public class ShaderSetter : MonoBehaviour
    {
        public GameObject Viewer;

        public Vector2 LowerLeftUV;
        public Vector2 UpperLeftUV;
        public Vector2 UpperRightUV;
        public Vector2 LowerRightUV;

        public void Awake()
        {
            //UVCalc c = GameObject.Find("UVManager").GetComponent<UVCalc>();
            //LowerLeftUV = c.LowerLeft;
            //UpperLeftUV = c.UpperLeft;
            //UpperRightUV = c.UpperRight;
            //LowerRightUV = c.LowerRight;
        }

        public void Update()
        {
            //UVCalc c = GameObject.Find("UVManager").GetComponent<UVCalc>();
            //LowerLeftUV = c.LowerLeft;
            //UpperLeftUV = c.UpperLeft;
            //UpperRightUV = c.UpperRight;
            //LowerRightUV = c.LowerRight;

            Material mat = gameObject.GetComponent<Renderer>().material;
            mat.SetVector("_uvLL", LowerLeftUV);
            mat.SetVector("_uvUL", UpperLeftUV);
            mat.SetVector("_uvLR", LowerRightUV);
            mat.SetVector("_uvUR", UpperRightUV);
            //mat.SetVector("_uvLL", new Vector4(0, 0, 0, 0));
            //mat.SetVector("_uvUL", new Vector4(0, 1, 0, 0));
            //mat.SetVector("_uvLR", new Vector4(1, 0, 0, 0));

            //mat.mainTexture.mipMapBias = -100000000.0f;
        }
    }
}