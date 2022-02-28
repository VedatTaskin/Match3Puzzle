using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    int width;
    int height;

    public int borderSize;
    [Range(0, 1)]
    public float swapTime = 0.5f;

    public GamepieceData gamepieceData;
    public TileData tileData;

    Tile clickedTile;
    Tile targetTile;

    private void Start()
    {
        width = tileData.width;
        height = tileData.height;
        tileData.SetupTiles(this);
        SetupCamera();
        FillRandom();
    }


    void SetupCamera()
    {
        Camera.main.transform.position = new Vector3( (float)(width-1) / 2, (float)(height-1) / 2, -10f);

        float aspectRatio = (float) Screen.width / (float) Screen.height;

        float verticalSize = (float) height / 2f + (float) borderSize;

        float horizontalSize = ((float)width / 2f + (float)borderSize) / aspectRatio;

        Camera.main.orthographicSize = (verticalSize > horizontalSize) ? verticalSize : horizontalSize;
    
    }

    public void PlaceGamePiece(Gamepiece gamePiece,int x, int y)
    {
        if (gamePiece == null)
        {
            Debug.LogWarning("BOARD: invalid gamepiece");
            return;
        }
        gamePiece.transform.position = new Vector3(x, y, 0);
        gamePiece.transform.rotation = Quaternion.identity;
        if (IsWithInBounds(x,y))
        {
            gamepieceData.allGamepieces[x, y] = gamePiece;
        }
        gamePiece.SetCoordinate(x, y);
    }

    bool IsWithInBounds(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0 && y < height);

    }

    void FillRandom()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject randomPiece = Instantiate(gamepieceData.GetRandomGamePiece(), Vector3.zero, Quaternion.identity) as GameObject;
                randomPiece.transform.parent = transform;

                if (randomPiece !=null)
                {
                    randomPiece.GetComponent<Gamepiece>().Init(this);
                    PlaceGamePiece(randomPiece.GetComponent<Gamepiece>(), i, j);
                    transform.parent = transform;
                }

            }

        }
    }

    public void ClickTile(Tile tile)
    {
        clickedTile = tile;
    }

    public void DragToTile(Tile _tile)
    {
        if (clickedTile !=null && isNextTo(clickedTile,_tile))
        {
            targetTile = _tile;
            SwitchTiles(clickedTile, targetTile);
        }
    }


    void SwitchTiles( Tile _clickedTile, Tile _targetTile)
    {

        Gamepiece clickedGamepiece = gamepieceData.allGamepieces[_clickedTile.xIndex, _clickedTile.yIndex];
        Gamepiece targetGamepiece = gamepieceData.allGamepieces[_targetTile.xIndex, _targetTile.yIndex];

        clickedGamepiece.Move(targetGamepiece.xIndex, targetGamepiece.yIndex, swapTime);
        targetGamepiece.Move(clickedGamepiece.xIndex, clickedGamepiece.yIndex, swapTime);

        clickedTile = null;
        targetTile = null;
    }

    bool isNextTo(Tile clicked, Tile target)
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
