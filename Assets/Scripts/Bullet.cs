using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool inPlay = true;
    private float colorAlpha = 1f;
    public float decayTime = .5f;

    public string outOfPlayLayer = "Effect";
    public float outOfPlayMass = .01f;
    public float lifetime = 20f;

    public float damage;
    public float armorPenetration;
    public float armorPenetrationDecay;
    public float passiveDamageDecay;
    public bool kinetic;

    public float firedTime;

    private bool resetNextFrame = false;
    private Vector3 lastFramePos = Vector3.zero;
    private Vector3 lastFrameVel = Vector3.zero;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (kinetic)
            lifetime *= Random.value;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!inPlay)
            return;

        GetComponent<Rigidbody>().AddForce(collision.relativeVelocity.magnitude * collision.GetContact(0).normal / 10f, ForceMode.VelocityChange);
        HandleDamage(collision.gameObject.GetComponent<ZombieHealth>());

        if (damage <= 0)
            Decay();
        else
        {
            if (collision.gameObject.tag == "Enemy")
            {
                Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
                resetNextFrame = true;
            }
            else
            {
                // Hit a wall
                Decay();
            }
        }
    }

    private void HandleDamage(ZombieHealth zombieHealth)
    {
        if (zombieHealth == null)
            return;

        float health = zombieHealth.Health;
        armorPenetration = Mathf.Clamp(armorPenetration - (kinetic ? zombieHealth.Armor / 2f : zombieHealth.Armor), 0, Mathf.Infinity);
        zombieHealth.Damage(damage + armorPenetration);
        damage -= Mathf.Clamp(health - armorPenetration, 0f, Mathf.Infinity);
        if (damage > 0f)
            Debug.Log("Bullet Pierced");
        else
            Debug.Log("Bullet Bounced");
    }

    private void Decay()
    {
        inPlay = false;
        gameObject.tag = "Untagged";
        gameObject.layer = LayerMask.NameToLayer(outOfPlayLayer);
        GetComponent<Rigidbody>().mass = outOfPlayMass;
    }

    private void Update()
    {
        if (lastFramePos != Vector3.zero && resetNextFrame)
        {
            transform.position = lastFramePos + lastFrameVel * Time.deltaTime;
            rb.velocity = lastFrameVel;
            resetNextFrame = false;
        }

        lastFramePos = transform.position;
        lastFrameVel = rb.velocity;

        lifetime -= Time.deltaTime;
        if (!inPlay)
        {
            colorAlpha -= Time.deltaTime / decayTime;
            if (colorAlpha < 0f)
            {
                Destroy(gameObject);
            }
            Color c = GetComponent<MeshRenderer>().material.GetColor("_BaseColor");
            GetComponent<MeshRenderer>().material.SetColor("_BaseColor", new Color(c.r, c.g, c.b, colorAlpha));
        }
        else if (lifetime < 0f || rb.velocity.sqrMagnitude < 100f)
            Decay();
    }
}