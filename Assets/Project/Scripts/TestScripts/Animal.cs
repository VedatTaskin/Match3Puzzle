using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal:MonoBehaviour
{
    public virtual AnimalType animalType { get;  }
    public virtual DogType dogType { get; }
    public virtual ColorType colorType { get; } 

}

public enum AnimalType
{
    Wolf,
    Dog,
    notAnimal
}
public enum DogType
{
    dog1,
    dog2,
    dog3,
    notDog
}

public enum ColorType
{
    white,
    green,
    blue,
    notColored
}