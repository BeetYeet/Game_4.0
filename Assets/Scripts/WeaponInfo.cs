using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

namespace Fighting
{
    public abstract class WeaponInfo : ScriptableObject
    {
        public int baseAmmo = 20;
        public int ammo = 0;

        public float baseWeaponCooldown;
        public float weaponDrawTime = .2f;
        public GameObject bullet;

        [Range(0f, 90f)]
        public float inaccuracyAngle = .1f;

        public float damage = 5f;
        public float armorPenetration = 1f;
        public bool kinetic = false;
        public bool highSpeed = false;

        public List<string> shootSounds = new List<string>();
        public AudioHandler.AudioContext audioContext = AudioHandler.AudioContext.empty;

        public string casingDenominator = "Casing ";
        public AudioHandler.AudioContext casingAudioContext = AudioHandler.AudioContext.empty;

        [Range(0f, 2f)]
        public float casingDelayFactor = 1f;

        // Internal things
        [HideInInspector]
        public bool usable = true;

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

        // New Run
        public virtual void HardReset()
        {
            ammo = baseAmmo;
            firepoint = null;
            SoftReset();
        }

        // de-holster / re-holster
        public virtual void SoftReset()
        {
            usable = true;
            currentCooldown = baseWeaponCooldown;
            fireTime = currentCooldown;
            isShooting = false;
        }

        public virtual void CasingSound()
        {
            if (firepoint == null)
                return;
            AudioHandler.AudioContext adjustedContext = casingAudioContext;
            adjustedContext.volume *= SettingsHandler.cache.volume_gun * SettingsHandler.cache.volume_master;
            AudioHandler.PlaySound(casingDenominator + Random.Range(1, 21).ToString("D2"), adjustedContext, firepoint.position.Mult(new Vector3(1f, 0f, 1f)));
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
            SoftReset();
            this.firepoint = firepoint;
            CalculateCooldown();
        }

        public virtual void Update()
        {
            if (isShooting && CheckAmmo())
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
            if ((!isShooting || drawTimeLeft > 0) && fireTime > currentCooldown)
                fireTime = currentCooldown;
        }

        internal virtual bool CheckAmmo()
        {
            if (ammo > 0)
            {
                usable = true;
                return true;
            }
            usable = false;
            return false;
        }

        internal virtual void Holster()
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
            Fire(overshotTime);
            ammo--;
            if (shootSounds.Count > 0)
            {
                AudioHandler.AudioContext adjustedContext = audioContext;
                adjustedContext.volume *= SettingsHandler.cache.volume_gun * SettingsHandler.cache.volume_master;
                AudioHandler.PlaySound(shootSounds[Random.Range(0, shootSounds.Count - 1)], adjustedContext, firepoint.position);
            }
            OnFire?.Invoke();
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