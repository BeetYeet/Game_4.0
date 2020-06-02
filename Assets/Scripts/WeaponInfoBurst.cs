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

        public int burstLength = 4;
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

        // de-holster / re-holster
        public override void SoftReset()
        {
            base.SoftReset();
            burstNum = -1;
        }

        public override void Update()
        {

            if (CheckAmmo() && (isShooting || burstNum != 0))
            {
                // wants to shoot
                if (drawTimeLeft <= 0f)
                {
                    // weapon is drawn
                    if (currentCooldown <= 0.0001f)
                    {
                        Debug.LogError("Way too low cooldown, big no-no!");
                        return;
                    }
                    while (fireTime > currentCooldown)
                    {
                        // weapon ready
                        fireTime -= currentCooldown;
                        if (highSpeed)
                            HandleFire(0f);
                        else
                            HandleFire(fireTime);
                    }
                }
                else
                {
                    // player is trying to draw the weapon
                    drawTimeLeft -= Time.deltaTime;
                }
            }
            else
            {
                Holster();
            }
            fireTime += Time.deltaTime;
            if (burstNum == 0)
                if ((!isShooting || drawTimeLeft > 0) && fireTime > currentCooldown)
                    fireTime = currentCooldown;
        }

        internal override Quaternion GetInaccurateRotation()
        {
            return firepoint.rotation * Quaternion.Euler(Mathf.Clamp(Random.Range(-inaccuracyAngle, inaccuracyAngle) / 3f, -10f, 10f), Random.Range(-inaccuracyAngle, inaccuracyAngle) * (1 + 3 * burstNum / (float)burstLength), 0f);
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