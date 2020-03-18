using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fighting
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon/Generic")]
    public class WeaponInfoGeneric : WeaponInfo
    {
        public float bulletVelocity;

        internal override void Fire()
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
            }
            else
            {
                Debug.LogError($"Bullet prefab lacks Rigidbody, Please add one! prefab: {bullet.name}");
            }
        }
    }
}