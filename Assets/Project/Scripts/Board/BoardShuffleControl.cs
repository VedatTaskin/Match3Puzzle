using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoardShuffleControl 
{
    public static Gamepiece[,] allPieces;
    public static int width;
    public static int height;
    public static float swapTime;

    /// <summary>
    /// Returns a list of gamepiece that contains only normaltype gamepiece
    /// and shuffled
    /// </summary>
    /// <param name="_allPieces"></param>
    /// <param name="_swapTime"></param>
    /// <returns></returns>
    static public List<Gamepiece> RemoveNormalPieces(Gamepiece[,] _allPieces, float _swapTime = 0.5f)
    {
        allPieces = _allPieces;
        swapTime = _swapTime;
        width = allPieces.GetLength(0);
        height = allPieces.GetLength(1);

        List<Gamepiece> normalPiecesList = new List<Gamepiece>();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var piece = allPieces[i, j];
                if (piece != null)
                {
                    if (piece.gamepieceType == GamepieceType.Normal && !normalPiecesList.Contains(piece))
                    {
                        normalPiecesList.Add(piece);
                        allPieces[i, j] = null;
                    }
                }
            }
        }
        return normalPiecesList;
    }

    static public List<Gamepiece> ShuffleList(List<Gamepiece> piecesToShuffle)
    {
        int maxCount = piecesToShuffle.Count;

        for (int i = 0; i < maxCount; i++)
        {
            int r= UnityEngine.Random.Range(i, maxCount);

            if (r==i)
            {
                continue;
            }
            var tempGamepice = piecesToShuffle[r];
            piecesToShuffle[r] = piecesToShuffle[i];
            piecesToShuffle[i] = tempGamepice;
        }
        return piecesToShuffle;
    }

}
