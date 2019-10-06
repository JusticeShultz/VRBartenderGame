using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIK : MonoBehaviour
{
    public float YOffset = 0f;
    public GameObject Camera;
    public GameObject Body;
    
    void Update()
    {
        Body.transform.position = new Vector3(Camera.transform.position.x, Camera.transform.position.y + YOffset, Camera.transform.position.z);
    }
}
