using UnityEngine;
using DG.Tweening;

public class ColumnRocketParent : MonoBehaviour
{
    public GameObject upRocket;
    public GameObject downRocket;
    public float timeToMove = 1.2f;
    private Board board;

    void Start()
    {
        board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();

        upRocket.transform.DOMoveY(20, timeToMove).OnComplete(()=>TaskFinished());
        downRocket.transform.DOMoveY(-20, timeToMove);
    }
    
    
    public void TaskFinished()
    {
        Debug.Log("bombing finished");
        //we lock the tiles that column we are in,
        for (int i = 0; i < board.height; i++)
        {
            board.tileData.allTiles[ (int) transform.position.x, i].isLockedAgainstCollapse = false;
        }
    }
    


}
