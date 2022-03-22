using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Gamepiece : MonoBehaviour
{
    public int xIndex;
    public int yIndex;
    public float fallTime = 0.3f;

    public virtual GamepieceType gamepieceType { get; }

    public NormalGamepieceType normalGamepieceType;
    public virtual BombType bombType { get; }

    public Ease easeType;
    [Space(10)]
    [SerializeField] AnimationCurve animationCurve;

    [HideInInspector] public Board board;

    public void Init( Board _board)
    {
        board = _board;
    }

    public void SetCoordinate( int x, int y)
    {
        xIndex = x;
        yIndex = y;
    }

    public virtual void Move(int destX,int destY, float timeToMove, MoveType movetype)
    {

        if (movetype == MoveType.Swap)
        {
            transform.DOMove(new Vector3(destX, destY, transform.position.z), timeToMove).SetEase(Ease.OutQuart).
                OnComplete(() =>
                {
                    board.PlaceGamePiece(this, destX, destY);

                });
        }
        else if (movetype == MoveType.Fall)
        {
            timeToMove = Mathf.Clamp(timeToMove, 0.25f, 0.6f);
            transform.DOMove(new Vector3(destX, destY, transform.position.z), timeToMove).SetEase(animationCurve).
                OnComplete(() =>
                {
                    board.PlaceGamePiece(this, destX, destY);
                    StartCoroutine(board.CheckMatchesAfterFallDown(this));
                });
        }
    }
}

