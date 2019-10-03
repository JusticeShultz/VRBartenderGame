using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ShakeTracker : MonoBehaviour
{
    [System.Serializable] public class OnShake : UnityEvent { }
    [System.Serializable] public class StopShake : UnityEvent { }
    [System.Serializable] public class FinishedShake : UnityEvent { }

    public float Sensitivity = 0.1f;
    public float ShakeTime = 3f;
    public OnShake onShake;
    public StopShake stopShake;
    public FinishedShake finishedShaking;
    
    private Vector3 lastPosition;
    private Vector3 lastPlayerPosition;
    private float TimeSinceNotShaken = 0f;
    private float LetGoTime = 0f;

    void Update()
    {
        Vector3 difference = transform.position - lastPosition;
        Vector3 player_position_difference = Movement.Player.transform.position - lastPlayerPosition;
        
        if (difference.magnitude >= Sensitivity)
        {
            if (player_position_difference.magnitude < 0.001f)
            {
                onShake.Invoke();

                TimeSinceNotShaken += Time.deltaTime;
                LetGoTime = 0f;

                if (TimeSinceNotShaken >= ShakeTime)
                {
                    TimeSinceNotShaken = 0f;
                    finishedShaking.Invoke();
                }
            }
        }
        else
        {
            LetGoTime += Time.deltaTime;

            if (LetGoTime >= 0.2f)
            {
                if (TimeSinceNotShaken > 0f)
                    stopShake.Invoke();

                TimeSinceNotShaken = 0f;
            }
        }

        lastPosition = transform.position;
        lastPlayerPosition = Movement.Player.transform.position;
    }
}
