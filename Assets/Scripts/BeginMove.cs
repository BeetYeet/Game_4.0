using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BeginMove : MonoBehaviour
{
    public float summonTime = 1f;
    public float summonTimeVariance = 3f;
    private EnemyMovement movement;
    private NavMeshAgent agent;
    private Animator anim;

    public void Begin()
    {
        movement.enabled = true;
        agent.enabled = true;
    }

    private void Start()
    {
        movement = transform.parent.GetComponent<EnemyMovement>();
        agent = transform.parent.GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        StartCoroutine(WaitForSummon());
    }

    private IEnumerator WaitForSummon()
    {
        yield return null;
        anim.enabled = false;
        yield return new WaitForSeconds(summonTime + Random.value * summonTimeVariance);
        anim.enabled = true;
    }
}