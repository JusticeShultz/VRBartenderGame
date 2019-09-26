using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DrinkSystemManager : MonoBehaviour
{
    public enum DrinkIngredients
    {
        Ice,
        Ingredient1,
        Ingredient2,
        Ingredient3,
        Ingredient4,
        Ingredient5
    }

    public enum DrinkNames
    {
        DevDrink1,
        DevDrink2,
        DevDrink3,
        DevDrink4,
        DevDrink5
    }

    //Dictionary Aids (TLDR. Unity dont have dictionary support in Inspector, So we make it in arrays, then make the dictionary at start)
    [SerializeField] [ReadOnlyField] //This needs to be manually updated when we add and subtract drinks
    private List<DrinkNames> drinkListNames = new List<DrinkNames> { DrinkNames.DevDrink1,
                                                                     DrinkNames.DevDrink2,
                                                                     DrinkNames.DevDrink3,
                                                                     DrinkNames.DevDrink4,
                                                                     DrinkNames.DevDrink5};

    [SerializeField]
    private List<IngredientSO> drinkListIngredients = new List<IngredientSO>();

    private Dictionary<DrinkNames, IngredientSO> drinkList = new Dictionary<DrinkNames, IngredientSO>();

    [HideInInspector]
    public Dictionary<DrinkNames, IngredientSO> DrinkList
    {
        get
        {
            return drinkList;
        }
    }
    //Dictionary Aids

    [SerializeField] [ReadOnlyField]
    private List<DrinkIngredients> myDrink = new List<DrinkIngredients>();
    [SerializeField] [ReadOnlyField]
    private bool myDrinkIsShaken = false;


    /*
     * 3 Portions
     * Each 'pump' is a portion
     * adding to cup layers in colors
     */


    void Start()
    {
        ResetDrink();

        drinkList.Clear();
        //Initialzed the Dictionary
        for (int i = 0; i < drinkListNames.Count; i++)
        {
            drinkList.Add(drinkListNames[i], drinkListIngredients[i]);
        }
    }

    //Returns a Random Drink
    public DrinkNames RequestRandomDrink()
    {
        return drinkListNames[Random.Range(0, drinkList.Count)];
    }

    /*
     * TODO 
     * Create a myDrink 
     * myDrink validation Check (when served) (take in an enum) //Validation is on AI side, and so the AI has to detect when it was served
     * Clear myDrink
     * myDrink Status? whats in the cup, shaken?
     * myDrink Serving?
     *
     * 
     */

    public bool ValidateDrink(DrinkNames drink)
    {
        if (myDrinkIsShaken != drinkList[drink].NeedsShaking)
        {
            return false;
        }

        foreach (DrinkIngredients ingredient in Enum.GetValues(typeof(DrinkIngredients)))
        {
            //For every ingredient, check how many are in the recipie, then check how many in your drink. If they don't match, they aint the same

            int ingredientsInDrink = 0;
            int ingredientsInMyDrink = 0;

            foreach (DrinkIngredients ing in drinkList[drink].DrinkContent)
            {
                if(ing == ingredient)
                {
                    ingredientsInDrink++;
                }
            }

            foreach (DrinkIngredients ing in myDrink)
            {
                if (ing == ingredient)
                {
                    ingredientsInMyDrink++;
                }
            }

            if (ingredientsInDrink != ingredientsInMyDrink)
            {
                return false;
            }
            
        }

        return true;
    }

    public void AddIngredient(DrinkIngredients ingredient)
    {
        myDrink.Add(ingredient);
    }

    public void ShakeDrink()
    {
        myDrinkIsShaken = true;
    }

    public void ResetDrink()
    {
        myDrink.Clear();
        myDrinkIsShaken = false;
    }


}
