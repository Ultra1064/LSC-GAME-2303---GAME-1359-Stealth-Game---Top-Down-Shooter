using UnityEngine;

public class HappyCube : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("VICTORY!!!");
    }
}
