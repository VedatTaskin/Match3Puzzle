using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{

    public GameObject dog1GO;
    public GameObject dog2GO;

    // Start is called before the first frame update
    void Start()
    {

        print(dog2GO.GetComponent<Animal>().animalType);
        print(dog2GO.GetComponent<Animal>().dogType);

        print("**************");

        print(dog1GO.GetComponent<Animal>().animalType);
        print(dog1GO.GetComponent<Animal>().dogType);

    }


}
