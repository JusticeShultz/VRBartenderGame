using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerFill : MonoBehaviour
{
    public Animator FillAmount;
    public ParticleSystem FilledEffect;
    [ReadOnlyField] public float FillValue = 0f;
    [ReadOnlyField] public bool IsFilled = false;

    void Update()
    {
        FillAmount.SetFloat("FillAmount", FillValue);
    }

    private void OnParticleCollision(GameObject other)
    {
        FillValue = Mathf.Clamp(FillValue + 0.001f, 0, 1);

        if (FillValue >= 0.95) IsFilled = true;

        if (IsFilled) FilledEffect.Play();
    }
}