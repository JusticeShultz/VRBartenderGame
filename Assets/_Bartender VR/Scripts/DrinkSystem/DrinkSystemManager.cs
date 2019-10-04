using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DrinkSystemManager : MonoBehaviour
{
    public enum DrinkIngredients
    {
        Ice = 0,
        Adelhyde = 1,
        PowderedDelta = 2,
        BronsonExtract = 3,
        Flanergide = 4,
        Karmotrine = 5
    }

    public enum DrinkNames
    {
        BadDrink = -1,
        BadTouch,
        Beer,
        BlueFairy,
        Brandtini,
        FringeWeaver,
        GutPunch,
        Moonblast,
        PianoMan,
        PileDriver,
        SugarRush,
        Suplex,
    }

    //Dictionary Aids (TLDR. Unity dont have dictionary support in Inspector, So we make it in arrays, then make the dictionary at start)
    [SerializeField] [ReadOnlyField] //This needs to be manually updated when we add and subtract drinks
    private List<DrinkNames> drinkListNames = new List<DrinkNames> { DrinkNames.BadDrink,
                                                                     DrinkNames.BadTouch,
                                                                     DrinkNames.Beer,
                                                                     DrinkNames.BlueFairy,
                                                                     DrinkNames.Brandtini,
                                                                     DrinkNames.FringeWeaver,
                                                                     DrinkNames.GutPunch,
                                                                     DrinkNames.Moonblast,
                                                                     DrinkNames.PianoMan,
                                                                     DrinkNames.PileDriver,
                                                                     DrinkNames.SugarRush,
                                                                     DrinkNames.Suplex
                                                                    };

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


    //Menu System
    [SerializeField] [ReadOnlyField]
    private DrinkNames DrinkOnMenu;


    [Space(10)]
    [Header("Material Shit")]
    [SerializeField]
    private MeshRenderer MenuDrinkRenderer;

    [SerializeField]
    private List<Material> ServingCountMat;

    [SerializeField]
    private List<MeshRenderer> ServingMesh;

    private Dictionary<DrinkIngredients, MeshRenderer> MenuIngredientsMesh = new Dictionary<DrinkIngredients, MeshRenderer>();


    [Space(10)]
    [Header("Glitch Shit")]

    [SerializeField]
    private List<MeshRenderer> GlitchIcons;

    public bool ZOOP;

    [SerializeField]
    private float GlitchDuration;


    [SerializeField]
    private float RandomGlitchTime;

    [SerializeField]
    private float RandomGlitchIntensity;

    [SerializeField]
    private float DrinkChangeGlitchIntensity;

    [SerializeField]
    [ReadOnlyField]
    private float GlitchTimer;


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


        for (int i = 0; i < ServingMesh.Count; i++)
        {
            MenuIngredientsMesh.Add((DrinkIngredients)i, ServingMesh[i]);
        }


        SetMenuMaterial();
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            MenuSwipe(true);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            MenuSwipe(false);
        }


        //Glitch
        GlitchTimer += Time.deltaTime;
        if (GlitchTimer > RandomGlitchTime)
        {
            GlitchTimer -= RandomGlitchTime;
            StartCoroutine(GlitchEffect(RandomGlitchIntensity));
        }

    }



    public DrinkNames ScanDrink()
    {
        foreach (DrinkNames drinks in Enum.GetValues(typeof(DrinkNames)))
        {
            if (ValidateDrink(drinks) == true)
            {
                return drinks;
            }
        }

        return DrinkNames.BadDrink;
    }
    
    public bool ValidateDrink(DrinkNames drink)
    {
        if (drink == DrinkNames.BadDrink)
        {
            return false;
        }

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

    public void MenuSwipe(bool isToRight)
    {
        GlitchTimer = 0.0f;

        if (isToRight == true)
        {
            DrinkNames[] Arr = (DrinkNames[])Enum.GetValues(typeof(DrinkNames));
            int idx = Array.IndexOf<DrinkNames>(Arr, DrinkOnMenu) + 1;

            if (idx == Arr.Length - 1)
            {
                DrinkOnMenu = Arr[0];
            }
            else
            {
                DrinkOnMenu = Arr[idx];
            }
        }
        else
        {
            DrinkNames[] Arr = (DrinkNames[])Enum.GetValues(typeof(DrinkNames));
            int idx = Array.IndexOf<DrinkNames>(Arr, DrinkOnMenu) - 1;

            if (idx == -1)
            {
                DrinkOnMenu = Arr[Arr.Length - 2];
            }
            else
            {
                DrinkOnMenu = Arr[idx];
            }

        }
        SetMenuMaterial();

        StartCoroutine(GlitchEffect(DrinkChangeGlitchIntensity));
    }

    private void SetMenuMaterial()
    {
        //Change the Menu Drink Icon
        MenuDrinkRenderer.materials[0].SetTexture("_MainTex", drinkList[DrinkOnMenu].DrinkImg);

        //Change the ingredient serving
        foreach (DrinkIngredients ingredient in Enum.GetValues(typeof(DrinkIngredients)))
        {

            //For every ingredient, check how many are in the recipie, then check how many in your drink. If they don't match, they aint the same
            int ingredientsInDrink = 0;

            foreach (DrinkIngredients ing in drinkList[DrinkOnMenu].DrinkContent)
            {
                if (ing == ingredient)
                {
                    ingredientsInDrink++;
                }
            }

            MenuIngredientsMesh[ingredient].material = ServingCountMat[ingredientsInDrink];

        }


    }

    private IEnumerator GlitchEffect(float intensity)
    {
        ZOOP = false;
        float time = 0;

        while (time < GlitchDuration)
        {

            foreach (MeshRenderer mesh in GlitchIcons)
            {
                mesh.material.SetFloat("_GlitchIntensity", Random.Range(-intensity, intensity));
            }

            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        foreach (MeshRenderer mesh in GlitchIcons)
        {
            mesh.material.SetFloat("_GlitchIntensity", 0.0f);
        }

    }

    public void MenuSwipeRight()
    {
        MenuSwipe(true);
    }

    public void MenuSwipeLeft()
    {
        MenuSwipe(false);
    }


    ////Add Manual Ingredient

    //public void AddIce()
    //{
    //    AddIngredient(DrinkIngredients.Ice);
    //}

    //public void AddAdelhyde()
    //{
    //    AddIngredient(DrinkIngredients.Adelhyde);
    //}

    //public void AddPowderedDelta()
    //{
    //    AddIngredient(DrinkIngredients.PowderedDelta);
    //}

    //public void AddBronsonExtract()
    //{
    //    AddIngredient(DrinkIngredients.BronsonExtract);
    //}

    //public void AddFlanergide()
    //{
    //    AddIngredient(DrinkIngredients.Flanergide);
    //}

    //public void AddKarmotrine()
    //{
    //    AddIngredient(DrinkIngredients.Karmotrine);
    //}


}
