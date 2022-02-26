using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepieceBase : MonoBehaviour
{
    public int xIndex;
    public int yIndex;

    public void SetCoordinate( int x, int y)
    {
        xIndex = x;
        yIndex = y;
    }

    public void Move(int destX,int destY)
    {

    }
}
