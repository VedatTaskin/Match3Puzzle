using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Interfaces
{

}

public interface ISpecialGamepieceRule
{
    List<Gamepiece> PerformBombRule(Gamepiece gamepiece);
}