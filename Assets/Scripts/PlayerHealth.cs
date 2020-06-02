using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth current;

    [SerializeField]
    private float health = 50f;

    [SerializeField]
    private float maxHealth = 50f;

    public float Health
    {
        get { return health; }
    }

    public float MaxHealth
    {
        get { return maxHealth; }
    }

    [SerializeField]
    private bool dead = false;

    public bool Dead
    {
        get { return dead; }
    }

    public event System.Action OnDeath;

    [SerializeField]
    private HealthbarHandler healthBar;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        UpdateHealthbar();
        healthBar.ResetValues();
        OnDeath += () =>
        {
            GameController.instance.OnRunEnd();
        };
    }

    private void UpdateHealthbar()
    {
        healthBar.UpdateHealth(health / maxHealth);
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
    }
}