using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterpolatedCamera
{
    public class ClipPlaneXComparer : IComparer<ClipPlaneManager>
    {
        int IComparer<ClipPlaneManager>.Compare(ClipPlaneManager x, ClipPlaneManager y)
        {
            PlaneRect xRect = x.clipPlane;
            PlaneRect yRect = y.clipPlane;

            if(xRect.center.x > yRect.center.x)
            {
                return 1;
            }
            else if(xRect.center.x < yRect.center.x)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static IComparer<ClipPlaneManager> SortByX()
        {
            return (IComparer<ClipPlaneManager>)new ClipPlaneXComparer();
        }
    }
}