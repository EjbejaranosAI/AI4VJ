using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    // Reference to the NavMeshAgent component that will be used for navigation
    NavMeshAgent agent;

    // The target GameObject that the bot will interact with (pursue, seek, or flee from)
    public GameObject target;
    Drive ds;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the NavMeshAgent component
        agent = this.GetComponent<NavMeshAgent>();
        ds = target.GetComponent<Drive>();
    }

    // Seek method: Moves the bot towards the specified location
    // This is a direct pursuit, where the bot sets its destination to the target's current position
    void Seek(Vector3 location)
    {
        // Set the destination of the agent to the given location
        agent.SetDestination(location);
    }

    // Flee method: Moves the bot away from the specified location
    // This behavior makes the bot "run away" from the target
    void Flee(Vector3 location)
    {
        // Calculate the direction vector from the target to the bot (flee direction)
        Vector3 fleeVector = location - this.transform.position;
        // Set the destination to a point in the opposite direction, making the bot flee
        agent.SetDestination(this.transform.position - fleeVector);
    }

    // Pursue method: Predicts the future position of the target and moves towards that predicted position
    // This method handles more advanced chasing by considering the target's movement speed and direction
    void Pursue()
    {
        // Calculate the direction vector from the bot to the target
        Vector3 targetDir = target.transform.position - this.transform.position;
        
        // Calculate the angle between the forward direction of the bot and the forward direction of the target
        float relativeHeading = Vector3.Angle(this.transform.forward, this.transform.TransformVector(target.transform.forward));

        // Calculate the angle between the bot's forward direction and the direction towards the target
        float toTarget = Vector3.Angle(this.transform.forward, this.transform.TransformVector(targetDir));

        // Conditional logic to decide whether to pursue directly or to predict the future position
        // First condition: If the target is more than 90 degrees ahead of the bot and the bot is nearly aligned with the target
        // OR if the target's speed is close to zero, pursue the target directly
        if((toTarget > 90 && relativeHeading < 20) || ds.currentSpeed < 0.01f)
        {
            // Use direct pursuit by seeking the target's current position
            Seek(target.transform.position);
            return;  // Exit the method early if direct pursuit is chosen
        }

        // Second condition: Predict where the target will be in the future based on its speed and distance
        // Calculate how far ahead to look based on the distance and speed differences between the bot and the target
        float lookAhead = targetDir.magnitude / (agent.speed + ds.currentSpeed);
        
        // Seek the predicted future position of the target
        Seek(target.transform.position + target.transform.forward * lookAhead);
    }

    // Evade method: Moves the bot away from the target if it is within a certain angle
    void Evade()
    {
        // Compute targetDir
        Vector3 targetDir = target.transform.position - this.transform.position;

        float relativeHeading = Vector3.Angle(this.transform.forward, this.transform.TransformVector(target.transform.forward));
        float toTarget = Vector3.Angle(this.transform.forward, this.transform.TransformVector(targetDir));

        if((toTarget < 90 && relativeHeading < 20) || ds.currentSpeed <0.01f)
        {
            Flee(target.transform.position);
            return;

        }
        // Compute the lookAhead
        float lookAhead = targetDir.magnitude / (agent.speed + ds.currentSpeed);
        // Pass the lookAhead to the Flee

        Flee(target.transform.position + target.transform.forward * lookAhead);
    }
    // Wander method: The Wander method allows the bot to move randomly within a defined space, giving it a more lifelike behavior.
    Vector3 wanderTarget = Vector3.zero;        // Variables for wander behavior
    void Wander()
    {
        float wanderRadius = 10; // Increasing the wanderRadius you can get smoothing movements, but need to check that the position in the navmesh exist, otherwise the values could be not reasonable and the character can not move
        float wanderDistance = 10;
        float wanderJitter = 1; // Try to change the wanderJitter between small and big values to see what happens with the movement

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter, 
                                        0, 
                                        Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0,0, wanderDistance);
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);

    }

    // Update is called once per frame
    // Update is called once per frame
    void Update()
    {
        // Visualize the forward direction of the bot
        Debug.DrawRay(this.transform.position, this.transform.forward * 4, Color.blue);

        // Visualize the direction to the target
        Debug.DrawLine(this.transform.position, target.transform.position, Color.green);

        // The following lines demonstrate how you can switch between different behaviors (Seek, Flee, Pursue)
        // To test different behaviors, uncomment one of the lines below:

        // Uncomment the line below to make the bot flee from the target
        // Flee(target.transform.position);

        // Uncomment the line below to make the bot seek (move directly towards) the target
        // Seek(target.transform.position);

        // Uncomment the line below to make the bot pursue the target using the advanced prediction
        // Pursue();

        // Uncomment the line below to make the bot evade the target
        // Evade();

        // By default, the bot will wander around randomly
        Wander();
    }

}
