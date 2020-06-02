using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{

    [SerializeField]
    private GameObject pickupText = null;

    private float minHealthGiven = 20f;
    private float maxHealthGiven = 30f;

    [SerializeField]
    private GameObject healEffect = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        Instantiate(pickupText, transform.position, Quaternion.identity).GetComponent<AmmoRise>().SetText("");
        PlayerController.player.GetComponent<PlayerHealth>().Heal(Random.Range(minHealthGiven, maxHealthGiven));

        Instantiate(healEffect, other.transform.position, Quaternion.identity, other.transform);

        Destroy(gameObject);
    }
}