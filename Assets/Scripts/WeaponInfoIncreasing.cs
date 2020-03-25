using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fighting
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon/Increasing")]
    public class WeaponInfoIncreasing : WeaponInfo
    {
        public float bulletVelocity;

        [Range(0f, 1f)]
        public float firerateVariance = 0.1f;

        [Range(0.01f, 2f)]
        public float minCooldownFactor = 0.3f;

        [Range(0.001f, 1f)]
        public float bulletContribution = 0.05f;

        [Range(0f, 20f)]
        public float cooldownDecayTime = 3f;

        [Range(1f, 10f)]
        public float bulletContributionCurvature = 3f;

        internal float currentCooldownProgress = 0f;

        internal override void CalculateCooldown()
        {
            float cooldown = baseWeaponCooldown * (1 + Random.Range(-firerateVariance, firerateVariance));
            currentCooldownProgress += bulletContribution * Mathf.Clamp01(1f / currentCooldownProgress);
            if (currentCooldownProgress > 1f)
                currentCooldownProgress = 1f;

            cooldown = Mathf.Lerp(cooldown, cooldown * minCooldownFactor, Mathf.Pow(currentCooldownProgress, 1f / bulletContributionCurvature));

            Debug.Log($"Heat: {currentCooldownProgress}\nCurrently at a firerate of: {1f / cooldown}");
            currentCooldown = cooldown;
        }

        internal override void ResetUpdate()
        {
            base.ResetUpdate();
            currentCooldownProgress -= Time.deltaTime / cooldownDecayTime;
            if (currentCooldownProgress < 0f)
                currentCooldownProgress = 0f;
            Debug.Log($"Heat: {currentCooldownProgress}");
        }

        public override void Reset()
        {
            base.Reset();
            currentCooldownProgress = 0f;
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
            b.GetComponent<Bullet>().firedTime = Time.time - overshotTime;
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