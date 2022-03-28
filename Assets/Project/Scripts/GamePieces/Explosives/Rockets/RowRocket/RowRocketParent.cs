using UnityEngine;
using DG.Tweening;

namespace Project.Scripts.Rocket
{
    public class RowRocketParent : MonoBehaviour
    {
        public GameObject rightRocket;
        public GameObject leftRocket;

        void Start()
        {
            var position = transform.position;
            rightRocket.transform.DOMoveX(10, 1).OnComplete(()=>TaskFinished());
            leftRocket.transform.DOMoveX(-10, 1);
        }


        void TaskFinished()
        {
            Debug.Log("bombing finished");
        }
    }
}
