// GENERATED AUTOMATICALLY FROM 'Assets/MyInputSystem.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @MyInputSystem : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @MyInputSystem()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MyInputSystem"",
    ""maps"": [
        {
            ""name"": ""Player Action Controlls"",
            ""id"": ""1108bab9-78f9-4e1a-967b-95212395cd89"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d9d919f4-ff1f-4c72-bdde-d23467683cbb"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot Direction"",
                    ""type"": ""PassThrough"",
                    ""id"": ""1d1603be-e765-49b7-a06b-977072b04bfc"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cycle Weapon Right"",
                    ""type"": ""Button"",
                    ""id"": ""4fa35d7d-0c48-4d74-8ffa-9c7a10963ca4"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cycle Weapon Left"",
                    ""type"": ""Button"",
                    ""id"": ""2e6e1561-f728-4e9a-8d05-88d25a0c7f5c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Heal"",
                    ""type"": ""Button"",
                    ""id"": ""a7f691c4-dc06-4fe2-a9bf-e432d567d4f1"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TakeDamage"",
                    ""type"": ""Button"",
                    ""id"": ""42f9ffca-bf08-4f02-8277-eb7abe935bfa"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2ddea231-1688-4d2b-ad4b-6d2576ca08e3"",
                    ""path"": ""<XInputController>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": ""Controller"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""21a4a2e8-1183-4a5b-9df3-31d895ef4ffe"",
                    ""path"": ""<XInputController>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": ""Controller"",
                    ""action"": ""Shoot Direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Arrow Keys"",
                    ""id"": ""b135a7f0-b59a-48ba-a316-e14f01e922e8"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Shoot Direction"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6799ffc6-6294-4e0f-b08e-8a0e2a698828"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Shoot Direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ec800468-e02f-4ca4-93bc-96d872238f54"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Shoot Direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""9f1bb797-13de-4923-9112-3ea2cec5b2fb"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Shoot Direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""aaea2778-7912-4831-8278-aadf89204353"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Shoot Direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""255500b4-527a-43ae-bbac-6b830e7d25d3"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""fa934e8b-ea15-4e53-bab0-9e9aa07b54d4"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""8c7a1fee-39c1-48c7-9197-37826c4eb4c8"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a8296f6d-2b22-4f32-9352-5a2827dc3674"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""6ff3cac5-8922-4d24-b603-41f7dbdf8b29"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f2b2cc65-5d1e-480d-998f-476ade576526"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Cycle Weapon Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4aed3b71-090d-419f-ab8c-e6b736cadae1"",
                    ""path"": ""<XInputController>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Cycle Weapon Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""45f21f77-ea6f-494c-949b-0e8653692230"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Cycle Weapon Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cfabb24b-6f7a-474b-9487-a6804acd9bdc"",
                    ""path"": ""<XInputController>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Cycle Weapon Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""978735bd-7a0e-4acd-8efb-9af67b73c39f"",
                    ""path"": ""<Keyboard>/numpadPlus"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Heal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6336bf36-4315-45ee-9171-cb01ab34d105"",
                    ""path"": ""<Keyboard>/numpadMinus"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""TakeDamage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Controller"",
            ""bindingGroup"": ""Controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<XInputController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player Action Controlls
        m_PlayerActionControlls = asset.FindActionMap("Player Action Controlls", throwIfNotFound: true);
        m_PlayerActionControlls_Move = m_PlayerActionControlls.FindAction("Move", throwIfNotFound: true);
        m_PlayerActionControlls_ShootDirection = m_PlayerActionControlls.FindAction("Shoot Direction", throwIfNotFound: true);
        m_PlayerActionControlls_CycleWeaponRight = m_PlayerActionControlls.FindAction("Cycle Weapon Right", throwIfNotFound: true);
        m_PlayerActionControlls_CycleWeaponLeft = m_PlayerActionControlls.FindAction("Cycle Weapon Left", throwIfNotFound: true);
        m_PlayerActionControlls_Heal = m_PlayerActionControlls.FindAction("Heal", throwIfNotFound: true);
        m_PlayerActionControlls_TakeDamage = m_PlayerActionControlls.FindAction("TakeDamage", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player Action Controlls
    private readonly InputActionMap m_PlayerActionControlls;
    private IPlayerActionControllsActions m_PlayerActionControllsActionsCallbackInterface;
    private readonly InputAction m_PlayerActionControlls_Move;
    private readonly InputAction m_PlayerActionControlls_ShootDirection;
    private readonly InputAction m_PlayerActionControlls_CycleWeaponRight;
    private readonly InputAction m_PlayerActionControlls_CycleWeaponLeft;
    private readonly InputAction m_PlayerActionControlls_Heal;
    private readonly InputAction m_PlayerActionControlls_TakeDamage;
    public struct PlayerActionControllsActions
    {
        private @MyInputSystem m_Wrapper;
        public PlayerActionControllsActions(@MyInputSystem wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerActionControlls_Move;
        public InputAction @ShootDirection => m_Wrapper.m_PlayerActionControlls_ShootDirection;
        public InputAction @CycleWeaponRight => m_Wrapper.m_PlayerActionControlls_CycleWeaponRight;
        public InputAction @CycleWeaponLeft => m_Wrapper.m_PlayerActionControlls_CycleWeaponLeft;
        public InputAction @Heal => m_Wrapper.m_PlayerActionControlls_Heal;
        public InputAction @TakeDamage => m_Wrapper.m_PlayerActionControlls_TakeDamage;
        public InputActionMap Get() { return m_Wrapper.m_PlayerActionControlls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActionControllsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActionControllsActions instance)
        {
            if (m_Wrapper.m_PlayerActionControllsActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerActionControllsActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerActionControllsActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerActionControllsActionsCallbackInterface.OnMove;
                @ShootDirection.started -= m_Wrapper.m_PlayerActionControllsActionsCallbackInterface.OnShootDirection;
                @ShootDirection.performed -= m_Wrapper.m_PlayerActionControllsActionsCallbackInterface.OnShootDirection;
                @ShootDirection.canceled -= m_Wrapper.m_PlayerActionControllsActionsCallbackInterface.OnShootDirection;
                @CycleWeaponRight.started -= m_Wrapper.m_PlayerActionControllsActionsCallbackInterface.OnCycleWeaponRight;
                @CycleWeaponRight.performed -= m_Wrapper.m_PlayerActionControllsActionsCallbackInterface.OnCycleWeaponRight;
                @CycleWeaponRight.canceled -= m_Wrapper.m_PlayerActionControllsActionsCallbackInterface.OnCycleWeaponRight;
                @CycleWeaponLeft.started -= m_Wrapper.m_PlayerActionControllsActionsCallbackInterface.OnCycleWeaponLeft;
                @CycleWeaponLeft.performed -= m_Wrapper.m_PlayerActionControllsActionsCallbackInterface.OnCycleWeaponLeft;
                @CycleWeaponLeft.canceled -= m_Wrapper.m_PlayerActionControllsActionsCallbackInterface.OnCycleWeaponLeft;
                @Heal.started -= m_Wrapper.m_PlayerActionControllsActionsCallbackInterface.OnHeal;
                @Heal.performed -= m_Wrapper.m_PlayerActionControllsActionsCallbackInterface.OnHeal;
                @Heal.canceled -= m_Wrapper.m_PlayerActionControllsActionsCallbackInterface.OnHeal;
                @TakeDamage.started -= m_Wrapper.m_PlayerActionControllsActionsCallbackInterface.OnTakeDamage;
                @TakeDamage.performed -= m_Wrapper.m_PlayerActionControllsActionsCallbackInterface.OnTakeDamage;
                @TakeDamage.canceled -= m_Wrapper.m_PlayerActionControllsActionsCallbackInterface.OnTakeDamage;
            }
            m_Wrapper.m_PlayerActionControllsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @ShootDirection.started += instance.OnShootDirection;
                @ShootDirection.performed += instance.OnShootDirection;
                @ShootDirection.canceled += instance.OnShootDirection;
                @CycleWeaponRight.started += instance.OnCycleWeaponRight;
                @CycleWeaponRight.performed += instance.OnCycleWeaponRight;
                @CycleWeaponRight.canceled += instance.OnCycleWeaponRight;
                @CycleWeaponLeft.started += instance.OnCycleWeaponLeft;
                @CycleWeaponLeft.performed += instance.OnCycleWeaponLeft;
                @CycleWeaponLeft.canceled += instance.OnCycleWeaponLeft;
                @Heal.started += instance.OnHeal;
                @Heal.performed += instance.OnHeal;
                @Heal.canceled += instance.OnHeal;
                @TakeDamage.started += instance.OnTakeDamage;
                @TakeDamage.performed += instance.OnTakeDamage;
                @TakeDamage.canceled += instance.OnTakeDamage;
            }
        }
    }
    public PlayerActionControllsActions @PlayerActionControlls => new PlayerActionControllsActions(this);
    private int m_ControllerSchemeIndex = -1;
    public InputControlScheme ControllerScheme
    {
        get
        {
            if (m_ControllerSchemeIndex == -1) m_ControllerSchemeIndex = asset.FindControlSchemeIndex("Controller");
            return asset.controlSchemes[m_ControllerSchemeIndex];
        }
    }
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    public interface IPlayerActionControllsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnShootDirection(InputAction.CallbackContext context);
        void OnCycleWeaponRight(InputAction.CallbackContext context);
        void OnCycleWeaponLeft(InputAction.CallbackContext context);
        void OnHeal(InputAction.CallbackContext context);
        void OnTakeDamage(InputAction.CallbackContext context);
    }
}
