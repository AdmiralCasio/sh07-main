using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationVisibility 
{
    static bool IsVisible(Vector3 target,Camera camera){
        Vector3 screenPoint=camera.WorldToViewportPoint(target);
        if((screenPoint.x>0 && screenPoint.x<1)&&(screenPoint.y>0 && screenPoint.y<1) &&(screenPoint.z>0)){
            return true;
        }
        return false;
        }
    }
