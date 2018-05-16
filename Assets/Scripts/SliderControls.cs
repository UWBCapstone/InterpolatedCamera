using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InterpolatedCamera
{
    public class SliderControls : MonoBehaviour
    {
        public GameObject AggregateClipPlane;
        public GameObject WorldObjectParent;
        public GameObject MainCamera;
        public ViewingCamManager viewingCamManager;
        public Slider PosX;
        public Text XField;
        public float PosXMin = -100.0f;
        public float PosXMax = 100.0f;
        public Slider PosY;
        public Text YField;
        public float PosYMin = -20.0f;
        public float PosYMax = 20.0f;
        public Slider PosZ;
        public Text ZField;
        public float PosZMin = -15f;
        public float PosZMax = 15f;

        private float expectedFarClipPlane = 3.0f;
        private List<float> oppositeOrigs;
        private List<Vector3> origCamPosList;
        private List<Vector3> origObjScales;
        private Vector3 origCamPos;
        private Vector3 origCent;
        private List<Vector3> origViewingCamDistanceRatios; // ratio between "distance between viewing cam pos xy and planecenter xy" and "distance between projected point between maincam and viewingcam on plane and planecenter xy"
        private List<Vector3> origWorldObjectDistanceRatios;
        private bool initialized = false;
        //private Vector3 planeToCentDifference;

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
            Invoke("Init", 2.5f);
        }

        public void Init()
        {
            SetMinMax();
            
            GameObject mainCam = AggregateClipPlane.GetComponent<AggregateClipPlane>().MainCamera;
            origCamPos = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y, mainCam.transform.position.z);

            PosX.value = origCamPos.x;
            PosY.value = origCamPos.y;
            PosZ.value = origCamPos.z;
            //Rotation.value = 0;



            int numChildren = AggregateClipPlane.transform.childCount;
            if (numChildren.Equals(1)
                && AggregateClipPlane.transform.GetChild(0).gameObject.name.Equals(AggregateClipPlane.GetComponent<AggregateClipPlane>().PlaneName))
            {
                GameObject interpolatedPlane = AggregateClipPlane.transform.GetChild(0).gameObject;
                DebugPlaneRect dpr = interpolatedPlane.GetComponent<DebugPlaneRect>();
                Vector3 planeCent = dpr.Center;

                origCent = new Vector3(
                    planeCent.x,
                    planeCent.y,
                    planeCent.z);

                Debug.Log("origCent = " + origCent);


                origViewingCamDistanceRatios = new List<Vector3>();
                for (int i = 0; i < viewingCamManager.ViewingCameras.Length; i++)
                {
                    Vector3 simOrigCamPos = origCamPos - new Vector3(0, 0, -10);
                    GameObject viewingCam = viewingCamManager.ViewingCameras[i];
                    Ray r = new Ray(simOrigCamPos, viewingCam.transform.position - simOrigCamPos);
                    Vector3 pos = viewingCam.GetComponent<UVCalc>().IntersectRepresentativePlane(r, simOrigCamPos).point;
                    Ray camRay = new Ray(simOrigCamPos, mainCam.transform.forward);
                    Vector3 camPos = interpolatedPlane.GetComponent<UVCalc>().IntersectRepresentativePlane(camRay, simOrigCamPos).point;
                    float ratioX = (viewingCam.transform.position.x - camPos.x) / (pos.x - camPos.x);
                    float ratioY = (viewingCam.transform.position.y - camPos.y) / (pos.y - camPos.y);
                    origViewingCamDistanceRatios.Add(new Vector3(ratioX, ratioY, 0));
                }
            }
            else
            {
                origCent = new Vector3(
                    origCamPos.x,
                    origCamPos.y,
                    mainCam.GetComponent<Camera>().farClipPlane);

                Debug.Log("origCent = " + origCent);
            }

            origObjScales = new List<Vector3>();
            for(int i = 0; i < WorldObjectParent.transform.childCount; i++)
            {
                origObjScales.Add(WorldObjectParent.transform.GetChild(i).transform.localScale);
            }

            origCamPosList = new List<Vector3>();
            for(int i = 0; i < viewingCamManager.ViewingCameras.Length; i++)
            {
                origCamPosList.Add(viewingCamManager.ViewingCameras[i].transform.position);
            }

            oppositeOrigs = new List<float>();
            for(int i = 0; i < viewingCamManager.ViewingCameras.Length; i++)
            {
                PlaneRect viewingClipPlane = viewingCamManager.ViewingCameras[i].GetComponent<ClipPlaneManager>().ClipRect;
                Camera viewingCamCam = viewingCamManager.ViewingCameras[i].GetComponent<Camera>();
                float oppositeOrig = Mathf.Abs(viewingClipPlane.Corner00.x - viewingClipPlane.center.x);
                oppositeOrigs.Add(oppositeOrig/viewingCamCam.aspect);

                Debug.Log("Corner00x = " + viewingClipPlane.Corner00.x);
                Debug.Log("centerx = " + viewingClipPlane.center.x);
            }


                //int numChildren = AggregateClipPlane.transform.childCount;
                //if(numChildren.Equals(1)
                //    && AggregateClipPlane.transform.GetChild(0).gameObject.name.Equals(AggregateClipPlane.GetComponent<AggregateClipPlane>().PlaneName))
                //{

                //}
                //else
                //{
                //}

            XField.text = PosX.value.ToString();
            YField.text = PosY.value.ToString();
            ZField.text = PosZ.value.ToString();
            //RotField.text = Rotation.value.ToString();

            initialized = true;
        }

        public void Update()
        {
            if (initialized)
            {
                GameObject interpolatedPlane = AggregateClipPlane.transform.GetChild(0).gameObject;
                if (interpolatedPlane != null)
                {
                    GameObject MainCamera = AggregateClipPlane.GetComponent<AggregateClipPlane>().MainCamera;
                    Camera mainCam = MainCamera.GetComponent<Camera>();
                    DebugPlaneRect dpr = interpolatedPlane.GetComponent<DebugPlaneRect>();
                    float planeScale = mainCam.farClipPlane / expectedFarClipPlane;
                    Vector3 cent = dpr.Center;
                    //Vector3 dir = new Vector3(PosX.value, PosY.value, dpr.Center.z) - cent;
                    //Vector3 dir = -(new Vector3(PosX.value, PosY.value, PosZ.value) - ((cent - origCent) / planeScale));
                    Vector3 amtMoved = this.transform.position - Vector3.zero;
                    //Vector3 dir = (new Vector3(PosX.value, PosY.value, PosZ.value) - (cent - origCent));
                    Vector3 dir = (new Vector3(-PosX.value, -PosY.value, PosZ.value) - amtMoved);
                    //Debug.Log("OrigCent = " + origCent);
                    //dir = new Vector3(dir.x, dir.y, Mathf.Abs(dir.z));
                    //Vector3 planeDir = new Vector3(PosX.value * planeScale, PosY.value * planeScale, PosZ.value * planeScale) - (cent - origCent);

                    //Debug.Log("Cent = " + cent);
                    //Debug.Log("Dir = " + dir);
                    //Debug.Log("Cent now = " + dpr.Center);

                    // Prevent flipping of image when camera z position gets too low
                    //GameObject MainCamera = AggregateClipPlane.GetComponent<AggregateClipPlane>().MainCamera;
                    //Camera mainCam = MainCamera.GetComponent<Camera>();
                    //if (dir.z < mainCam.farClipPlane)
                    //{
                    //    dir.z = mainCam.farClipPlane + (mainCam.farClipPlane - dir.z); // -3.5 -> -2.5 when farClip = 3
                    //}

                    // Prevent showing of image when camera z position gets too low
                    if (PosZ.value < -mainCam.farClipPlane)
                    //if(PosZ.value < -expectedFarClipPlane)
                    {
                        //planeDir.z = 0;
                        dir.z = 0;
                        dpr.Translate(new Vector3(0, 0, -dpr.Center.z));
                    }
                    
                    //dpr.Translate(dir);
                    foreach(var viewingCam in viewingCamManager.ViewingCameras)
                    {
                        viewingCam.transform.Translate(dir);
                    }
                    WorldObjectParent.transform.Translate(dir);
                    this.transform.Translate(dir);

                    //AdjustViewingCams();

                    //// Now to calculate each viewingCam's change
                    //for (int i = 0; i < viewingCamManager.ViewingCameras.Length; i++)
                    //{
                    //    if (viewingCamManager.ViewingCameras != null)
                    //    {
                    //        Vector3 origCamPos = origCamPosList[i];
                    //        Vector3 camPos = viewingCamManager.ViewingCameras[i].transform.position;
                    //        //Vector3 camDir = (cent - origCent) - (camPos - origCamPos);
                    //        float shrinkFactor = (dpr.Center.z - origCent.z) / (mainCam.transform.position.z - origCent.z);
                    //        //Vector3 camDir = (mainCam.transform.position - camPos) * shrinkFactor;
                    //        Vector3 camDir = ((mainCam.transform.position - origCamPos) * shrinkFactor);// - camPos;

                    //        if (shrinkFactor != 0)
                    //        {
                    //            Debug.Log("CamDir = " + camDir);
                    //            Debug.Log("shrinkFactor = " + shrinkFactor);
                    //        }

                    //        //viewingCamManager.ViewingCameras[i].transform.Translate(camDir);
                    //        viewingCamManager.ViewingCameras[i].transform.position = camDir;
                    //    }
                    //}



                    //Vector3 simCamPos = dpr.Center - origCent + origCamPos;

                    //// Translate the UV plane's simulated position
                    ////dpr.Translate(dir);
                    //// Translate the x/y by how far the z is
                    ////float PullFactor = Mathf.Abs(1/(PosZ.value - origCamPos.z + 1));
                    //if (dir.z != 0.0f)
                    //{
                    //    for(int i = 0; i < viewingCamManager.ViewingCameras.Length; i++)
                    //    {
                    //        Vector3 tempSimCamPos = simCamPos - new Vector3(0, 0, -10);
                    //        Ray ray = new Ray(tempSimCamPos, viewingCamManager.ViewingCameras[i].transform.position - tempSimCamPos);
                    //        //Ray rayNew = new Ray(simCamPos, viewingCamManager.ViewingCameras[i].transform.position - simCamPos);
                    //        Vector3 pos = viewingCamManager.ViewingCameras[i].GetComponent<UVCalc>().IntersectRepresentativePlane(ray, tempSimCamPos).point;
                    //        if(origViewingCamDistanceRatios == null)
                    //        {
                    //            origViewingCamDistanceRatios = new List<Vector3>();
                    //            for (int j = 0; j < viewingCamManager.ViewingCameras.Length; j++)
                    //            {
                    //                Vector3 simOrigCamPos = origCamPos - new Vector3(0, 0, -10);
                    //                GameObject viewingCam = viewingCamManager.ViewingCameras[j];
                    //                Ray r0 = new Ray(simOrigCamPos, viewingCam.transform.position - simOrigCamPos);
                    //                Vector3 pos0 = viewingCam.GetComponent<UVCalc>().IntersectRepresentativePlane(r0, simOrigCamPos).point;
                    //                Ray camRay = new Ray(simOrigCamPos, mainCam.transform.forward);
                    //                Vector3 camPos = interpolatedPlane.GetComponent<UVCalc>().IntersectRepresentativePlane(camRay, simOrigCamPos).point;
                    //                float ratioX = (viewingCam.transform.position.x - camPos.x) / (pos0.x - camPos.x);
                    //                float ratioY = (viewingCam.transform.position.y - camPos.y) / (pos0.y - camPos.y);
                    //                origViewingCamDistanceRatios.Add(new Vector3(ratioX, ratioY, 0));
                    //            }
                    //        }
                    //        Vector3 ratio = origViewingCamDistanceRatios[i];
                    //        if (float.IsNaN(ratio.x) || float.IsNaN(ratio.y))
                    //        {
                    //            GameObject viewingCam = viewingCamManager.ViewingCameras[i];
                    //            Ray camRay = new Ray(tempSimCamPos, mainCam.transform.forward);
                    //            Vector3 camPos = interpolatedPlane.GetComponent<UVCalc>().IntersectRepresentativePlane(camRay, tempSimCamPos).point;
                    //            float ratioX = (viewingCam.transform.position.x - camPos.x) / (pos.x - camPos.x);
                    //            float ratioY = (viewingCam.transform.position.y - camPos.y) / (pos.y - camPos.y);
                    //            origViewingCamDistanceRatios[i] = new Vector3(ratioX, ratioY, 0);
                    //            Debug.Log("New cam ratio = " + new Vector3(ratioX, ratioY, 0));
                    //            ratio = origViewingCamDistanceRatios[i];
                    //            Debug.Log("Stored ratio = " + origViewingCamDistanceRatios[i]);
                    //        }
                    //        float viewingCamPosX = (pos.x - tempSimCamPos.x) * ratio.x;
                    //        float viewingCamPosY = (pos.y - tempSimCamPos.y) * ratio.y;
                    //        Vector3 viewingCamPosXY = new Vector3(viewingCamPosX, viewingCamPosY, 0);

                    //        viewingCamManager.ViewingCameras[i].transform.position = viewingCamPosXY;
                    //    }

                    //    float PullFactor = (origCamPos.z - PosZ.value) * 0.25f;
                    //    Ray r = new Ray(mainCam.transform.position, dpr.Center - mainCam.transform.position);
                    //    //Debug.Log("Pull Factor = " + PullFactor);

                    //    Vector3 pullVec = new Vector3(origCent.x - dpr.Center.x, origCent.y - dpr.Center.y, 0);
                    //    //dpr.Translate(new Vector3(PullFactor * dpr.Center.x, PullFactor * dpr.Center.y, 0));
                    //    //dpr.Translate(pullVec * PullFactor + dir);
                    //    dpr.Translate(dir);
                    //}
                    //else
                    //{
                    //    dpr.Translate(dir);
                    //}

                    //// Translate the world objects to simulate the camera moving and changing perspective
                    ////WorldObjectParent.transform.Translate(-dir);
                    ////WorldObjectParent.transform.Translate(-new Vector3(dir.x, dir.y, 0));
                    //// Factor z change to shrink objects to a point
                    //if (PosZ.value < -mainCam.farClipPlane)
                    //{
                    //    WorldObjectParent.transform.localScale = Vector3.zero;
                    //}
                    //else
                    //{
                    //    float shrinkFactor = (mainCam.farClipPlane + PosZ.value) / mainCam.farClipPlane;
                    //    //Debug.Log("ShrinkFactor = " + shrinkFactor);
                    //    WorldObjectParent.transform.localScale = Vector3.one;

                    //    for(int i = 0; i < WorldObjectParent.transform.childCount; i++)
                    //    {
                    //        Vector3 origScale = origObjScales[i];
                    //        GameObject child = WorldObjectParent.transform.GetChild(i).gameObject;
                    //        child.transform.localScale = origScale * shrinkFactor;

                    //        float PullFactor = (origCamPos.z - PosZ.value);

                    //        child.transform.Translate(-new Vector3(PullFactor * dir.x, PullFactor * dir.y, 0));
                    //        //Debug.Log("dir.x = " + dir.x);
                    //        //Debug.Log("PullFactor * dir.x = " + PullFactor * dir.x);
                    //    }
                    //}


                    //// Translate all world objects as well
                    //// Rotate all world objects?

                    ////Debug.Log("Dir = " + dir);
                    ////Debug.Log("origCent = " + origCent);
                    ////Debug.Log("cent = " + cent);

                    XField.text = PosX.value.ToString();
                    YField.text = PosY.value.ToString();
                    ZField.text = PosZ.value.ToString();
                    //RotField.text = Rotation.value.ToString();
                }
            }
        }

        public void SetMinMax()
        {
            PosX.minValue = PosXMin;
            PosX.maxValue = PosXMax;
            PosY.minValue = PosYMin;
            PosY.maxValue = PosYMax;
            PosZ.minValue = PosZMin;
            PosZ.maxValue = PosZMax;
            //Rotation.minValue = RotMin;
            //Rotation.maxValue = RotMax;
        }

        public void AddOrigObjScale(Vector3 localScale)
        {
            origObjScales.Add(localScale);
        }

        /// <summary>
        /// Impossible to fix with current implementation without tearing down the project and starting over. It seems that the representative plane constructed in teh viewing camera's uvcalc script is causing UV calculations to go awry. If we pushed the representative plane to be back as far as the main camera's interpolated plane, this should calculate correctly. However, I don't know what else this will mess up in my project's logic.
        /// </summary>
        public void AdjustViewingCams()
        {
            int numChildren = AggregateClipPlane.transform.childCount;
            if (numChildren.Equals(1)
                && AggregateClipPlane.transform.GetChild(0).gameObject.name.Equals(AggregateClipPlane.GetComponent<AggregateClipPlane>().PlaneName))
            {
                for (int i = 0; i < viewingCamManager.ViewingCameras.Length; i++)
                {
                    GameObject viewingCam = viewingCamManager.ViewingCameras[i];
                    Camera mainCam = MainCamera.GetComponent<Camera>();
                    
                    //float mainCamPointOnPlane = (mainCam.transform.position + mainCam.transform.forward * mainCam.farClipPlane).x;
                    PlaneRect viewingClipPlane = viewingCam.GetComponent<ClipPlaneManager>().ClipRect;
                    //float oppositeOrig = Mathf.Abs(viewingClipPlane.Corner00.x - viewingClipPlane.center.x);
                    float oppositeOrig = oppositeOrigs[i];

                    Ray ray = new Ray(viewingCam.transform.position, viewingCam.transform.forward);

                    GameObject interpolatedPlane = AggregateClipPlane.transform.GetChild(0).gameObject;
                    DebugPlaneRect dpr = interpolatedPlane.GetComponent<DebugPlaneRect>();

                    RaycastHit hit = dpr.Intersect(ray, viewingCam.transform.position);
                    float adjacentNew = Mathf.Abs((hit.point - viewingCam.transform.position).magnitude);
                    
                    float theta = (Mathf.Tan(oppositeOrig / adjacentNew) * Mathf.Rad2Deg) * 2;
                    
                    if (theta != viewingCam.GetComponent<Camera>().fieldOfView)
                    {
                        //Debug.Log("Original opposite = " + oppositeOrig);
                        //Debug.Log("Intersect hits at " + hit.point);
                        Debug.Log("New adjacent value = " + adjacentNew);
                        //Debug.Log("orig theta = " + viewingCam.GetComponent<Camera>().fieldOfView);
                        Debug.Log("new theta = " + theta);

                        //Debug.Log("Updating camera for " + viewingCam.name);
                    }
                    viewingCam.GetComponent<Camera>().fieldOfView = theta;
                    viewingCam.GetComponent<ClipPlaneManager>().UpdateInfo(viewingCam.GetComponent<Camera>());

                }
            }
        }
    }
}