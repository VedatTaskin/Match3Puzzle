using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public interface IGamepieceRule
{
    bool PerformRule(Gamepiece gamepiece, Board board, Gamepiece otherGamepiece);
}

public interface ISelfDestroy
{
    bool SelfDestroy(Board board, Gamepiece otherGamepiece=null);
}


