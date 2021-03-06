﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleSmashing : MonoBehaviour
{
    public float DestroySensitivity = 10f;
    public GameObject DestroyedState;
    public Valve.VR.InteractionSystem.VelocityEstimator estimator;

    private void OnCollisionEnter(Collision collision)
    {
        if (estimator.GetAccelerationEstimate().magnitude >= DestroySensitivity)
        {
            GameObject toCreate = Instantiate(DestroyedState, transform.position, transform.rotation);
            toCreate.transform.localScale = transform.localScale;
            Destroy(gameObject);
        }
    }
}
