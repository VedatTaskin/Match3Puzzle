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

    public void DragToTile(Tile tile)
    {
        targetTile = tile;
    }

    public void ReleaseTile()
    {
        if (clickedTile != null && targetTile != null)
        {
            SwitchTiles(clickedTile, targetTile);
        }
        clickedTile = null;
        targetTile = null;
    }

    void SwitchTiles( Tile clickedTile, Tile targetTile)
    {

        Gamepiece clickedGamepiece = gamepieceData.allGamepieces[clickedTile.xIndex, clickedTile.yIndex];
        Gamepiece targetGamepiece = gamepieceData.allGamepieces[targetTile.xIndex, targetTile.yIndex];

        clickedGamepiece.Move(targetGamepiece.xIndex, targetGamepiece.yIndex, swapTime);
        targetGamepiece.Move(clickedGamepiece.xIndex, clickedGamepiece.yIndex, swapTime);

    }
}
