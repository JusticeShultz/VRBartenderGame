using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrinkSystemUnitTest : MonoBehaviour
{
    DrinkSystemManager drinkGod;

    public List<DrinkSystemManager.DrinkNames> RandomDrinks;

    public Image TargetDrinkImage;

    public Image YourDrinkImage;


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
        //NewRequest();
    }

    void GetRandomDrinksTest(int count)
    {
        RandomDrinks.Clear();

        for (int i = 0; i < count; i++)
        {
            RandomDrinks.Add(drinkGod.RequestRandomDrink());
        }
    }


    //public void NewRequest()
    //{
    //    TargetDrink = drinkGod.RequestRandomDrink();
    //    ResetDrink();
    //    drinkIsGood = false;
    //    drinkIsBad = false;
    //    TargetDrinkImage.sprite = drinkGod.DrinkList[TargetDrink].DrinkImg.;
    //    TargetDrinkImage.SetNativeSize();
    //}

    //public void CheckDrink()
    //{
    //    drinkIsGood = drinkGod.ValidateDrink(TargetDrink);
    //    drinkIsBad = !drinkGod.ValidateDrink(TargetDrink);

    //    YourDrinkImage.sprite = drinkGod.DrinkList[drinkGod.ScanDrink()].DrinkImg;

    //    YourDrinkImage.SetNativeSize();
    //}

    public void ResetDrink()
    {
        drinkGod.ResetDrink();
        drinkIsGood = false;
        drinkIsBad = false;
        YourDrinkImage.sprite = null;
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
        drinkGod.AddIngredient(DrinkSystemManager.DrinkIngredients.Adelhyde);
    }
    
    public void AddIngredient2()
    {
        drinkGod.AddIngredient(DrinkSystemManager.DrinkIngredients.PowderedDelta);
    }

    public void AddIngredient3()
    {
        drinkGod.AddIngredient(DrinkSystemManager.DrinkIngredients.BronsonExtract);
    }

    public void AddIngredient4()
    {
        drinkGod.AddIngredient(DrinkSystemManager.DrinkIngredients.Flanergide);
    }

    public void AddIngredient5()
    {
        drinkGod.AddIngredient(DrinkSystemManager.DrinkIngredients.Karmotrine);
    }




}
