using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMenu : MonoBehaviour
{
    //HandMenu System
    [SerializeField]
    private DrinkSystemManager DrinkManager;


    [Space(10)]
    [Header("Material Shit")]

    [SerializeField]
    private MeshRenderer MenuDrinkRenderer;

    [SerializeField]
    private List<Material> ServingCountMat;

    [SerializeField]
    private List<MeshRenderer> ServingMesh;

    [SerializeField]
    private GameObject shakerIcon;

    private Dictionary<DrinkSystemManager.DrinkIngredients, MeshRenderer> MenuIngredientsMesh = new Dictionary<DrinkSystemManager.DrinkIngredients, MeshRenderer>();

    void OnEnable()
    {
        if (MenuIngredientsMesh.Count == 0)
        {
            return;
        }
        shaker.SetActive(false);
        SetHandMenuMaterial();
    }


    void Start()
    {
        for (int i = 0; i < ServingMesh.Count; i++)
        {
            MenuIngredientsMesh.Add((DrinkSystemManager.DrinkIngredients)i, ServingMesh[i]);
        }


        SetHandMenuMaterial();
    }


    public void SetHandMenuMaterial()
    {
        //Change the Menu Drink Icon

        if (DrinkManager.MyDrink.Count >= 5)
        {
            if (DrinkManager.MyDinkIsShaken == false)
            {
                shaker.SetActive(true);
            }
            else 
            {
                shaker.SetActive(false);
                MenuDrinkRenderer.gameObject.SetActive(true);
                MenuDrinkRenderer.material.SetTexture("_MainTex", DrinkManager.DrinkList[DrinkManager.ScanDrink()].DrinkImg);
            }
        }
        else
        {
            MenuDrinkRenderer.gameObject.SetActive(false);
        }
        //Add Shake shit

        //Change the ingredient serving
        foreach (DrinkSystemManager.DrinkIngredients ingredient in Enum.GetValues(typeof(DrinkSystemManager.DrinkIngredients)))
        {

            //For every ingredient, check how many are in the recipie, then check how many in your drink. If they don't match, they aint the same
            int ingredientsInDrink = 0;

            foreach (DrinkSystemManager.DrinkIngredients ing in DrinkManager.MyDrink)
            {
                if (ing == ingredient)
                {
                    ingredientsInDrink++;
                }
            }


            try
            {
                MenuIngredientsMesh[ingredient].material = ServingCountMat[ingredientsInDrink];
            }
            catch (KeyNotFoundException ass)
            {
                Debug.Log($"{ingredient} and {ingredientsInDrink}");
                throw;
            }
        }
    }
}
