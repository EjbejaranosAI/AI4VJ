using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlannerCallbacks : MonoBehaviour
{
    Moves moves;
    UnityEngine.AI.NavMeshAgent agent;

    void Start()
    {
        moves = this.GetComponent<Moves>();
        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public void Steal(GameObject treasure)
    {
        treasure.GetComponent<Renderer>().enabled = false;
    }

    public IEnumerator Seek(GameObject treasure, GameObject copGO)
    {
        agent.SetDestination(treasure.transform.position);
        while ((Vector3.Distance(treasure.transform.position, transform.position) > 2f) &&
               (Vector3.Distance(treasure.transform.position, copGO.transform.position) > 10f))
            yield return null;
    }

    public IEnumerator Wander(GameObject cop, GameObject treasure)
    {
        while (Vector3.Distance(treasure.transform.position, cop.transform.position) < 10f)
        {
            moves.Wander();
            yield return null;
        }
    }    
}
