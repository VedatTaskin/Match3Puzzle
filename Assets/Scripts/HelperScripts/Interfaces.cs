using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public interface ISpecialGamepieceRule
{
    List<Gamepiece> PerformRule(Gamepiece gamepiece);
}