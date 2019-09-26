using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkSystemUnitTest : MonoBehaviour
{
    DrinkSystemManager drinkGod;

    public List<DrinkSystemManager.DrinkNames> RandomDrinks;

    [ReadOnlyField]
    public DrinkSystemManager.DrinkNames TargetDrink;

    [ReadOnlyField]
    public bool drinkIsGood = false;
    [ReadOnlyField]
    public bool drinkIsBad = false;

    void Start()
    {
        drinkGod = GetComponent<DrinkSystemManager>();

        //GetRandomDrinksTest(10);
        NewRequest();
    }

    void GetRandomDrinksTest(int count)
    {
        RandomDrinks.Clear();

        for (int i = 0; i < count; i++)
        {
            RandomDrinks.Add(drinkGod.RequestRandomDrink());
        }
    }


    public void NewRequest()
    {
        TargetDrink = drinkGod.RequestRandomDrink();
        ResetDrink();
        drinkIsGood = false;
        drinkIsBad = false;
    }

    public void CheckDrink()
    {
        drinkIsGood = drinkGod.ValidateDrink(TargetDrink);
        drinkIsBad = !drinkGod.ValidateDrink(TargetDrink);
    }

    public void ResetDrink()
    {
        drinkGod.ResetDrink();
        drinkIsGood = false;
        drinkIsBad = false;

    }
    
    public void AddIce()
    {
        drinkGod.AddIngredient(DrinkSystemManager.DrinkIngredients.Ice);
    }

    public void Shake()
    {
        drinkGod.ShakeDrink();
    }
    
    public void AddIngredient1()
    {
        drinkGod.AddIngredient(DrinkSystemManager.DrinkIngredients.Ingredient1);
    }
    
    public void AddIngredient2()
    {
        drinkGod.AddIngredient(DrinkSystemManager.DrinkIngredients.Ingredient2);
    }

    public void AddIngredient3()
    {
        drinkGod.AddIngredient(DrinkSystemManager.DrinkIngredients.Ingredient3);
    }

    public void AddIngredient4()
    {
        drinkGod.AddIngredient(DrinkSystemManager.DrinkIngredients.Ingredient4);
    }

    public void AddIngredient5()
    {
        drinkGod.AddIngredient(DrinkSystemManager.DrinkIngredients.Ingredient5);
    }




}
