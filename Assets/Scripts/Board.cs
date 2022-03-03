using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Board : MonoBehaviour
{
    int width;
    int height;

    public int borderSize;
    [Range(0, 1)]
    public float swapTime = 0.5f;
    public float fallTime = 0.5f;

    public GamepieceData gamepieceData;
    public TileData tileData;

    GameState gameState;
    Tile clickedTile;
    Tile targetTile;

    private void Start()
    {
        width = tileData.width;
        height = tileData.height;
        tileData.SetupTiles(this);
        SetupCamera();
        FillBoard();
        gameState = GameState.CanSwap;
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
        gamePiece.transform.position = new Vector3(x, y , 0);
        gamePiece.transform.rotation = Quaternion.identity;
        if (IsWithInBounds(x,y))
        {
            gamepieceData.allGamepieces[x, y] = gamePiece;
        }
        gamePiece.SetCoordinate(x, y);
    }     

    void FillBoard()
    {
        int maxIterations=100;
        int iterations=0;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (gamepieceData.allGamepieces[i,j] ==null && tileData.allTiles[i,j].tileType != TileType.Obstacle)
                {
                    Gamepiece piece = FillRandomAt(i, j);
                    iterations = 0;

                    while (gamepieceData.HasMatchOnFill(i, j))
                    {
                        gamepieceData.ClearGamepieceAt(i, j);
                        piece = FillRandomAt(i, j);
                        iterations++;

                        if (iterations >= maxIterations)
                        {
                            Debug.Log("********************");
                            break;
                        }
                    }                    
                }
            }
        }
    }

    Gamepiece FillRandomAt(int x, int y)
    {
        GameObject randomPiece = Instantiate(gamepieceData.GetRandomGamePiece(), Vector3.zero, Quaternion.identity) as GameObject;
        int offset = 10;

        if (randomPiece != null)
        {
            randomPiece.GetComponent<Gamepiece>().Init(this);
            PlaceGamePiece(randomPiece.GetComponent<Gamepiece>(), x, y);

            randomPiece.transform.position = new Vector3(x, y + offset, 0);
            randomPiece.GetComponent<Gamepiece>().Move(x, y, fallTime);

            transform.parent = transform;
            return randomPiece.GetComponent<Gamepiece>();
        }
        return null;
    }

    public void ClickTile(Tile _tile)
    {
        if (gameState==GameState.CanSwap)
        {
            if (_tile.tileType != TileType.Obstacle)
            {
                clickedTile = _tile;
            }
        }     
    }

    public void DragToTile(Tile _tile)
    {
        if (clickedTile !=null && _tile.tileType != TileType.Obstacle && gameState==GameState.CanSwap)
        {
            targetTile = _tile;

            if (Utility.IsNextTo(clickedTile, _tile))
            {
                SwitchTiles(clickedTile, targetTile);
            }
        }
    }

    public void ReleaseTile()
    {
        clickedTile = null;
        targetTile = null;
    }

    void SwitchTiles( Tile _clickedTile, Tile _targetTile)
    {
        StartCoroutine(SwitchTileRoutine(_clickedTile, _targetTile));
        gameState = GameState.Busy;
        clickedTile = null;
        targetTile = null;
    }

    IEnumerator SwitchTileRoutine(Tile _clickedTile, Tile _targetTile)
    {
        Gamepiece clickedGamepiece = gamepieceData.allGamepieces[_clickedTile.xIndex, _clickedTile.yIndex];
        Gamepiece targetGamepiece = gamepieceData.allGamepieces[_targetTile.xIndex, _targetTile.yIndex];

        if (targetGamepiece!= null && clickedGamepiece != null)
        {
            clickedGamepiece.Move(_targetTile.xIndex, _targetTile.yIndex, swapTime);
            targetGamepiece.Move(_clickedTile.xIndex, _clickedTile.yIndex, swapTime);

            // we wait until end of the switch movement
            yield return new WaitForSeconds(swapTime); 

            List<Gamepiece> matchesAtClickedGamepiece = gamepieceData.FindMatchesAt(_clickedTile.xIndex, _clickedTile.yIndex);
            List<Gamepiece> matchesAtTargetGamepiece = gamepieceData.FindMatchesAt(_targetTile.xIndex, _targetTile.yIndex);
            List<Gamepiece> allMatches = matchesAtClickedGamepiece.Union(matchesAtTargetGamepiece).ToList();

            // we check for the match count, if there are not any match we swap back again
            if (allMatches.Count==0)
            {
                clickedGamepiece.Move(_clickedTile.xIndex, _clickedTile.yIndex, swapTime);
                targetGamepiece.Move(_targetTile.xIndex, _targetTile.yIndex, swapTime);
                yield return new WaitForSeconds(swapTime);
            }

            //if there is a match we start to clear and collapse routine
            else
            {
                yield return StartCoroutine(gamepieceData.ClearAndCollapseRoutine(allMatches));
                yield return StartCoroutine(RefillBoard());
            }
        }
        gameState = GameState.CanSwap;
    }

    IEnumerator RefillBoard()
    {
        FillBoard();        
        yield return new WaitForSeconds(fallTime);
        var newMatches= gamepieceData.FindAllMatches();

        if (newMatches !=null)
        {
            gameState = GameState.Busy;
            yield return StartCoroutine(gamepieceData.ClearAndCollapseRoutine(newMatches));
            yield return StartCoroutine(RefillBoard());
            yield return new WaitForSeconds(fallTime);
        }
        gameState = GameState.CanSwap;

    }

    bool IsWithInBounds(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0 && y < height);
    }

}








