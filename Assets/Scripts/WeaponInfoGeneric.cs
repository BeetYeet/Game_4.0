using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fighting
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon/Generic")]
    public class WeaponInfoGeneric : WeaponInfo
    {
        public float bulletVelocity;
        public float firerateVariance = 0.1f;

        internal override void CalculateCooldown()
        {
            currentCooldown = baseWeaponCooldown * (1 + Random.Range(-firerateVariance, firerateVariance)); ;
        }

        internal override void Fire(float overshotTime)
        {
            if (firepoint == null)
            {
                Debug.LogError($"Weaponinfo lacks Firepoint, Please add one! Weaponinfo: { name }");
                return;
            }

            if (bullet == null)
            {
                Debug.LogError($"No bullet prefab, Please add one! prefab: {name}");
                return;
            }

            GameObject b = Instantiate(bullet, firepoint.position, firepoint.rotation);
            b.GetComponent<Bullet>().firedTime = Time.time - overshotTime;
            Rigidbody rigid = b.GetComponent<Rigidbody>();
            if (rigid != null)
            {
                rigid.velocity = firepoint.forward * bulletVelocity;
                b.transform.position += rigid.velocity * overshotTime;
            }
            else
            {
                Debug.LogError($"Bullet prefab lacks Rigidbody, Please add one! prefab: {bullet.name}");
            }
        }

        internal override void Fire()
        {
            Fire(0f);
        }
    }
}