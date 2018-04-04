using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InterpolatedCamera
{
    public class SliderControls : MonoBehaviour
    {
        public GameObject AggregateClipPlane;
        public Slider PosX;
        public Text XField;
        public float PosXMin = -100.0f;
        public float PosXMax = 100.0f;
        public Slider PosY;
        public Text YField;
        public float PosYMin = -20.0f;
        public float PosYMax = 20.0f;
        //public Slider Rotation;
        //public Text RotField;
        //public float RotMin = -30.0f;
        //public float RotMax = 30.0f;


        //public float DeltaX = 2.5f;
        //public float DeltaY = 0.2f;
        //public float DeltaRot = 15;

        public void Awake()
        {
            SetMinMax();

            GameObject mainCam = AggregateClipPlane.GetComponent<AggregateClipPlane>().MainCamera;

            PosX.value = mainCam.transform.position.x;
            PosY.value = mainCam.transform.position.y;
            //Rotation.value = 0;

            XField.text = PosX.value.ToString();
            YField.text = PosY.value.ToString();
            //RotField.text = Rotation.value.ToString();
        }

        public void Update()
        {
            SetMinMax();

            GameObject interpolatedPlane = AggregateClipPlane.transform.GetChild(0).gameObject;
            if (interpolatedPlane != null)
            {
                DebugPlaneRect dpr = interpolatedPlane.GetComponent<DebugPlaneRect>();
                Vector3 cent = dpr.Center;
                Vector3 dir = new Vector3(PosX.value, PosY.value, dpr.Center.z) - cent;

                //Debug.Log("Cent = " + cent);
                //Debug.Log("Dir = " + dir);
                //Debug.Log("Cent now = " + dpr.Center);
                
                dpr.Translate(dir);
                // Translate all world objects as well
                // Rotate all world objects?

                XField.text = PosX.value.ToString();
                YField.text = PosY.value.ToString();
                //RotField.text = Rotation.value.ToString();
            }
        }

        public void SetMinMax()
        {
            PosX.minValue = PosXMin;
            PosX.maxValue = PosXMax;
            PosY.minValue = PosYMin;
            PosY.maxValue = PosYMax;
            //Rotation.minValue = RotMin;
            //Rotation.maxValue = RotMax;
        }
    }
}