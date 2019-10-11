using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coaster : MonoBehaviour
{
    [System.Serializable] public class OnPlaceDrink : UnityEvent { }

    public GameObject CupDisplay;
    public Valve.VR.InteractionSystem.HoverButton ResetButton;
    public OnPlaceDrink onPlaceDrink;

    private bool CupInRange = false;

    void Start()
    {
        
    }

    void Update()
    {
        CupDisplay.SetActive(DrinkFillSystem.ShakerInHand);

        if(CupInRange && !DrinkFillSystem.ShakerInHand)
        {
            onPlaceDrink.Invoke();
            ResetButton.onButtonDown.Invoke(new Valve.VR.InteractionSystem.Hand());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Shaker")
            CupInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Shaker")
            CupInRange = false;
    }
}
