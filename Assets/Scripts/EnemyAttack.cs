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
        if (EnemyMovement.debug) Debug.Log("Full Attack");
        DealDamage(attackDamage);
    }

    private void DealDamage(float amount)
    {
        switch (SettingsHandler.cache.dificulty)
        {
            case Difficulty.Easy:
                player.Damage(amount * .5f);
                break;

            default:
                player.Damage(amount);
                break;

            case Difficulty.Hard:
                player.Damage(amount * 1.5f);
                break;

            case Difficulty.Extreme:
                player.Damage(amount * 3f);
                break;
        }
    }

    private void Nibble()
    {
        if (EnemyMovement.debug) Debug.Log("Nibbled");
        DealDamage(attackDamage / 2f);
    }
}