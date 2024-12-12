using UnityEngine;
using UnityEngine.AI;

public class Formation : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;
    public Vector3 pos;
    public Quaternion rot;

    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        transform.rotation = target.transform.rotation;
        transform.position = target.transform.TransformPoint(pos);
    }

    void Update()
    {
        agent.destination = target.transform.TransformPoint(pos);
    }
}
