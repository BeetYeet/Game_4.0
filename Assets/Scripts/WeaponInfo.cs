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

        // Internal things
        [HideInInspector]
        public bool isShooting;

        [HideInInspector]
        public float nextWeaponCooldown;

        internal float drawTimeLeft = 0f;

        internal float fireTime = 0f;
        internal Transform firepoint;

        public event System.Action OnFire;

        public virtual void Initialize(Transform firepoint)
        {
            this.firepoint = firepoint;
            nextWeaponCooldown = baseWeaponCooldown;
        }

        public virtual void Update()
        {
            fireTime += Time.deltaTime;
            if (isShooting)
            {
                if (drawTimeLeft == 0f)
                {
                    while (fireTime > nextWeaponCooldown)
                    {
                        float overshotTime = fireTime - nextWeaponCooldown;
                        HandleFire(overshotTime);
                        fireTime = overshotTime;
                    }
                }
                else
                {
                    drawTimeLeft -= Time.deltaTime;
                    if (drawTimeLeft < 0f)
                    {
                        fireTime -= drawTimeLeft;
                        drawTimeLeft = 0f;
                    }
                }
            }
            else
            {
                if (fireTime > nextWeaponCooldown)
                    fireTime = nextWeaponCooldown;
                drawTimeLeft = weaponDrawTime;
            }
        }

        internal void HandleFire()
        {
            HandleFire(0);
        }

        internal void HandleFire(float overshotTime)
        {
            OnFire?.Invoke();
            Fire(overshotTime);
            fireTime -= nextWeaponCooldown;
        }

        internal abstract void Fire();

        internal abstract void Fire(float overshotTime);
    }
}