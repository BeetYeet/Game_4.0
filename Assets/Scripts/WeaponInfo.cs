using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fighting
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon/Dummy")]
    public abstract class WeaponInfo : ScriptableObject
    {
        public float baseWeaponCooldown;
        public float weaponDrawTime = .2f;
        public GameObject bullet;

        [Range(0f, 90f)]
        public float inaccuracyAngle = .1f;

        public float damage = 5f;
        public float armorPenetration = 1f;
        public bool kinetic = false;

        // Internal things
        [HideInInspector]
        public bool isShooting;

        internal float drawTimeLeft = 0f;

        internal float fireTime = 0f;
        internal Transform firepoint;

        public event System.Action OnFire;

        /// <summary>
        /// Calculates the next cooldown of the gun, Do NOT call this multiple times for one bullet, instead look at the value of currentCooldown
        /// </summary>
        internal virtual void CalculateCooldown()
        {
            float cooldown = baseWeaponCooldown;
            currentCooldown = cooldown;
        }

        public virtual void Reset()
        {
            firepoint = null;
            currentCooldown = baseWeaponCooldown;
        }

        internal virtual Quaternion GetInaccurateRotation()
        {
            return firepoint.rotation * Quaternion.Euler(Mathf.Clamp(Random.Range(-inaccuracyAngle, inaccuracyAngle) / 3f, -10f, 10f), Random.Range(-inaccuracyAngle, inaccuracyAngle), 0f);
        }

        /// <summary>
        /// the amount of cooldown time required to fire again
        /// </summary>
        internal float currentCooldown;

        /// <summary>
        /// Initialization for the script
        /// </summary>
        /// <param name="firepoint">The point in world space to spawn the bullets at</param>
        public virtual void Initialize(Transform firepoint)
        {
            Reset();
            this.firepoint = firepoint;
            CalculateCooldown();
        }

        public virtual void Update()
        {
            fireTime += Time.deltaTime;
            if (!isShooting && fireTime > currentCooldown)
                fireTime = currentCooldown;

            if (isShooting)
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
                ResetUpdate();
            }
        }

        internal virtual void ResetUpdate()
        {
            // weapon should not be drawn, holster it
            drawTimeLeft = weaponDrawTime;
        }

        internal void HandleFire()
        {
            HandleFire(0);
        }

        internal void HandleFire(float overshotTime)
        {
            OnFire?.Invoke();
            Fire(overshotTime);
            CalculateCooldown();
        }

        internal abstract void Fire();

        internal abstract void Fire(float overshotTime);

        internal virtual void AssignValuesToBullet(float overshotTime, GameObject bullet)
        {
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.firedTime = Time.time - overshotTime;
            bulletScript.damage = damage;
            bulletScript.armorPenetration = armorPenetration;
            bulletScript.kinetic = kinetic;
        }
    }
}