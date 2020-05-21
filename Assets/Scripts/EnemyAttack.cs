using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private PlayerHealth player;
    private EnemyMovement movement;
    public Animator animator;

    [SerializeField]
    private float attackSpeed = 2f;

    [SerializeField]
    private float attackDamage = 10f;

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerHealth>();
        movement = GetComponent<EnemyMovement>();
        animator.SetFloat("AttackSpeed", attackSpeed);

        movement.OnStartAttack += Nibble;
    }

    public void TriggerAttack()
    {
        Debug.Log("Full Attack");
        DealDamage(attackDamage);
    }

    private void DealDamage(float amount)
    {
        player.Damage(amount);
    }

    private void Nibble()
    {
        Debug.Log("Nibbled");
        DealDamage(attackDamage / 2f);
    }
}