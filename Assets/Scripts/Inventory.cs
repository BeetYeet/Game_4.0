using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fighting;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class Inventory : MonoBehaviour
{
    public List<WeaponInfo> arsenal = new List<WeaponInfo>();

    public WeaponHandler weaponHandler;

    private MyInputSystem input;

    private TextMeshProUGUI weaponNameUI;

    private void Awake()
    {
        if (weaponHandler == null)
        {
            Debug.LogError($"Unassigned WeaponHandler script!\nat {gameObject.name}");
            enabled = false;
        }
        if (arsenal == null)
        {
            Debug.LogError($"Arsenal list null!\nat {gameObject.name}");
            enabled = false;
        }
        try
        {
            weaponNameUI = GameObject.FindWithTag("Weapon Name UI").GetComponent<TextMeshProUGUI>();
        }
        catch (System.NullReferenceException _) { }

        input = new MyInputSystem();
        input.PlayerActionControlls.CycleWeaponRight.started += OnCycleWeaponRight;
        input.PlayerActionControlls.CycleWeaponLeft.started += OnCycleWeaponLeft;

        weaponHandler.weapon.OnFire += Weapon_OnFire;
        UpdateWeaponName();
    }

    private void Weapon_OnFire()
    {
        if (weaponHandler.weapon.ammo <= 0)
        {
            arsenal.Remove(weaponHandler.weapon);
        }
    }

    private void Update()
    {
        UpdateWeaponName();
    }

    ~Inventory()
    {
        weaponHandler.weapon.OnFire -= Weapon_OnFire;
    }

    public void OnCycleWeaponRight(InputAction.CallbackContext ctx)
    {
        if (arsenal.Count <= 1) return;

        weaponHandler.weapon.OnFire -= Weapon_OnFire;

        WeaponInfo weapon = arsenal[0];
        arsenal.Remove(weapon);
        arsenal.Add(weapon);
        weapon.OnFire += Weapon_OnFire;

        weaponHandler.SetWeapon(arsenal[0]);
    }

    private void UpdateWeaponName()
    {
        if (weaponNameUI == null)
            return;

        weaponNameUI.text = $"{weaponHandler.weapon.name} ({weaponHandler.weapon.ammo})";
    }

    public void OnCycleWeaponLeft(InputAction.CallbackContext ctx)
    {
        if (arsenal.Count <= 1) return;

        weaponHandler.weapon.OnFire -= Weapon_OnFire;

        WeaponInfo weapon = arsenal[arsenal.Count - 1];
        arsenal.Remove(weapon);
        arsenal.Insert(0, weapon);
        weapon.OnFire += Weapon_OnFire;

        weaponHandler.SetWeapon(weapon);
        UpdateWeaponName();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}