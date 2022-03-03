using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTile : Tile
{
    private void Awake()
    {
        tileType = TileType.Obstacle;
    }
}
