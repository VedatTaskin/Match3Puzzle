using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

[CreateAssetMenu(fileName ="Gamepieces Data", menuName ="Create/Gamepiece Data Holder")]
public class GamepieceData : ScriptableObject
{
    public Gamepiece[,] allGamepieces;
    public GameObject[] gamePiecePrefabs;
    public TileData tileData;

    int width;
    int height;
    float collapseTime=0.1f;
    private void OnEnable()
    {
        width = tileData.width;
        height = tileData.height;
        allGamepieces = new Gamepiece[width,height];
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

        if (startPiece != null)
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

            if (nextPiece == null )
            {
                break;
            }
            else
            {
                if (startPiece.normalGamepieceType == nextPiece.normalGamepieceType)
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
            if (tileData.allTiles[x,y].tileType==TileType.Breakable)
            {
                tileData.allTiles[x, y].GetComponent<BreakableTile>().SetBreakableValue();
            }
        }
    }

    public void ClearGamepieces(List<Gamepiece> gamepieces)
    {
        foreach (var piece in gamepieces)
        {
            if (piece != null)
            {
                ClearGamepieceAt(piece.xIndex, piece.yIndex);
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
            if (!columns.Contains(piece.xIndex))
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

            yield return new WaitForSeconds(0.2f);
            var movingPieces = CollapseColumn(matches);

            while (!Utility.GamepiecesAreCollapsed(movingPieces))
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.2f);
            var newMatches = new List<Gamepiece>();

            foreach (var piece in movingPieces)
            {
                newMatches =newMatches.Union(FindMatchesAt(piece.xIndex,piece.yIndex)).ToList();
            }

            if (newMatches.Count==0)
            {
                routineIsFinished = true;
            }
            else
            {
                routineIsFinished = false;
                matches = newMatches;
            }

            // check point to prevent infinite loop
            iterations++;
            if (iterations>maxIterations)
            {
                break;
            }

        } while (!routineIsFinished);

    }
}
