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

    public static bool GamepiecesAreCollapsed(List<Gamepiece> gamepieces)
    {
        foreach (var piece in gamepieces)
        {
            if (piece != null)
            {
                if (piece.transform.position.y -piece.yIndex >0.001)
                {
                    return false;
                }
            }
        }
        return true;
    }

}
