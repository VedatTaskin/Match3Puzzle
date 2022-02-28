using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[CreateAssetMenu(fileName ="Gamepieces Data", menuName ="Create/Gamepiece Data Holder")]
public class GamepieceData : ScriptableObject
{
    public Gamepiece[,] allGamepieces;
    public GameObject[] gamePiecePrefabs;
    public TileData tileData;

    int width;
    int height;
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

    public void ClearGamepieces(List<Gamepiece> gamepieces)
    {
        foreach (var piece in gamepieces)
        {
            ClearGamepieceAt(piece.xIndex, piece.yIndex);
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
}
