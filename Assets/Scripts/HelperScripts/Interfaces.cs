using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public interface IBombRule
{
    List<Gamepiece> PerformRule(Gamepiece gamepiece, Board board, Gamepiece otherGamepiece);
}

public interface ICollectibleRule
{
    void CollectibleRule(Gamepiece gamepiece, Board board, Gamepiece otherGamepiece);
}