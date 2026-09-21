using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleManager : MonoBehaviour
{
    public static RumbleManager Instance;
    [SerializeField] private Gamepad gp;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public void RumblePulse(float lowFreq, float highFreq, float duration)
    {
        gp = Gamepad.current;
        if(gp != null)
        {
            gp.SetMotorSpeeds(lowFreq, highFreq);
            StartCoroutine(StopRumble(duration));
        }
    }

    IEnumerator StopRumble(float duration)
    {
        yield return new WaitForSeconds(duration);
        gp.SetMotorSpeeds(0, 0);
    }
}
