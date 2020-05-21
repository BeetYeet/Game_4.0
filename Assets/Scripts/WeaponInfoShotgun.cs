using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fighting
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon/Shotgun")]
    public class WeaponInfoShotgun : WeaponInfo
    {
        public float bulletVelocity = 30f;
        public float bulletVelocityVariance = 10f;
        public float firerateVariance = 0.1f;
        public int pelletCount = 15;
        public int pelletVariance = 3;
        public float damageVariance = 1f;

        private int count = 0;

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
            count = pelletCount + Random.Range(-pelletVariance, pelletVariance);
            for (int i = 0; i < count; i++)
            {
                GameObject b = Instantiate(bullet, firepoint.position, GetInaccurateRotation());
                AssignValuesToBullet(overshotTime, b);
                Rigidbody rigid = b.GetComponent<Rigidbody>();
                if (rigid != null)
                {
                    rigid.velocity = b.transform.forward * (bulletVelocity + Random.Range(-bulletVelocityVariance, bulletVelocityVariance));
                    b.transform.position += rigid.velocity * overshotTime;
                }
                else
                {
                    Debug.LogError($"Bullet prefab lacks Rigidbody, Please add one! prefab: {bullet.name}");
                }
            }
        }

        internal override void AssignValuesToBullet(float overshotTime, GameObject bullet)
        {
            base.AssignValuesToBullet(overshotTime, bullet);
            bullet.GetComponent<Bullet>().damage = (damage + Random.Range(-damageVariance, damageVariance)) / count;
        }

        internal override void Fire()
        {
            Fire(0f);
        }
    }
}