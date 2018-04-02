using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloakingBox
{
    public enum CloakLayers
    {
        Visible,
        Invisible
    };

    public class LayerManager : MonoBehaviour
    {
        public string VisibleLayer = "Visible";
        public string InvisibleLayer = "Invisible";
        
        public static string VisibleLayer_s = "Visible";
        public static string InvisibleLayer_s = "Invisible";

        // Update is called once per frame
        void Update()
        {
            if (!VisibleLayer_s.Equals(VisibleLayer))
            {
                // set the static layer to be the correct one
                VisibleLayer_s = VisibleLayer;
            }
            if (!InvisibleLayer_s.Equals(InvisibleLayer))
            {
                // set the static layer to be the correct one
                InvisibleLayer_s = InvisibleLayer;
            }
        }

        public static LayerMask GetLayerMask(CloakLayers layer)
        {
            LayerMask mask = LayerMask.NameToLayer("Default");
            switch (layer)
            {
                case CloakLayers.Visible:
                    mask = LayerMask.NameToLayer(VisibleLayer_s);
                    break;
                case CloakLayers.Invisible:
                    mask = LayerMask.NameToLayer(InvisibleLayer_s);
                    break;
            }
            return mask;
        }

        public static List<string> GetLayerNameList()
        {
            List<string> layerNameList = new List<string>();
            layerNameList.Add(VisibleLayer_s);
            layerNameList.Add(InvisibleLayer_s);

            return layerNameList;
        }
    }
}
