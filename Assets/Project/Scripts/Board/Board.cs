using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Board : MonoBehaviour
{
    [HideInInspector] public int width;
    [HideInInspector] public int height;

    public int borderSize;

    [Range(0, 1)]
    public float swapTime = 0.5f;
    public float fallTime = 0.2f;

    [Header("Gamepieces & Tiles SO")]
    public GamepieceData gamepieceData;
    public TileData tileData;

    WaitForSeconds waitForFallTime;
    Tile clickedTile;
    Tile targetTile;
    int offset = 10;
    int firstFill = 0;

    private void Awake()
    {
        waitForFallTime = new WaitForSeconds(fallTime);
        width = tileData.width;
        height = tileData.height;
    }

    private void Start()
    {
        tileData.SetupTiles(this);
        gamepieceData.Init(this);
        SetupCamera();
        FillBoard();

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
        StartCoroutine(CheckForDeadlock());        
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
        StartCoroutine(gamepieceGO.GetComponent<Gamepiece>().Move(x, y, fallTime,MoveType.Fall));
        transform.parent = transform;
    }

    private GameObject CreateBomb(GameObject prefab, int x, int y)
    {
        if (prefab != null && IsWithInBounds(x, y))
        {
            GameObject bomb = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
            bomb.GetComponent<Bombs>().Init(this);
            bomb.GetComponent<Bombs>().SetCoordinate(x, y);
            bomb.transform.position = new Vector3(x, y, 0);
            bomb.transform.rotation = Quaternion.identity;
            gamepieceData.allGamepieces[x, y] = bomb.GetComponent<Gamepiece>();
            return bomb;
        }
        return null;
    }

    public Gamepiece FillRandomGamepieceAt(int x, int y)
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
        if (gamepieceData.allGamepieces[_tile.xIndex,_tile.yIndex].pieceState == PieceState.CanMove)
        {
            if (_tile.tileType != TileType.Obstacle)
            {
                clickedTile = _tile;
            }
        }
    }

    public void DragToTile(Tile _tile)
    {
        if (clickedTile != null && _tile.tileType != TileType.Obstacle 
            && gamepieceData.allGamepieces[_tile.xIndex, _tile.yIndex].pieceState == PieceState.CanMove)
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
        // WE can activate a bomb only one tap as in Royal Match
        if (clickedTile != null && targetTile == null)
        {
            StartCoroutine(CheckForBombActivation(clickedTile));
        }

        clickedTile = null;
        targetTile = null;
    }

    IEnumerator CheckForBombActivation(Tile clickedTile)
    {
        var piece = gamepieceData.allGamepieces[clickedTile.xIndex, clickedTile.yIndex];
        if (piece != null && piece.gamepieceType == GamepieceType.Bomb)
        {
            List<Gamepiece> gamepiecesWillClear = new List<Gamepiece>();
            gamepiecesWillClear = piece.GetComponent<ISelfDestroy>().SelfDestroy(this);
            if (gamepiecesWillClear != null)
            {
                //yield return StartCoroutine(gamepieceData.ClearAndCollapseRoutine(gamepiecesWillClear));
                //yield return StartCoroutine(RefillBoard());
            }
        }
        yield return null;        
    }

    void SwitchTiles(Tile _clickedTile, Tile _targetTile)
    {
        StartCoroutine(SwitchTileRoutine(_clickedTile, _targetTile));
        clickedTile = null;
        targetTile = null;
    }

    IEnumerator SwitchTileRoutine(Tile _clickedTile, Tile _targetTile)
    {
        // clicked and target tiles are stored temporarly
        Gamepiece clickedGamepiece = gamepieceData.allGamepieces[_clickedTile.xIndex, _clickedTile.yIndex];
        Gamepiece targetGamepiece = gamepieceData.allGamepieces[_targetTile.xIndex, _targetTile.yIndex];

        if (targetGamepiece != null && clickedGamepiece != null
            && clickedGamepiece.gamepieceType != GamepieceType.NotMoveable && targetGamepiece.gamepieceType != GamepieceType.NotMoveable)
        {
            // if clicked and target Gamepieces are exist and moveable we swap this two objects
            StartCoroutine(clickedGamepiece.Move(_targetTile.xIndex, _targetTile.yIndex, swapTime, MoveType.Swap));
            yield return StartCoroutine(targetGamepiece.Move(_clickedTile.xIndex, _clickedTile.yIndex, swapTime, MoveType.Swap));
             

            //clicked and target Gamepieces now checking
            StartCoroutine(ApplyGamepieceRule(clickedGamepiece, targetGamepiece));
        }

    }

    IEnumerator FindNewMatches()
    {
        var newMatches = gamepieceData.FindAllMatches();

        if (newMatches != null)
        {
            //yield return StartCoroutine(gamepieceData.ClearAndCollapseRoutine(newMatches));
            //yield return StartCoroutine(RefillBoard());
            yield return new WaitForSeconds(fallTime);
        }
    }

    public bool IsWithInBounds(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0 && y < height);
    }

    IEnumerator ApplyGamepieceRule(Gamepiece clicked, Gamepiece target)
    {

        // Rule choser returns a bool if swap is valise 
        // If there isn't any gamepieces to clear we swap back again
        if (!RuleChoser.ChooseRule(clicked, target, this))
        {
            StartCoroutine(clicked.Move(target.xIndex, target.yIndex, swapTime, MoveType.Swap));
            yield return StartCoroutine(target.Move(clicked.xIndex, clicked.yIndex, swapTime, MoveType.Swap));
        }

        yield return null;
    }

    public GameObject DropBomb(int x, int y, Vector2 swapDirection, List<Gamepiece> gamepieces)
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
        return bombGO;
    }

    IEnumerator CheckForDeadlock()
    {
        yield return waitForFallTime;
        if (BoardDeadlockControl.IsDeadLock(gamepieceData.allGamepieces, 3))
        {
            StartCoroutine(ShuffleBoard());
        }
    }

    IEnumerator ShuffleBoard()
    {
        var normalPieceList = BoardShuffleControl.RemoveNormalPieces(gamepieceData.allGamepieces);
        var shuffledList = BoardShuffleControl.ShuffleList(normalPieceList);
        StartCoroutine(PlaceShuffledList(shuffledList));
        yield return null;
    }

    IEnumerator PlaceShuffledList(List<Gamepiece> gamepieces)
    {
        Queue<Gamepiece> normalPieces = new Queue<Gamepiece>(gamepieces);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var piece = gamepieceData.allGamepieces[i, j];
                if (piece == null && tileData.allTiles[i, j].tileType != TileType.Obstacle)
                {
                    piece = normalPieces.Dequeue();
                    piece.SetCoordinate(i, j);
                    StartCoroutine(piece.Move(i, j, fallTime, MoveType.Swap));
                }
            }
        }

        yield return waitForFallTime;

        yield return FindNewMatches();
        yield return StartCoroutine(CheckForDeadlock());

    }

    public IEnumerator CollapseRoutine(List<Gamepiece> matches)
    {
        List<Gamepiece> PiecesAtTheBottomOfMatches = new List<Gamepiece>();

        //önce ilgili kolonlar gruplandırılıyor kendi içerisinde
        var groupByColumn = matches.GroupBy(n => n.xIndex);

        //bir silinme esnasında en alttaki elemanlar hangisi ise bulunuyor
        foreach (var grp in groupByColumn)
        {
            var bottomPiece = grp.OrderByDescending(i => (i.yIndex)).Last();
            if (!PiecesAtTheBottomOfMatches.Contains(bottomPiece))
            {
                PiecesAtTheBottomOfMatches.Add(bottomPiece);
            }
        }

        //now we collapse each column that we have cleared an object
        foreach (var piece in PiecesAtTheBottomOfMatches)
        {
            _ = CollapseGamepieces(piece);
        }
        yield return null;
    }

    //we return which gamepieces are collapsing
    List<Gamepiece> CollapseGamepieces(Gamepiece piece)
    {
        var allGamepieces = gamepieceData.allGamepieces;
        int column = piece.xIndex;

        //we want to know which pieces are moving, we will check if they make another match after collapsing
        List<Gamepiece> movingPieces = new List<Gamepiece>();

        for (int i = piece.yIndex; i < height - 1; i++)
        {
            if (allGamepieces[column, i] == null
                && tileData.allTiles[column, i].tileType != TileType.Obstacle)
            {
                //Debug.Log(column + ", " + i +" null");

                for (int j = i + 1; j < height; j++)
                {
                    if (allGamepieces[column, j] != null)
                    {
                        allGamepieces[column, i] = allGamepieces[column, j];
                        allGamepieces[column, j] = null;
                        allGamepieces[column, i].SetCoordinate(column, i);
                        StartCoroutine(allGamepieces[column, i].Move(column, i, fallTime * (j - i), MoveType.Fall));

                        if (!movingPieces.Contains(allGamepieces[column, i]))
                        {
                            movingPieces.Add(allGamepieces[column, i]);
                        }
                        break;
                    }
                }
            }
        }

        StartCoroutine(FillColumn( column));
        return movingPieces;        
    }

    IEnumerator FillColumn(int column)
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < height; i++)
        {
            if (gamepieceData.allGamepieces[column, i] == null
                && tileData.allTiles[column, i].tileType != TileType.Obstacle)
            {
                //Debug.Log(column + ", " + i + " null");
                FillRandomGamepieceAt(column, i);
            }
        }
    }

    public IEnumerator CheckMatchesAfterFallDown(Gamepiece piece)
    {
        yield return new WaitForSeconds(0.3f);
        List<Gamepiece> newMatches = new List<Gamepiece>();
        newMatches = gamepieceData.FindMatchesAt(piece.xIndex,piece.yIndex);

        gamepieceData.ClearGamepieces(newMatches);

        //*******************
        //We will instantiate bomb immediately 
        _ = NewMatchesCanMakeBomb(newMatches);
        //*****************

        StartCoroutine(CollapseRoutine(newMatches));
    }

    public void BombCreation(Gamepiece clicked,  Gamepiece target, List<Gamepiece> matches)
    {
        Vector2 swapDirection = new Vector2(target.xIndex - clicked.xIndex, target.yIndex - clicked.yIndex);
        _ = DropBomb(clicked.xIndex, clicked.yIndex, swapDirection, matches);
    }

    // we didn't detailed this method, WE drop a bomb if new matches greater than 4,
    // we should check their color, and match type 
    bool NewMatchesCanMakeBomb(List<Gamepiece> newMatches)
    {
        if (newMatches.Count >= 4)
        {
            DropBomb(newMatches[1].xIndex, newMatches[1].yIndex, new Vector2(0, 1), newMatches);
            return true;
        }
        return false;
    }
}








