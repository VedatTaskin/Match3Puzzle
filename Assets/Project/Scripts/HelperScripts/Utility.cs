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
                if (piece.transform.position.y -piece.yIndex >0.001 &&
                    piece.transform.position.x - piece.xIndex > 0.001)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public static bool GamepiecesAreCollapsed(Gamepiece[,] gamepieces)
    {
        int width = gamepieces.GetLength(0);
        int height = gamepieces.GetLength(1);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var piece = gamepieces[i, j];
                if (piece!= null)
                {
                    if (piece.transform.position.y - piece.yIndex > 0.001 &&
                            piece.transform.position.x - piece.xIndex > 0.001)
                    {
                        return false;
                    }

                }
            }
        }
        return true;
    }

}
