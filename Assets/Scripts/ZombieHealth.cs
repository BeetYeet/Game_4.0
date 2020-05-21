using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    [SerializeField]
    private float health;

    [SerializeField]
    private float maxHealth;

    public float Health
    {
        get { return health; }
    }

    [SerializeField]
    private float armor;

    public float Armor
    {
        get { return armor; }
    }

    [SerializeField, Range(0.1f, 5f)]
    private float timeBetweenPassiveHealth = 1f;

    [SerializeField, Range(0.1f, 5f)]
    private float passiveHealth = 2f;

    private float timeUntillHeal = 1f;

    [SerializeField]
    private HealthbarHandler healthBar;

    public System.Action OnDeath;

    [SerializeField]
    private bool dead = false;

    public bool Dead
    {
        get { return dead; }
    }

    public GameObject DeathReplacement;
    private EnemyMovement movement;

    private void Start()
    {
        UpdateHealthbar();
        healthBar.ResetValues();
        OnDeath += () =>
        {
            Destroy(gameObject);
            Instantiate(DeathReplacement, transform.position, transform.rotation);
            GameController.instance.OnEnemyDie();
        };
        movement = GetComponent<EnemyMovement>();
    }

    private void UpdateHealthbar()
    {
        healthBar.UpdateHealth(health / maxHealth);
    }

    private void Update()
    {
        timeUntillHeal -= Time.deltaTime;
        if (timeBetweenPassiveHealth >= 0.1f)
            while (timeUntillHeal < 0f)
            {
                timeUntillHeal += timeBetweenPassiveHealth;
                Heal(passiveHealth);
            }
    }

    public void Heal(float amount)
    {
        health = Mathf.Clamp(health + Mathf.Clamp(amount, 0f, Mathf.Infinity), 0f, maxHealth);
        UpdateHealthbar();
    }

    public void Damage(float amount)
    {
        health = Mathf.Clamp(health - Mathf.Clamp(amount, 0f, Mathf.Infinity), 0f, maxHealth);
        UpdateHealthbar();
        if (!dead && health == 0f)
        {
            dead = true;
            OnDeath?.Invoke();
        }
        movement.Stun(1f);
    }
}