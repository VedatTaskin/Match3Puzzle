using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombs : Gamepiece, IGamepieceRule,ISelfDestroy
{
    [HideInInspector] public bool anyMatches = false;

    public override GamepieceType gamepieceType => GamepieceType.Bomb;
    private void OnEnable() => normalGamepieceType = NormalGamepieceType.None;
    public virtual bool PerformRule(Gamepiece gamepiece,Board board, Gamepiece otherGamepiece=null)
    {
        return anyMatches;
    }
    public virtual bool SelfDestroy(Board board, Gamepiece otherGamepiece)
    {
        Debug.Log("Self Destroying");
        return false;
    }
}
