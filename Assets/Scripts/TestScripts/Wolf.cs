

public class Wolf : Animal //Derived class
{
    public override AnimalType animalType { get => AnimalType.Wolf; }
    public override DogType dogType { get => DogType.notDog; }

}