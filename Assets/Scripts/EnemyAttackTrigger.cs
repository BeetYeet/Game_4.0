using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    public EnemyAttack attackScript;

    public void Attack()
    {
        if(EnemyMovement.debug) Debug.Log("Animation Attacked");
        attackScript.TriggerAttack();
    }
}