using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public class TouchScreenManager : MonoBehaviour
    {

        public static CloakingBoxCreator cloakingBoxCreator;
        
        public void Awake()
        {
            cloakingBoxCreator = GameObject.Find("CloakingBoxCreatorManager").GetComponent<CloakingBoxCreator>();

            //
            GameObject cloakingBox = cloakingBoxCreator.GenerateCloakingBox(new Vector3());
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.touchSupported)
            {
                bool touched = false;

                if(Input.touches.Length > 0)
                {
                    foreach(Touch touch in Input.touches)
                    {
                        if(touch.phase == TouchPhase.Began)
                        {
                            touched = true;
                            break;
                        }
                    }
                }

                if (touched)
                {
                    GameObject portal = cloakingBoxCreator.GenerateCloakingBox(new Vector3());
                }
            }
        }
    }
}