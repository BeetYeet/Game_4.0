using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePartExplode : MonoBehaviour
{
    public float randomPos;
    public float velocity;

    public float decayTime;
    public float decayTimeVariance;

    private void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        transform.position = transform.parent.position + Random.insideUnitSphere * randomPos;
        rb.AddForce(velocity * Random.insideUnitSphere, ForceMode.VelocityChange);
        decayTime += Random.Range(-decayTimeVariance, decayTimeVariance);
    }

    private void Update()
    {
        if (decayTime > 0f)
        {
            decayTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}