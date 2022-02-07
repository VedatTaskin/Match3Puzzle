using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public int xIndex;
    public int yIndex;

    bool m_isMoving = false;

    Board m_board;

    public MatchValue matchValue;
    public enum MatchValue
    {
        Yellow,
        Blue,
        Magenta,
        Indigo,
        Green,
        Teal,
        Red,
        Cyan,
        Wild
    }


    public void Update()
    {   
        /*
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move((int)transform.position.x + 1, (int) transform.position.y, 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move((int)transform.position.x - 1, (int)transform.position.y, 0.5f);
        }
        */
    }

    public void Init(Board board)
    {
        m_board = board;
    }
    
    public void SetCoordinate( int x, int y)
    {
        xIndex = x;
        yIndex = y;
    }

    public void Move( int destX, int destY, float timeToMove)
    {
        if (!m_isMoving)
        {
            StartCoroutine(MoveRoutine(new Vector3(destX, destY, 0), timeToMove));
        }

    }

    IEnumerator MoveRoutine(Vector3 destination, float timeToMove)
    {
        Vector3 startPosition = transform.position;

        bool reachedDestination = false;

        float elapsedTime = 0f;

        m_isMoving = true;

        while (!reachedDestination)
        {
            //if we are close to our destination
            if (Vector3.Distance(transform.position,destination)<0.01f)
            {
                reachedDestination = true;

                if (m_board != null)
                {
                    m_board.PlaceGamePiece(this, (int)destination.x, (int)destination.y);
                }

                break;
            }

            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp(elapsedTime / timeToMove, 0f, 1f);
                                       
            //taken from https://en.wikipedia.org/wiki/Smoothstep
            t = t * t * t * (t * (t * 6 - 15) + 10); //smoother step interpolation, Ease in- ease out 
            // I would use DOTWEEN :), Animation curve may be used

            transform.position = Vector3.Lerp(startPosition, destination, t);

            yield return null;
        }

        m_isMoving = false;
    }
}
