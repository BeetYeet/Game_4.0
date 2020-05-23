using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fighting
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon/Burst")]
    public class WeaponInfoBurst : WeaponInfo
    {
        public float bulletVelocity;
        public float firerateVariance = 0.1f;

        public int burstLength = 3;
        private int burstNum;
        public float burstDelay = 1f;

        internal override void CalculateCooldown()
        {
            if (burstNum >= burstLength)
            {
                currentCooldown = burstDelay;
                burstNum = 0;
            }
            else
            {
                currentCooldown = baseWeaponCooldown * (1 + Random.Range(-firerateVariance, firerateVariance)); ;
                burstNum++;
            }
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

            GameObject b = Instantiate(bullet, firepoint.position, GetInaccurateRotation());
            AssignValuesToBullet(overshotTime, b);
            Rigidbody rigid = b.GetComponent<Rigidbody>();
            if (rigid != null)
            {
                rigid.velocity = b.transform.forward * bulletVelocity;
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