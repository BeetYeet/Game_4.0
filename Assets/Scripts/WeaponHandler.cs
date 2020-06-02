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
        public ParticleSystem casingParticles;

        public float minCasingDelay = .8f, maxCasingDelay = 1.2f;

        private void Start()
        {
            weapon.Initialize(firepoint);
            if (lightAnim == null)
            {
                Debug.LogError($"Unassigned LightAnimation script!\nat {gameObject.name}");
                enabled = false;
            }
            weapon.OnFire += OnFire;

            SceneHandler.instance.transition.OnTransitionOutEnd += CleanUp;
        }

        private void OnFire()
        {
            if (particles != null)
                particles.Play();
            if (lightAnim != null)
                lightAnim.Trigger();
            StartCoroutine(Casing(AudioExtras.NormalRandom(minCasingDelay, maxCasingDelay, 0f, 10f), weapon));
            casingParticles.Play();
        }

        private IEnumerator Casing(float delay, WeaponInfo weapon)
        {
            yield return new WaitForSeconds(delay * weapon.casingDelayFactor);
            weapon.CasingSound();
        }

        private void CleanUp()
        {
            weapon.OnFire -= OnFire;
            weapon.HardReset();
            SceneHandler.instance.transition.OnTransitionOutEnd -= CleanUp;
        }

        ~WeaponHandler()
        {
            CleanUp();
        }

        private void LateUpdate()
        {
            weapon.isShooting = isShooting;
            if (weapon.firepoint == null)
                weapon.firepoint = firepoint;
            weapon.Update();
        }

        public void SetWeapon(WeaponInfo newWeapon)
        {
            weapon.SoftReset();
            weapon.OnFire -= OnFire;
            weapon = newWeapon;
            newWeapon.Initialize(firepoint);
            newWeapon.OnFire += OnFire;
        }
    }
}