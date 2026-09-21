using UnityEngine;

public class KillScript : MonoBehaviour
{
    public float killTime = 2f;

    private void Start()
    {
        Destroy(gameObject, killTime);
    }
}
