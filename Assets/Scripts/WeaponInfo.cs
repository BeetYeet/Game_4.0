using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fighting
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon/Dummy")]
    public abstract class WeaponInfo : ScriptableObject
    {
        [HideInInspector]
        public bool isShooting;

        public float nextWeaponCooldown;

        internal Transform firepoint;
        public GameObject bullet;
        public float timeUntillNextShot { get; internal set; }

        public event System.Action OnFire;

        public virtual void Update()
        {
            if (timeUntillNextShot > 0f)
                timeUntillNextShot -= Time.deltaTime;
            if (timeUntillNextShot <= 0f)
            {
                timeUntillNextShot = 0f;
                if (isShooting)
                {
                    OnFire?.Invoke();
                    Fire();
                    timeUntillNextShot = nextWeaponCooldown;
                }
            }
        }

        public void SetFirepoint(Transform firepoint)
        {
            this.firepoint = firepoint;
        }

        internal abstract void Fire();
    }
}