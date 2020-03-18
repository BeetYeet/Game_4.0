using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fighting
{
    public class WeaponHandler : MonoBehaviour
    {
        public WeaponInfo weapon;
        public Transform firepoint;
        public bool isShooting = false;
        public LightAnimation lightAnim;
        public ParticleSystem particles;

        private void Start()
        {
            weapon.SetFirepoint(firepoint);
            if (lightAnim == null)
            {
                Debug.LogError($"Unassigned LightAnimation script!\nat {gameObject.name}");
                enabled = false;
            }
            weapon.OnFire += lightAnim.Trigger;
            weapon.OnFire += particles.Play;
        }

        private void LateUpdate()
        {
            weapon.isShooting = isShooting;
            weapon.Update();
        }
    }
}