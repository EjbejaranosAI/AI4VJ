using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Approaching : StateMachineBehaviour
{
    Moves moves;
    BlackBoard blackboard; 

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        moves = animator.GetComponent<Moves>(); 
        blackboard = animator.GetComponent<BlackBoard>();        
        animator.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 2f;
        moves.Seek(blackboard.treasure.transform.position);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector3.Distance(blackboard.treasure.transform.position, animator.transform.position) < 2f)
        {
            blackboard.treasure.GetComponent<Renderer>().enabled = false;
            Debug.Log("Stolen");
            animator.SetTrigger("stolen");
        }
        else
            if (Vector3.Distance(blackboard.cop.position, blackboard.treasure.transform.position) < blackboard.dist2Steal)
            {
                animator.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 1f;
                animator.SetTrigger("near");
            };
    }
}
