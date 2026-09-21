using UnityEngine;

public class EnemySenses : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (GetComponentInParent<StealthEnemy>().state != StealthEnemy.State.Investigate) //Checks if the enemy isn't already investigating
        {
            if (other.gameObject.GetComponent<StealthController>()) //Checks if it's the player
            {
                if (!other.gameObject.GetComponent<StealthController>().sneak && !GetComponentInParent<StealthEnemy>().IsThereAWall()) //Checks if the player is sneaking or if there's a wall.
                {
                    GetComponentInParent<StealthEnemy>().investigationTarget = other.gameObject.transform.position;
                    GetComponentInParent<StealthEnemy>().state = StealthEnemy.State.Investigate;
                }
            }
        }
    }
}
