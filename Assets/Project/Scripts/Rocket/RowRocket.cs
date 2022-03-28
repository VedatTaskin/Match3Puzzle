using UnityEngine;
using DG.Tweening;

namespace Project.Scripts.Rocket
{
    public class RowRocket : MonoBehaviour
    {
        public GameObject rightRocket;
        public GameObject leftRocket;
        
        
        // Start is called before the first frame update
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
