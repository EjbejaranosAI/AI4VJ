using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiding : StateMachineBehaviour
{
    Moves moves;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        moves = animator.GetComponent<Moves>();         
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        moves.Hide();
    }
}
