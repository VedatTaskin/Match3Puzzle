using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Board : MonoBehaviour
{
    [HideInInspector] public int width;
    [HideInInspector] public int height;

    public int borderSize;
    [Range(0, 1)]
    public float swapTime = 0.5f;
    public float fallTime = 0.5f;

    public GamepieceData gamepieceData;
    public TileData tileData;

    GameState gameState;
    Tile clickedTile;
    Tile targetTile;
    int offset = 10;
    int firstFill = 0;

    private void Start()
    {
        width = tileData.width;
        height = tileData.height;
        tileData.SetupTiles(this);
        gamepieceData.Init(this);
        SetupCamera();
        FillBoard();
        gameState = GameState.CanSwap;

    }

    void SetupCamera()
    {
        Camera.main.transform.position = new Vector3((float)(width - 1) / 2, (float)(height - 1) / 2, -10f);

        float aspectRatio = (float)Screen.width / (float)Screen.height;

        float verticalSize = (float)height / 2f + (float)borderSize;

        float horizontalSize = ((float)width / 2f + (float)borderSize) / aspectRatio;

        Camera.main.orthographicSize = (verticalSize > horizontalSize) ? verticalSize : horizontalSize;

    }

    public void PlaceGamePiece(Gamepiece gamePiece, int x, int y)
    {
        if (gamePiece == null)
        {
            Debug.LogWarning("BOARD: invalid gamepiece");
            return;
        }
        gamePiece.transform.position = new Vector3(x, y, 0);
        gamePiece.transform.rotation = Quaternion.identity;
        if (IsWithInBounds(x, y))
        {
            gamepieceData.allGamepieces[x, y] = gamePiece;
        }
        gamePiece.SetCoordinate(x, y);
    }

    void FillBoard()
    {
        int maxIterations = 100;
        int iterations = 0;

        // firstly we instantiate ordered gamepieces
        FirstFillCheck();

        // than we instantiate randomly gamepieces
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {

                if (gamepieceData.allGamepieces[i, j] == null && tileData.allTiles[i, j].tileType != TileType.Obstacle)
                {
                    Gamepiece piece = null;

                    if (j==height-1 && gamepieceData.CanAddCollectible())
                    {
                        piece = FillRandomCollectibleAt(i, j);
                        gamepieceData.collectiblesCount++;
                    }
                    else
                    {
                        piece = FillRandomGamepieceAt(i, j);
                        iterations = 0;

                        while (gamepieceData.HasMatchOnFill(i, j))
                        {
                            gamepieceData.ClearGamepieceAt(i, j);
                            piece = FillRandomGamepieceAt(i, j);
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

        //check for deadlock
        CheckBoardDeadlock.IsDeadLock(gamepieceData.allGamepieces, 3);
    }

    private void FirstFillCheck()
    {
        if (gamepieceData.orderedGamepieces.Length != 0 && firstFill == 0)
        {
            foreach (var item in gamepieceData.orderedGamepieces)
            {
                GameObject itemGO = Instantiate(item.prefab, Vector3.zero, Quaternion.identity) as GameObject;
                CraeateGamepiece(itemGO, item.xCoord, item.yCoord);
            }
            firstFill++;
        }
    }

    private void CraeateGamepiece(GameObject gamepieceGO, int x, int y)
    {
        gamepieceGO.GetComponent<Gamepiece>().Init(this);
        PlaceGamePiece(gamepieceGO.GetComponent<Gamepiece>(), x, y);
        gamepieceGO.transform.position = new Vector3(x, y + offset, 0);
        gamepieceGO.GetComponent<Gamepiece>().Move(x, y, fallTime);
        transform.parent = transform;
    }

    private GameObject CreateBomb(GameObject prefab, int x, int y)
    {
        if (prefab != null && IsWithInBounds(x, y))
        {
            GameObject bomb = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
            bomb.GetComponent<Bombs>().Init(this);
            bomb.GetComponent<Bombs>().SetCoordinate(x, y);
            bomb.transform.parent = transform;
            return bomb;
        }
        return null;
    }

    Gamepiece FillRandomGamepieceAt(int x, int y)
    {
        GameObject randomPiece = Instantiate(gamepieceData.GetRandomGamePiece(), Vector3.zero, Quaternion.identity) as GameObject;

        if (randomPiece != null)
        {
            CraeateGamepiece(randomPiece, x, y);
            return randomPiece.GetComponent<Gamepiece>();
        }
        return null;
    }

    Gamepiece FillRandomCollectibleAt(int x, int y)
    {
        GameObject randomCollectible = Instantiate(gamepieceData.collectiblePrefab, Vector3.zero, Quaternion.identity) as GameObject;
        if (randomCollectible != null)
        {
            CraeateGamepiece(randomCollectible, x, y);
            return randomCollectible.GetComponent<Gamepiece>();
        }
        return null;    
    }

    public void ClickTile(Tile _tile)
    {
        if (gameState == GameState.CanSwap)
        {
            if (_tile.tileType != TileType.Obstacle)
            {
                clickedTile = _tile;
            }
        }
    }

    public void DragToTile(Tile _tile)
    {
        if (clickedTile != null && _tile.tileType != TileType.Obstacle && gameState == GameState.CanSwap)
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

    void SwitchTiles(Tile _clickedTile, Tile _targetTile)
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

        if (targetGamepiece != null && clickedGamepiece != null
            && clickedGamepiece.gamepieceType != GamepieceType.NotMoveable && targetGamepiece.gamepieceType != GamepieceType.NotMoveable)
        {

            clickedGamepiece.Move(_targetTile.xIndex, _targetTile.yIndex, swapTime);
            targetGamepiece.Move(_clickedTile.xIndex, _clickedTile.yIndex, swapTime);

            // we wait until end of the switch movement
            yield return new WaitForSeconds(swapTime);

            StartCoroutine(ApplyGamepieceRule(clickedGamepiece, targetGamepiece));
        }

    }

    IEnumerator RefillBoard()
    {
        FillBoard();
        yield return new WaitForSeconds(fallTime);
        var newMatches = gamepieceData.FindAllMatches();

        if (newMatches != null)
        {
            gameState = GameState.Busy;
            yield return StartCoroutine(gamepieceData.ClearAndCollapseRoutine(newMatches));
            yield return StartCoroutine(RefillBoard());
            yield return new WaitForSeconds(fallTime);
        }
        gameState = GameState.CanSwap;
    }

    public bool IsWithInBounds(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0 && y < height);
    }

    IEnumerator ApplyGamepieceRule(Gamepiece clicked, Gamepiece target)
    {
        List<Gamepiece> gamepiecesWillClear = RuleChoser.ChooseRule(clicked, target, this);

        if (gamepiecesWillClear == null || gamepiecesWillClear.Count == 0)
        {
            clicked.Move(target.xIndex, target.yIndex, swapTime);
            target.Move(clicked.xIndex, clicked.yIndex, swapTime);
            yield return new WaitForSeconds(swapTime);
        }

        else
        {
            yield return StartCoroutine(gamepieceData.ClearAndCollapseRoutine(gamepiecesWillClear));
            yield return StartCoroutine(RefillBoard());
        }

        gameState = GameState.CanSwap;
        yield return null;
    }

    public void DropBomb(int x, int y, Vector2 swapDirection, List<Gamepiece> gamepieces)
    {
        GameObject bombGO = null;
        // Decide bomb type
        if (gamepieces.Count >= 4)
        {

            if (!gamepieceData.IsCornerMatch(gamepieces))
            {
                //color bomb
                if (gamepieces.Count >= 5)
                {
                    bombGO = CreateBomb(gamepieceData.colorBomb, x, y);
                }
                else
                {
                    //row bomb
                    if (swapDirection.x != 0)
                    {
                        bombGO = CreateBomb(gamepieceData.rowBomb, x, y);
                    }
                    //column bomb
                    else
                    {
                        bombGO = CreateBomb(gamepieceData.columnBomb, x, y);
                    }

                }

            }

            else
            {
                //color bomb
                if (gamepieces.Count >= 7)
                {
                    bombGO = CreateBomb(gamepieceData.colorBomb, x, y);
                }

                //adjacent bomb
                else
                {                    
                    bombGO = CreateBomb(gamepieceData.adjacentBomb, x, y);
                }

            }
        }
        gamepieceData.bomb = bombGO;          
    }

}








