using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakerShake : MonoBehaviour
{
    public float speed = 1;
    public float defaultRot = 180;
    public float range = 40;
    public bool isRotatingLeft = true;

    private float n;

    void Update()
    {
        n += Time.deltaTime * speed;

        if (isRotatingLeft == true)
        {
            transform.localEulerAngles = Vector3.Lerp(Vector3.up * (defaultRot + range), Vector3.up * (defaultRot - range), n);

            if (n >= 1)
            {
                isRotatingLeft = false;
                n = 0;
            }
        }
        else 
        {
            transform.localEulerAngles = Vector3.Lerp(Vector3.up * (defaultRot - range), Vector3.up * (defaultRot + range), n);

            if (n >= 1)
            {
                isRotatingLeft = true;
                n = 0;
            }
        }
        

    }
}
