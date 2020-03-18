using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fighting
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon/Generic")]
    public class WeaponInfoGeneric : WeaponInfo
    {
        public float bulletVelocity;

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
            Rigidbody rigid = b.GetComponent<Rigidbody>();
            if (rigid != null)
            {
                rigid.AddForce(firepoint.forward * bulletVelocity, ForceMode.VelocityChange);
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