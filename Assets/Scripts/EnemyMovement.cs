using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Controll;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : Controll.CharacterController
{
    private GameObject player
    {
        get
        {
            return PlayerController.player;
        }
    }

    public float attackDistance = 0.5f;

    private NavMeshAgent agent;

    public event System.Action OnStartAttack;

    public event System.Action OnStopAttack;

    public bool Stunned
    {
        get { return stunDuration > 0f; ; }
    }

    public float stunDuration = 0f;
    public float speed = 4.5f;

    private void StartAttack()
    {
        agent.isStopped = true;
        Debug.Log("Started attacking");
    }

    private void StopAttack()
    {
        agent.isStopped = false;
        Debug.Log("Stopped attacking");
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        OnStartAttack += StartAttack;
        OnStopAttack += StopAttack;
    }

    public void Stun(float duration)
    {
        if (Stunned)
        {
            stunDuration += duration / 2f;
        }
        else
        {
            stunDuration += duration;
        }
    }

    private void Update()
    {
        agent.destination = player.transform.position;
        if (Vector3.Distance(transform.position, player.transform.position) < attackDistance)
        {
            //Debug.Log("Attacking");
            bool wasAttacking = isAttacking;
            isAttacking = true;
            if (!wasAttacking)
            {
                OnStartAttack?.Invoke();
            }
        }
        else
        {
            //Debug.Log("Not attacking");
            bool wasAttacking = isAttacking;
            isAttacking = false;
            if (wasAttacking)
            {
                OnStopAttack?.Invoke();
            }
        }
        if (isAttacking)
        {
            moveState = SpeedState.Idle;
            agent.speed = 0f;
        }
        else
        {
            if (Stunned)
            {
                moveState = SpeedState.Walking;
                agent.speed = speed / 3f;
            }
            else
            {
                moveState = SpeedState.Running;
                agent.speed = speed;
            }
        }
        stunDuration -= Time.deltaTime;
        if (stunDuration < 0f)
            stunDuration = 0f;
    }
}