using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableDistance : MonoBehaviour
{
    public GameObject Target;
    public float Distance = 4.0f;

    void Update()
    {
        Target.SetActive(Vector3.Distance(Movement.Player.transform.position, Target.transform.position) <= Distance); 
    }
}
