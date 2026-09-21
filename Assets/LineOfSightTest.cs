using UnityEngine;

public class LineOfSightTest : MonoBehaviour
{
    [SerializeField] Transform target;
    float dot;

    enum State { idle, patrol, pursue, dead };
    State myState = State.idle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (myState)
        {
            case State.idle:
                UpdateIdle(); break;
            case State.patrol:
                UpdatePatrol(); break;
            case State.pursue:
                UpdatePursue(); break;
            case State.dead:
                UpdateDead(); break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, target.position);

        if (dot > 0.8)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;
        Vector3 endPoint = transform.position + transform.forward * 5;
        Gizmos.DrawLine(transform.position, endPoint);
    }

    void UpdateIdle()
    {
        Vector3 forwardDirection = transform.forward;
        Vector3 toTarget = (target.position - transform.position).normalized;

        dot = Vector3.Dot(forwardDirection, toTarget);

        if (dot > 0.8)
            myState = State.pursue;
    }
    void UpdatePursue()
    {
        //transform.Translate(transform.forward * -10 * Time.deltaTime);
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance > 10)
            myState = State.dead;
    }
    void UpdatePatrol()
    {

    }
    void UpdateDead()
    {
        Debug.Log("im ded");
    }
}
