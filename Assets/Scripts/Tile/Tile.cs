﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int xIndex;
    public int yIndex;
    [HideInInspector] public TileType tileType;

    Board board;

    public void Init(int x, int y, Board board)
    {
        xIndex = x;
        yIndex = y;
        this.board = board;
    }


    private void OnMouseDown()
    {
        board.ClickTile(this);
    }

    private void OnMouseEnter()
    {
        board.DragToTile(this);
    }

    private void OnMouseUp()
    {
        board.ReleaseTile();
    }

    
}