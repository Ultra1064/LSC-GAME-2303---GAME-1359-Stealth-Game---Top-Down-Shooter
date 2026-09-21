using UnityEngine;
using System.Collections;

public class HitStopManager : MonoBehaviour
{
    public static HitStopManager instance;

    private float hitStopDuration;
    private float timeSpeed;
    [SerializeField] private float pendingStopDuration;
    [SerializeField] private bool isFrozen;

    private void Start()
    {
        if (instance == null)
            instance = this;

        isFrozen = false;
    }

    private void Update()
    {
        if (pendingStopDuration != 0 && !isFrozen)
        {
            StartCoroutine(HitStopTimer());
        }
    }

    public void DoHitStop(float duration, float speed)
    {
        hitStopDuration = duration;
        timeSpeed = speed;
        pendingStopDuration = hitStopDuration;
    }
    IEnumerator HitStopTimer()
    {
        isFrozen = true;
        var ogTime = Time.timeScale;
        Time.timeScale = timeSpeed; //Chaos Control?
        yield return new WaitForSecondsRealtime(hitStopDuration); //Real Time is real time
        Time.timeScale = ogTime;
        pendingStopDuration = 0;
        isFrozen = false;
    }
}
