using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDetection_Child : MonoBehaviour
{
    public enum Importance { NonVital, Vital, VeryVital, Critical, InstaDeath }

    public float ForceSensitivity = 3f;
    public Importance _Importance;
    public DamageDetection_Core Core;

    private void OnCollisionEnter(Collision collision)
    {
        if (Core)
        {
            if(collision.relativeVelocity.magnitude >= ForceSensitivity)
            {
                if (_Importance == Importance.NonVital)
                    Core.Health -= 5f;
                else if (_Importance == Importance.Vital)
                    Core.Health -= 15f;
                else if (_Importance == Importance.VeryVital)
                    Core.Health -= 35f;
                else if (_Importance == Importance.Critical)
                    Core.Health -= 65f;
                else if (_Importance == Importance.InstaDeath)
                    Core.Health -= Mathf.Infinity;
            }
        }
        else Destroy(this);
    }
}