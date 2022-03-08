using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public interface IBombRule
{
    List<Gamepiece> PerformRule(Gamepiece gamepiece);
}