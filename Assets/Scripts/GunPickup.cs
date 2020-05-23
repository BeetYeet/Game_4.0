using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fighting;

public class GunPickup : MonoBehaviour
{

    public List<Drop> drops = new List<Drop>();

    [SerializeField]
    private GameObject pickupText = null;

    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        Drop drop = Get();
        Inventory inv = PlayerController.player.GetComponent<Inventory>();
        if (inv.arsenal.Contains(drop.weapon))
        {
            drop.weapon.ammo += drop.ammo;
            Instantiate(pickupText).GetComponent<AmmoRise>().SetText($"+ammo: {drop.ammo}\n({drop.weapon.name})");
        }
        else
        {
            if (Random.value <= drop.dropChance)
            {
                inv.arsenal.Add(drop.weapon);
                drop.weapon.ammo = drop.weapon.baseAmmo + drop.ammo;
                Instantiate(pickupText).GetComponent<AmmoRise>().SetText($"+{drop.weapon.name}\n(ammo: {drop.weapon.ammo})");
            }
        }
    }

    private Drop Get()
    {
        Drop highestDrop = null;
        float highestNum = -1f;

        for (int i = 0; i < drops.Count; i++)
        {
            float newNum = drops[i].relativeChance * Random.value;
            if (newNum > highestNum)
            {
                highestNum = newNum;
                highestDrop = drops[i];
            }
        }

        return highestDrop.Get();
    }
}

[System.Serializable]
public class Drop
{
    public string name { get { if (weapon != null) return weapon.name; return "No Weapon"; } }
    public WeaponInfo weapon = null;
    public int minAmmo = 20;
    public int maxAmmo = 40;

    public float dropChance = .1f;

    [HideInInspector]
    public int ammo;

    public Drop Get()
    {
        ammo = Random.Range(minAmmo, maxAmmo);
        return this;
    }

    public float relativeChance = 1f;
}