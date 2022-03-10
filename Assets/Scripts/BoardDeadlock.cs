using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoardDeadlock : MonoBehaviour
{
    // Gives a column or a row list with three elements at a specific point
    // We check if we can make a match with 3 element 
    List<Gamepiece> GetRowOrColumnList(Gamepiece[,] allPieces, int x, int y, int listLength = 3, bool checkRow = true)
    {
        int width = allPieces.GetLength(0);
        int height = allPieces.GetLength(1);

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
    List<Gamepiece> GetMinimumMatches(List<Gamepiece> gamePieces, int minForMatches = 2)
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
    List<Gamepiece> GetNeighbors(Gamepiece[,] allPieces, int x, int y)
    {
        int width = allPieces.GetLength(0);
        int height = allPieces.GetLength(1);

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

    bool HasMoveAt(Gamepiece [,] allPieces, int x, int y, int listLength = 3, bool checkRow = true)
    {
        List<Gamepiece> pieces = GetRowOrColumnList(allPieces, x, y, listLength, checkRow);

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

    public bool IsDeadLock(Gamepiece [,] allPieces, int listLength=3)
    {
        int width = allPieces.GetLength(0);
        int height = allPieces.GetLength(1);

        bool isDeadLock = true;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (HasMoveAt(allPieces,i,j,listLength,true) || HasMoveAt(allPieces,i,j,listLength,false))
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
