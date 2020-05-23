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

    [SerializeField]
    private ZombieType type = ZombieType.Basic;

    private enum ZombieType
    {
        Basic,
        Regen,
        Tough,
        Fast
    }

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
        switch (type)
        {
            default:
                timeUntillHeal -= Time.deltaTime;
                if (timeBetweenPassiveHealth >= 0.1f)
                {
                    while (timeUntillHeal < 0f)
                    {
                        timeUntillHeal += timeBetweenPassiveHealth;
                        Heal(passiveHealth);
                    }
                }
                break;

            case ZombieType.Regen:
                if (health < maxHealth / 2)
                    timeUntillHeal -= Time.deltaTime * 2f;
                else
                    timeUntillHeal -= Time.deltaTime;
                if (timeBetweenPassiveHealth >= 0.1f)
                {
                    while (timeUntillHeal < 0f)
                    {
                        timeUntillHeal += timeBetweenPassiveHealth;
                        if (health < maxHealth / 2)
                            Heal(passiveHealth * 2);
                        else
                            Heal(passiveHealth);
                    }
                }
                break;

            case ZombieType.Tough:
                timeUntillHeal -= Time.deltaTime;
                if (timeBetweenPassiveHealth >= 0.1f)
                {
                    while (timeUntillHeal < 0f)
                    {
                        timeUntillHeal += timeBetweenPassiveHealth;
                        Heal(passiveHealth * 0.1f * health);
                    }
                }
                armor = health / 10f;
                break;

            case ZombieType.Fast:

                movement.speed = health / 3f + 1f;

                switch (SettingsHandler.cache.dificulty)
                {
                    case Difficulty.Easy:
                        movement.speed *= .8f;
                        break;

                    default:
                        break;

                    case Difficulty.Hard:
                        movement.speed *= 1.1f;
                        break;

                    case Difficulty.Extreme:
                        movement.speed *= 1.2f;
                        break;
                }
                break;
        }
    }

    public void Heal(float amount)
    {
        health = Mathf.Clamp(health + Mathf.Clamp(amount, 0f, Mathf.Infinity), 0f, maxHealth);
        UpdateHealthbar();
    }

    public void Damage(float amount)
    {
        switch (SettingsHandler.cache.dificulty)
        {
            case Difficulty.Easy:
                amount *= 1.5f;
                break;

            case Difficulty.Hard:
                amount *= .9f;
                break;

            case Difficulty.Extreme:
                amount *= .8f;
                break;
        }

        health = Mathf.Clamp(health - Mathf.Clamp(amount, 0f, Mathf.Infinity), 0f, maxHealth);
        UpdateHealthbar();
        if (!dead && health == 0f)
        {
            dead = true;
            OnDeath?.Invoke();
        }
        movement.Stun(.2f);
    }
}