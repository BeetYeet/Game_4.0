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

    private void Start()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!inPlay)
            return;
        inPlay = false;
        gameObject.tag = "Untagged";
        gameObject.layer = LayerMask.NameToLayer(outOfPlayLayer);
        GetComponent<Rigidbody>().mass = outOfPlayMass;
        GetComponent<Rigidbody>().AddForce(collision.relativeVelocity.magnitude * collision.GetContact(0).normal / 10f, ForceMode.VelocityChange);
    }

    private void Update()
    {
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
    }
}