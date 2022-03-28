using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketThatMoves : MonoBehaviour
{
    [HideInInspector] public  Board board;
    private int lastPosition;

    private void OnEnable()
    {
        board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>(); // we can think better way later 
        lastPosition = -1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward)*10, Color.yellow);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.forward);

        // If it hits something...
        if (hit.collider != null)
        {
            var xPosition = (int) hit.transform.position.x;
            var yPosition = (int) hit.transform.position.y;


            if (lastPosition != xPosition)
            {
                if (board.gamepieceData.allGamepieces[xPosition,yPosition] != null &&
                    board.gamepieceData.allGamepieces[xPosition,yPosition].pieceState == PieceState.CanMove)
                {
                    board.gamepieceData.ClearGamepieceAt(xPosition, yPosition);
                    lastPosition = xPosition;
                }

            }
        }
    }
}
