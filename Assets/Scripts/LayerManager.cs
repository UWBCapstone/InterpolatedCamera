using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterpolatedCamera
{
    public enum CloakLayers
    {
        Visible,
        Invisible,
        TableView,
        Table
    };

    public class LayerManager : MonoBehaviour
    {
        public string VisibleLayer = "Visible";
        public string InvisibleLayer = "Invisible";
        public string TableViewLayer = "TableView";
        public string TableLayer = "Table";
        
        public static string VisibleLayer_s = "Visible";
        public static string InvisibleLayer_s = "Invisible";
        public static string TableViewLayer_s = "TableView";
        public static string TableLayer_s = "Table";

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
            if (!TableViewLayer_s.Equals(TableViewLayer))
            {
                TableViewLayer_s = TableViewLayer;
            }
            if (!TableLayer_s.Equals(TableLayer))
            {
                TableLayer_s = TableLayer;
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
                case CloakLayers.TableView:
                    mask = LayerMask.NameToLayer(TableViewLayer_s);
                    break;
                case CloakLayers.Table:
                    mask = LayerMask.NameToLayer(TableLayer_s);
                    break;
            }
            return mask;
        }

        public static List<string> GetLayerNameList()
        {
            List<string> layerNameList = new List<string>();
            layerNameList.Add(VisibleLayer_s);
            layerNameList.Add(InvisibleLayer_s);
            layerNameList.Add(TableViewLayer_s);
            layerNameList.Add(TableLayer_s);

            return layerNameList;
        }
    }
}
