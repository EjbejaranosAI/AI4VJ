using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wandering : StateMachineBehaviour
{
    Moves moves;
    BlackBoard blackboard; 

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        moves = animator.GetComponent<Moves>(); 
        blackboard = animator.GetComponent<BlackBoard>();        
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector3.Distance(blackboard.cop.position, blackboard.treasure.transform.position) > blackboard.dist2Steal)
            animator.SetTrigger("away");
        else
            moves.Wander();
    }
}
