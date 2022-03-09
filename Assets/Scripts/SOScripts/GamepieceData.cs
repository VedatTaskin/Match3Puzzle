using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

[CreateAssetMenu(fileName ="Gamepieces Data", menuName ="Create/Gamepiece Data Holder")]
public class GamepieceData : ScriptableObject
{
    [HideInInspector] public Gamepiece[,] allGamepieces;
    [HideInInspector] public GameObject bomb;

    [Space(10)]
    [Tooltip("Normal gamepieces will be in the game")]
    public GameObject[] gamePiecePrefabs;

    [Space(10)]
    [Tooltip("Bombs in the Game")]
    [Header("Bombs")]
    public GameObject rowBomb;
    public GameObject columnBomb;
    public GameObject adjacentBomb;
    public GameObject colorBomb;

    [Space(10)]
    [Tooltip("Tile will be in the game - Scriptable Object")]
    public TileData tileData;

    [Space(10)]
    [Header("Collectibles")]
    public int collectiblesCount;
    public int maxCollectibles;
    public GameObject collectiblePrefab;
    [Range(0,1)]
    public float chanceForCollectible;

    [Space(10)]
    [Tooltip("Some gamepieces can be define before starting")]
    public OrderedGamepieces[] orderedGamepieces;

    int width;
    int height;
    float collapseTime=0.1f;
    [HideInInspector] public Board board;

    private void OnEnable()
    {
        width = tileData.width;
        height = tileData.height;
        allGamepieces = new Gamepiece[width,height];
        collectiblesCount = 0;
    }

    public void Init( Board _board)
    {
        board = _board;
    }

    public GameObject GetRandomGamePiece()
    {
        int randomIndex = Random.Range(0, gamePiecePrefabs.Length);
        if (gamePiecePrefabs[randomIndex] == null)
        {
            Debug.LogWarning("BOARD" + randomIndex + " doesn't contain a vaild Gamepiece prefab");
        }
        return gamePiecePrefabs[randomIndex];
    }

    //This function find matches from specific point to specific direction
    List<Gamepiece> FindMatches(int startX, int startY, Vector2 searchDirection, int minLength = 3)
    {
        List<Gamepiece> matches = new List<Gamepiece>();
        Gamepiece startPiece = allGamepieces[startX, startY];

        if (startPiece != null )
        {
            matches.Add(startPiece);
        }
        else
        {
            return null;
        }

        int nextX;
        int nextY;

        int maxValue = (width > height) ? width : height;

        for (int i = 0; i < maxValue; i++)
        {
            nextX = startX + (int)searchDirection.x * i;
            nextY = startY + (int)searchDirection.y * i;

            if (!IsWithInBounds(nextX,nextY))
            {
                break;
            }

            Gamepiece nextPiece = allGamepieces[nextX, nextY];


            if (nextPiece == null)
            {
                break;
            }
            else
            {
                if (startPiece.normalGamepieceType == nextPiece.normalGamepieceType &&
                        startPiece.gamepieceType == GamepieceType.Normal && nextPiece.gamepieceType == GamepieceType.Normal)
                {
                    matches.Add(nextPiece);
                }
                else
                {
                    break;
                }
            }
        }

        if (matches.Count >= minLength)
        {
            return matches;
        }
        return null;
    }

    List<Gamepiece> FindVerticalMatches(int startX, int startY, int minLength = 3)
    {
        List<Gamepiece> upwardMatches = FindMatches(startX, startY, new Vector2(0, 1), 2);
        List<Gamepiece> downwardMatches= FindMatches(startX, startY, new Vector2(0,-1), 2);

        if (upwardMatches == null)
        {
            upwardMatches = new List<Gamepiece>();
        }
        if (downwardMatches == null)
        {
            downwardMatches = new List<Gamepiece>();
        }

        var combinedMatches = upwardMatches.Union(downwardMatches).ToList();

        return (combinedMatches.Count >= minLength) ? combinedMatches : null;    
    }

    List<Gamepiece> FindHorizontalMatches(int startX, int startY, int minLength = 3)
    {
        List<Gamepiece> rightMatches = FindMatches(startX, startY, new Vector2(1, 0), 2);
        List<Gamepiece> leftMatches = FindMatches(startX, startY, new Vector2(-1, 0), 2);

        if (rightMatches == null)
        {
            rightMatches = new List<Gamepiece>();
        }
        if (leftMatches == null)
        {
            leftMatches = new List<Gamepiece>();
        }

        var combinedMatches = leftMatches.Union(rightMatches).ToList();

        return (combinedMatches.Count >= minLength) ? combinedMatches : null;
    }

    public List<Gamepiece> FindMatchesAt(int x, int y, int minLength = 3)
    {
        List<Gamepiece> horizMatches = FindHorizontalMatches(x, y, minLength);
        List<Gamepiece> vertMatches = FindVerticalMatches(x, y, minLength);


        if (horizMatches == null)
        {
            horizMatches = new List<Gamepiece>();
        }

        if (vertMatches == null)
        {
            vertMatches = new List<Gamepiece>();
        }

        var combinedMatches = horizMatches.Union(vertMatches).ToList();
        return combinedMatches;
    }

    public List<Gamepiece> FindAllMatches()
    {
        List<Gamepiece> matches = new List<Gamepiece>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                matches=matches.Union(FindMatchesAt(i, j)).ToList();
            }
        }
        return (matches.Count == 0) ? null : matches;
    } 

    bool IsWithInBounds(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0 && y < height);
    }

    public void ClearGamepieceAt(int x, int y)
    {
        Gamepiece gamepieceToClear = allGamepieces[x, y];

        if (gamepieceToClear != null)
        {
            allGamepieces[x, y] = null;
            Destroy(gamepieceToClear.gameObject);
        }
    }

    private void BreakTilesAt(int x, int y)
    {
        if (tileData.allTiles[x, y].tileType == TileType.Breakable)
        {
            tileData.allTiles[x, y].GetComponent<BreakableTile>().SetBreakableValue();
        }
    }

    public void ClearGamepieces(List<Gamepiece> gamepieces)
    {
        foreach (var piece in gamepieces)
        {
            if (piece != null)
            {
                ClearGamepieceAt(piece.xIndex, piece.yIndex);
                BreakTilesAt(piece.xIndex, piece.yIndex);
            }
        }
    }

    public bool HasMatchOnFill(int x, int y, int minLength = 3)
    {
        List<Gamepiece> leftMatches = FindMatches(x, y, new Vector2(-1, 0), minLength);
        List<Gamepiece> downMatches = FindMatches(x, y, new Vector2(0, -1), minLength);

        if (leftMatches == null)
        {
            leftMatches = new List<Gamepiece>();
        }

        if (downMatches == null)
        {
            downMatches = new List<Gamepiece>();
        }
        return (downMatches.Count > 0 || leftMatches.Count > 0);
    }

    public List<int> FindCollapsingColumnNumbers(List<Gamepiece> gamepieces)
    {
        List<int> columns = new List<int>();

        foreach (var piece in gamepieces)
        {
            if (!columns.Contains(piece.xIndex) )
            {
                columns.Add(piece.xIndex);
            }
        }
        return columns;
    }

    public List<Gamepiece> CollapseColumn(List<Gamepiece> gamepieces)
    {
        //which pillars will be collapse
        List<int> columns = FindCollapsingColumnNumbers(gamepieces);

        // we want to know which pieces are moving, we will check if they make another match after collapsing
        List<Gamepiece> movingPieces = new List<Gamepiece>();

        for (int c = 0; c < columns.Count; c++)
        {
            // each pillar will check seperately
            for (int i = 0; i < height - 1; i++)
            {
                if (allGamepieces[columns[c], i] == null && tileData.allTiles[columns[c], i].tileType !=TileType.Obstacle )
                {
                    for (int j = i + 1; j < height; j++)
                    {
                        if (allGamepieces[columns[c],j] !=null)
                        {
                            var piece = allGamepieces[columns[c], j];
                            allGamepieces[columns[c], i] = piece;

                            if (!movingPieces.Contains(piece))
                            {
                                movingPieces.Add(piece);
                            }                            

                            piece.Move(columns[c], i, collapseTime * (j-i));
                            piece.SetCoordinate(columns[c], i);
                            
                            allGamepieces[columns[c], j] = null;
                            break;
                        }
                    }
                }
            }
        }
        return movingPieces;
    }

    public IEnumerator ClearAndCollapseRoutine(List<Gamepiece> _allMatches)
    {
        var matches = _allMatches;
        bool routineIsFinished = true;
        int maxIterations = 100;
        int iterations = 0;

        do
        {
            yield return new WaitForSeconds(0.2f);
            ClearGamepieces(matches);

            //instantiate bomb here
            CheckBombCreation();

            yield return new WaitForSeconds(0.2f);
            var movingPieces = CollapseColumn(matches);

            while (!Utility.GamepiecesAreCollapsed(movingPieces))
            {
                yield return null;
            }

            //check if there are another matches after collapsing column
            yield return new WaitForSeconds(0.2f);
            var newMatches = new List<Gamepiece>();

            foreach (var piece in movingPieces)
            {
                newMatches = newMatches.Union(FindMatchesAt(piece.xIndex, piece.yIndex)).ToList();
            }

            //check if Collectibles are reached to the bottom of the board
            var collectiblesFound = FindCollectiblesAtRow(0);
            if (collectiblesFound.Count != 0)
            {
                newMatches = newMatches.Union(collectiblesFound).ToList();
                collectiblesCount -= collectiblesFound.Count;
            }

            // if there are not any pieces to clear we end up the loop
            if (newMatches.Count == 0)
            {
                routineIsFinished = true;
            }

            // if there is a gamepiece to clear we restart the loop
            else
            {
                routineIsFinished = false;
                matches = newMatches;
            }

            // check point to prevent infinite loop
            iterations++;
            if (iterations > maxIterations)
            {
                break;
            }

        } while (!routineIsFinished);

    }

    //To find collectible in a specific row
    private List<Gamepiece> FindCollectiblesAtRow(int row)
    {
        List<Gamepiece> collectibles = new List<Gamepiece>();

        for (int i = 0; i < board.width; i++)
        {
            var piece = board.gamepieceData.allGamepieces[i, row];
            if (piece != null)
            {                
                if (piece.gamepieceType == GamepieceType.Collectible && !collectibles.Contains(piece))
                {
                    collectibles.Add(piece);
                    //Debug.Log(piece.xIndex + " " + piece.yIndex);
                }
            }
        }
        return collectibles;
    }

    private void CheckBombCreation()
    {
        if (bomb != null)
        {
            int x = bomb.GetComponent<Bombs>().xIndex;
            int y = bomb.GetComponent<Bombs>().yIndex;
            allGamepieces[x, y] = bomb.GetComponent<Gamepiece>();
            bomb = null;
        }
    }

    public bool IsCornerMatch(List<Gamepiece> gamepieces)
    {
        bool vertical = false;
        bool horizontal = false;

        int xStart = -1;
        int yStart = -1;

        foreach (var piece in gamepieces)
        {
            if (xStart == -1 || yStart ==-1)
            {
                xStart = piece.xIndex;
                yStart = piece.yIndex;
                continue;
            }

            if (piece.xIndex != xStart && piece.yIndex == yStart)
            {
                horizontal = true;
            }

            if (piece.xIndex == xStart && piece.yIndex != yStart)
            {
                vertical = true;
            }
        }

        return (horizontal && vertical);
    }

    public bool CanAddCollectible()
    {
        return (Random.Range(0f, 1f) <= chanceForCollectible && collectiblesCount < maxCollectibles);
    }
}

[System.Serializable]
public class OrderedGamepieces
{
    public GameObject prefab;
    public int xCoord;
    public int yCoord;
}