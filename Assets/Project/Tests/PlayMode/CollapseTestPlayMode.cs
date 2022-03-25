using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CollapseTestPlayMode
    {

        [UnityTest]
        public IEnumerator CollapseProperly()
        {
            var gameobject = new GameObject();
            var boardGO = gameobject.AddComponent<Board>();
            
            boardGO.CollapseAtAPoint(1);

            yield return new WaitForSeconds(boardGO.fallTime);
        }
    }
}
