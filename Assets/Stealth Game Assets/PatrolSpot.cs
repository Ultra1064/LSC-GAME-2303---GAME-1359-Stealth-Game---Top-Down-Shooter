using UnityEngine;

public class PatrolSpot : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) //This is used to cycle the patrolling spots of each enemy.
    {
        if (other.gameObject.GetComponent<StealthEnemy>())
        {
            for(int x = 0; x < other.gameObject.GetComponent<StealthEnemy>().patrolSpots.Length; x++)
            {//Has to be gameObject.transform to be able to compare
                if (this.gameObject.transform == other.gameObject.GetComponent<StealthEnemy>().patrolSpots[x]) //The only issue with this code, is that you cannot reuse a patrol spot in multiple spots in the array, it'll find itself at multiple points in the for loop, possibly ruining the pathing.
                {
                    //Debug.Log(x);
                    if (x < other.gameObject.GetComponent<StealthEnemy>().patrolSpots.Length - 1) //If the position of this spot isn't the end of the array, go to the next element, else start over.
                        other.gameObject.GetComponent<StealthEnemy>().currentPatrolSpot = other.gameObject.GetComponent<StealthEnemy>().patrolSpots[x + 1];
                    else
                        other.gameObject.GetComponent<StealthEnemy>().currentPatrolSpot = other.gameObject.GetComponent<StealthEnemy>().patrolSpots[0];
                    break; //Ends the for loop early, as we don't need it to finish iterating once it finds what it's looking for. Optimization!
                }
            }
        }
    }
}
