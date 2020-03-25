using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fighting;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public List<WeaponInfo> arsenal = new List<WeaponInfo>();

    public WeaponHandler weaponHandler;

    private MyInputSystem input;

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
        input = new MyInputSystem();
        input.PlayerActionControlls.CycleWeaponRight.started += OnCycleWeaponRight;
        input.PlayerActionControlls.CycleWeaponLeft.started += OnCycleWeaponLeft;
    }

    private void Update()
    {
    }

    public void OnCycleWeaponRight(InputAction.CallbackContext ctx)
    {
        if (arsenal.Count <= 1) return;

        WeaponInfo weapon = arsenal[0];
        arsenal.Remove(weapon);
        arsenal.Add(weapon);
        weaponHandler.SetWeapon(arsenal[0]);
    }

    public void OnCycleWeaponLeft(InputAction.CallbackContext ctx)
    {
        if (arsenal.Count <= 1) return;

        WeaponInfo weapon = arsenal[arsenal.Count - 1];
        arsenal.Remove(weapon);
        arsenal.Insert(0, weapon);
        weaponHandler.SetWeapon(weapon);
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