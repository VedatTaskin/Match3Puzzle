using UnityEngine;
using DG.Tweening;

public class ColumnRocketParent : MonoBehaviour
{
    public GameObject upRocket;
    public GameObject downRocket;

    void Start()
    {
        var position = transform.position;
        upRocket.transform.DOMoveY(10, 1).OnComplete(()=>TaskFinished());
        downRocket.transform.DOMoveY(-10, 1);
    }


    void TaskFinished()
    {
        Debug.Log("bombing finished");
    }
}
