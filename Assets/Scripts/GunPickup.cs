using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fighting;
using Tools;

public class GunPickup : MonoBehaviour
{

    public List<Drop> drops = new List<Drop>();

    [SerializeField]
    private GameObject pickupText = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        Inventory inventory = PlayerController.player.GetComponent<Inventory>();

        List<Drop> owned = drops.ContainsWeapon(inventory.arsenal);
        List<Drop> unowned = drops.DiscontainsWeapon(inventory.arsenal);

        if (unowned.Count > 0 && Chance(.01f * unowned.Count))
        {
            // drop new weapon
            Drop drop = GetRandomWeaponFrom(unowned, true);
            NewWeapon(drop.weapon, drop.GetAmmo(), inventory);
        }
        else
        {
            // drop ammo
            if (unowned.Count == 0 || Chance(.6f))
            {
                // for owned weapon
                Drop drop = GetRandomWeaponFrom(owned, false);
                MoreAmmo(drop.weapon, drop.GetAmmo());
            }
            else
            {
                // for unowned weapon
                Drop drop = GetRandomWeaponFrom(unowned, false);
                MoreAmmo(drop.weapon, drop.GetAmmo());
            }
        }

        Destroy(gameObject);
    }

    private bool Chance(float probability)
    {
        probability = Mathf.Clamp01(probability);

        if (Random.value < probability)
            return true;
        return false;
    }

    private void MoreAmmo(WeaponInfo weapon, int amount)
    {
        weapon.ammo += amount;
        Instantiate(pickupText, transform.position + Vector3.up * 2f, Quaternion.identity).GetComponent<AmmoRise>().SetText($"+ammo: {amount}\n({weapon.name})");
    }

    private void NewWeapon(WeaponInfo weapon, int extraAmmo, Inventory inv)
    {
        inv.arsenal.Add(weapon);
        weapon.HardReset();
        weapon.ammo += extraAmmo;
        GameObject go = Instantiate(pickupText, transform.position + Vector3.up * 2f, Quaternion.identity);
        go.GetComponent<AmmoRise>().SetText($"NEW WEAPON:\n{weapon.name}\n(ammo: {weapon.ammo})");
        go.transform.localScale *= 1.5f;
    }

    private Drop GetRandomWeaponFrom(List<Drop> list, bool getWeaponDrop)
    {
        Drop highestDrop = null;
        float highestNum = -1f;
        if (getWeaponDrop)
            for (int i = 0; i < list.Count; i++)
            {
                float newNum = list[i].relativeDropChance * Random.value;
                if (newNum > highestNum)
                {
                    highestNum = newNum;
                    highestDrop = list[i];
                }
            }
        else
        {
            for (int i = 0; i < list.Count; i++)
            {
                float newNum = list[i].relativeAmmoChance * Random.value;
                if (newNum > highestNum)
                {
                    highestNum = newNum;
                    highestDrop = list[i];
                }
            }
        }
        return highestDrop;
    }
}

[System.Serializable]
public class Drop
{
    public string name { get { if (weapon != null) return weapon.name; return "No Weapon"; } }
    public WeaponInfo weapon = null;
    public int minAmmo = 20;
    public int maxAmmo = 40;

    public float relativeDropChance = .1f;
    public float relativeAmmoChance = 1f;

    [HideInInspector]
    public int ammo;

    public int GetAmmo()
    {
        return Random.Range(minAmmo, maxAmmo);
    }
}