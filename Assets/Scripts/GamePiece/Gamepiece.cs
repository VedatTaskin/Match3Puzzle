using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Gamepiece : MonoBehaviour
{
    public int xIndex;
    public int yIndex;
    public GamepieceType gamepieceType;
    public NormalGamepieceType normalGamepieceType;
    public SpecialGamepieceType specialGamepieceType;

    public Ease easeType;
    Board board;

    public void Init( Board _board)
    {
        board = _board;
    }

    public void SetCoordinate( int x, int y)
    {
        xIndex = x;
        yIndex = y;
    }

    public void Move(int destX,int destY, float timeToMove)
    {
        transform.DOMove(new Vector3(destX, destY, transform.position.z), timeToMove).SetEase(easeType).
            OnComplete(()=>board.PlaceGamePiece(this, destX, destY));
    } 
}

