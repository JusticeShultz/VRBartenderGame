using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ShakeTracker : MonoBehaviour
{
    [System.Serializable] public class ShakeEvent : UnityEvent { }

    public float Sensitivity = 0.1f;
    public float ShakeTime = 3f;
    public ShakeEvent onShake;
    public ShakeEvent stopShake;
    public ShakeEvent finishedShaking;
    
    private Vector3 lastPosition;
    private float _ShakeTime = 0f;

    void Update()
    {
        Vector3 difference = transform.position - lastPosition;

        if (difference.magnitude >= Sensitivity)
        {
            _ShakeTime += Time.deltaTime;
            onShake.Invoke();

            if (_ShakeTime >= ShakeTime)
            {
                _ShakeTime = 0f;
                finishedShaking.Invoke();
            }
        }
        else
        {
            if (_ShakeTime > 0f)
                stopShake.Invoke();

            _ShakeTime = 0f;
        }

        lastPosition = transform.position;
    }
}
