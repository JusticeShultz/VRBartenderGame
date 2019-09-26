using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkSystemUnitTest : MonoBehaviour
{
    DrinkSystemManager drinkGod;

    public List<DrinkSystemManager.DrinkNames> RandomDrinks;

    void Start()
    {
        drinkGod = GetComponent<DrinkSystemManager>();

        //GetRandomDrinksTest(10);
    }

    void GetRandomDrinksTest(int count)
    {
        RandomDrinks.Clear();

        for (int i = 0; i < count; i++)
        {
            RandomDrinks.Add(drinkGod.RequestRandomDrink());
        }
    }




}
