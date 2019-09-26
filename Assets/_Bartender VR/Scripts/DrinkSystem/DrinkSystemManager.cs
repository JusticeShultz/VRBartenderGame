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
        Ingredient5,
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
    private List<IngredientSO> drinkListIngredients;

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

    /*
     * 3 Portions
     * Each 'pump' is a portion
     * adding to cup layers in colors
     */


    void Start()
    {
        //Initialzed the Dictionary
        for(int i = 0; i < drinkListNames.Count; i++)
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
     * Create a Drink 
     * Drink validation Check (when served) (take in an enum)
     * Drink Clear
     * 
     */



}
