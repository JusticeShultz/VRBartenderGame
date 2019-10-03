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
    public ParticleSystem Liquid;
    public ParticleSystem Drip;

    public Rigidbody Rigidbody;

    public Valve.VR.InteractionSystem.VelocityEstimator Ve;
    public Valve.VR.InteractionSystem.Interactable Intble;
    public Valve.VR.InteractionSystem.Throwable Thrbl;
    public Valve.VR.InteractionSystem.InteractableHoverEvents IHE;
    public Valve.VR.SteamVR_Skeleton_Poser Psr;

    private bool DoOnce = false;
    private bool ShakerInPlace = false;
    private bool IsButtonDown = false;
    private bool Filled = false;

    void Start()
    {
        ShakerInHand = false;
    }

    void Update()
    {
        if (!ShakerInHand && ShakerInPlace && !Filled)
        {
            Rigidbody.isKinematic = true;
            Shaker.transform.position = Cup.transform.position + new Vector3(0.04f, -0.1f, 0);
            Shaker.transform.rotation = Cup.transform.rotation;

            if (!DoOnce && IsButtonDown)
            {
                DoOnce = true;
                StartCoroutine(DoFill());
            }
        }

        if (!ShakerInPlace) Filled = false;

        Cup.SetActive(ShakerInHand && !DoOnce);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Shaker")
            ShakerInPlace = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Shaker")
        {
            ShakerInPlace = false;
            DoOnce = false;
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
        Filled = true;
        DoOnce = false;
    }

    public void ButtonDown()
    {
        //print("Down");
        IsButtonDown = true;

        if(!ShakerInHand && ShakerInPlace)
            Liquid.Stop();
        else
            Liquid.Play();
    }

    public void ButtonUp()
    {
        //print("Up");

        IsButtonDown = false;

        Liquid.Stop();
        Drip.Play();
    }
}