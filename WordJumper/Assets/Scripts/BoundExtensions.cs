using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoundExtensions 
{
    public static Bounds getObjectBounds(GameObject obj) 
    {
        return obj.GetComponent<Bounds>();
    }

    public static float getObjectWidth(GameObject obj) 
    {              
        return getObjectBounds(obj).size.x;
    }
 

  
}
