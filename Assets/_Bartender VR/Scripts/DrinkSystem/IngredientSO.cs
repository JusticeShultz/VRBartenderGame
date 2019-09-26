using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DrinkData", menuName = "BartenderVR/New Drink Data", order = 1)]
public class IngredientSO : ScriptableObject
{
    [SerializeField]
    private string drinkDisplayName;
    [SerializeField]
    private List<DrinkSystemManager.DrinkIngredients> drinkContent;
    [SerializeField]
    private bool needsShaking;
    
    [HideInInspector]
    public string DrinkName
    {
        get
        {
            return drinkDisplayName;
        }
    }

    [HideInInspector]
    public List<DrinkSystemManager.DrinkIngredients> DrinkContent
    {
        get
        {
            return drinkContent;
        }
    }
    
    [HideInInspector]
    public bool NeedsShaking
    {
        get
        {
            return needsShaking;
        }
    }

}
