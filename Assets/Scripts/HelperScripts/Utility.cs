using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility 
{
    public static bool IsNextTo(Tile clicked, Tile target)
    {
        if (Mathf.Abs(clicked.xIndex - target.xIndex) == 1 && clicked.yIndex == target.yIndex)
        {
            return true;
        }
        if (Mathf.Abs(clicked.yIndex - target.yIndex) == 1 && clicked.xIndex == target.xIndex)
        {
            return true;
        }
        return false;
    }

}
