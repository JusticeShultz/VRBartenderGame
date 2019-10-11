using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollDeath : MonoBehaviour
{
    public List<Rigidbody> Rigidbodies = new List<Rigidbody>();

    public void DoRagdoll()
    {
        for (int i = 0; i < Rigidbodies.Count; i++)
        {
            Rigidbodies[i].isKinematic = false;
        }
    }
}
