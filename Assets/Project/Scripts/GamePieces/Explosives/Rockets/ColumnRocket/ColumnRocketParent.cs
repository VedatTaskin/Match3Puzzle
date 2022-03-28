using UnityEngine;
using DG.Tweening;

public class ColumnRocketParent : MonoBehaviour
{
    public GameObject upRocket;
    public GameObject downRocket;
    public float timeToMove = 1f;

    void Start()
    {
        var position = transform.position;
        upRocket.transform.DOMoveY(20, timeToMove).OnComplete(()=>TaskFinished());
        downRocket.transform.DOMoveY(-20, timeToMove);
    }


    void TaskFinished()
    {
        Debug.Log("bombing finished");
    }
}
