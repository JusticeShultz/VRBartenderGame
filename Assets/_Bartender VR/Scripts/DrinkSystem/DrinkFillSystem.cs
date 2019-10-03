using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkFillSystem : MonoBehaviour
{
    public static bool ShakerInHand = false;

    public float FillTime = 2.0f;
    public GameObject Shaker;
    public GameObject Cup;
    public GameObject FinishedFillingEffect;

    public Rigidbody Rigidbody;
    public Valve.VR.InteractionSystem.VelocityEstimator Ve;
    public Valve.VR.InteractionSystem.Interactable Intble;
    public Valve.VR.InteractionSystem.Throwable Thrbl;
    public Valve.VR.InteractionSystem.InteractableHoverEvents IHE;
    public Valve.VR.SteamVR_Skeleton_Poser Psr;

    private bool DoOnce = false;
    private bool ShakerInPlace = false;

    void Start()
    {
        ShakerInHand = false;
    }

    void Update()
    {
        if(DoOnce)
        {
            Rigidbody.isKinematic = true;
            Shaker.transform.position = Cup.transform.position;
            DoOnce = false;
            StartCoroutine(DoFill());
        }
        else Cup.SetActive(ShakerInHand && !DoOnce);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Shaker")
        {
            if (!ShakerInHand && !DoOnce)
            {
                DoOnce = true;
                ShakerInPlace = true;
            }
        }
    }

    public void PickUpShaker()
    {
        ShakerInHand = true;
    }

    public void DropShaker()
    {
        ShakerInHand = false;
    }

    IEnumerator DoFill()
    {
        Rigidbody.isKinematic = true;
        Shaker.transform.position = Cup.transform.position;
        Ve.enabled = false;
        Intble.enabled = false;
        Thrbl.enabled = false;
        IHE.enabled = false;
        Psr.enabled = false;

        yield return new WaitForSeconds(FillTime);

        //Ray put your shit here to fill up, also make a thing to check the ingredient type : )
        print("Shit filled");
        Instantiate(FinishedFillingEffect, Shaker.transform.position, Quaternion.identity);

        Ve.enabled = true;
        Intble.enabled = true;
        Thrbl.enabled = true;
        IHE.enabled = true;
        Psr.enabled = true;
    }
}