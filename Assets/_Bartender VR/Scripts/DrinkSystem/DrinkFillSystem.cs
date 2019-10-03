using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkFillSystem : MonoBehaviour
{
    public static bool ShakerInHand = false;

    public float FillTime = 2.0f;
    public GameObject Shaker;
    public GameObject Cup;

    public Valve.VR.InteractionSystem.VelocityEstimator Ve;
    public Valve.VR.InteractionSystem.Interactable Intble;
    public Valve.VR.InteractionSystem.Throwable Thrbl;
    public Valve.VR.InteractionSystem.InteractableHoverEvents IHE;
    public Valve.VR.SteamVR_Skeleton_Poser Psr;

    void Start()
    {
        ShakerInHand = false;
    }

    void Update()
    {
        Cup.SetActive(ShakerInHand);
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.name);
        StartCoroutine(DoFill());
        Ve.enabled = false;
        Intble.enabled = false;
        Thrbl.enabled = false;
        IHE.enabled = false;
        Psr.enabled = false;
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
        yield return new WaitForSeconds(FillTime);
    }
}