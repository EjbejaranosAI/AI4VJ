using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingAI : MonoBehaviour
{
    public Transform[] waypoints;  // Array de puntos de patrullaje
    public float moveSpeed = 3f;
    public float waitTime = 1f;
    private int currentWaypointIndex = 0;

    void Start()
    {
        StartCoroutine(Patrol());
    }

    IEnumerator Patrol()
    {
        while (true)
        {
            // Mover hacia el waypoint actual
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            while (Vector3.Distance(transform.position, targetWaypoint.position) > 0.2f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);
                yield return null; // Espera al siguiente frame
            }

            // Esperar en el waypoint
            yield return new WaitForSeconds(waitTime);

            // Cambiar al siguiente waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}
