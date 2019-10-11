using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageDetection_Core : MonoBehaviour
{
    [System.Serializable] public class DeathEvent : UnityEvent { }

    public float Health = 100f;
    public DeathEvent OnDeath;
    
    void Update()
    {
        if(Health <= 0)
        {
            OnDeath.Invoke();
            Destroy(this);
        }
    }
}
