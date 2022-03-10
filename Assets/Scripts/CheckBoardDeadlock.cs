using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class CheckBoardDeadlock
{
    public static Gamepiece[,] allPieces;
    public static int width;
    public static int height;
    public static int listLength;
            

    // Gives a column or a row list with three elements at a specific point
    // We check if we can make a match with 3 element 
    static List<Gamepiece> GetRowOrColumnList(int x, int y, bool checkRow = true)
    {
        List<Gamepiece> piecesList = new List<Gamepiece>();

        for (int i = 0; i < listLength; i++)
        {
            if (checkRow)
            {
                if (x + i < width && y < height && allPieces[x+i,y] != null)
                {
                    piecesList.Add(allPieces[x + i, y]);

                }
            }
            else
            {
                if (x < width && y + i < height && allPieces[x,y+i] != null)
                {
                    piecesList.Add(allPieces[x, y + i]);
                }
            }
        }

        return piecesList;
    }

    //We check if there are two element with same normalGamepieceType in the list
    static List<Gamepiece> GetMinimumMatches(List<Gamepiece> gamePieces, int minForMatches = 2)
    {
        List<Gamepiece> matches = new List<Gamepiece>();

        var groups = gamePieces.GroupBy(n => n.normalGamepieceType);
        foreach (var grp in groups)
        {
            if (grp.Count() >= minForMatches && grp.Key != NormalGamepieceType.None)
            {
                matches = grp.ToList();
            }
        }
        return matches;
    }

    // we take neighbors of a specific Gamepiece
    static List<Gamepiece> GetNeighbors(Gamepiece[,] allPieces, int x, int y)
    {

        List<Gamepiece> neighbors = new List<Gamepiece>();

        Vector2[] searchDirections = new Vector2[4]
        {
            new Vector2(-1f,0f),
            new Vector2 (1f,0f),
            new Vector2(0f,1f),
            new Vector2(0f,-1f)
        };

        foreach (var dir in searchDirections)
        {
            if (x+(int)dir.x >=0 && x + (int)dir.x <width &&
                    y + (int)dir.y >= 0 && y + (int)dir.y < height)
            {
                if (allPieces[x + (int)dir.x, y + (int)dir.y] != null)
                {
                    neighbors.Add(allPieces[x + (int)dir.x, y + (int)dir.y]);
                }
            }
        }

        return neighbors;
    }

    static bool HasMoveAt(int x, int y, bool checkRow = true)
    {
        List<Gamepiece> pieces = GetRowOrColumnList( x, y, checkRow);

        List<Gamepiece> matches = GetMinimumMatches(pieces, listLength - 1);

        Gamepiece unmatchedPiece = null;

        if (pieces != null && matches != null)
        {
            if (pieces.Count==listLength && matches.Count==listLength-1)
            {
                unmatchedPiece = pieces.Except(matches).FirstOrDefault();
            }

            if (unmatchedPiece!=null)
            {
                List<Gamepiece> neighbors = GetNeighbors(allPieces, unmatchedPiece.xIndex,unmatchedPiece.yIndex);
                neighbors = neighbors.Except(matches).ToList();
                neighbors = neighbors.FindAll(n => n.normalGamepieceType == matches[0].normalGamepieceType);

                matches = matches.Union(neighbors).ToList();
            }

            if (matches.Count >=listLength)
            {
                string rowColStr = (checkRow) ? "row" : "column";
                Debug.Log("*************AvailableMove**********");
                Debug.Log("Move" + matches[0].normalGamepieceType + "piece to" + unmatchedPiece.xIndex + ", " +
                    unmatchedPiece.yIndex + "to form matching" + rowColStr);
                return true;
            }
        }                
        return false;
    }

    static public bool IsDeadLock(Gamepiece [,] _allPieces, int _listLength=3)
    {
        allPieces = _allPieces;
        width = allPieces.GetLength(0);
        height = allPieces.GetLength(1);
        listLength = _listLength;

        bool isDeadLock = true;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (HasMoveAt(i,j,true) || HasMoveAt(i,j,false))
                {
                    isDeadLock = false;
                    // we may want to return immediately when we find a match
                    // or we can continue to check all board
                    // return false
                }
            }
        }
        if (isDeadLock)
        {
            Debug.Log("Board is deadlock");
        }
        return isDeadLock;
    }

}
