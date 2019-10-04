using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakerRespawn : MonoBehaviour
{
    public GameObject Shaker;
    public Rigidbody ShakerRigidbody;
    public Vector3 RespawnPoint;
    public Vector3 RespawnRotation;
    
    public void Respawn()
    {
        if(!DrinkFillSystem.ShakerInHand)
        {
            Shaker.transform.position = RespawnPoint;
            Shaker.transform.rotation = Quaternion.Euler(RespawnRotation);
            ShakerRigidbody.velocity = Vector3.zero;
        }
    }
}
