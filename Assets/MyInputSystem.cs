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
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2ddea231-1688-4d2b-ad4b-6d2576ca08e3"",
                    ""path"": ""<XInputController>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
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
                    ""processors"": """",
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
                    ""groups"": ""Controller"",
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
                    ""groups"": ""Controller"",
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
                    ""groups"": ""Controller"",
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
                    ""groups"": ""Controller"",
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
                    ""groups"": ""Controller"",
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
                    ""groups"": ""Controller"",
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
                    ""groups"": ""Controller"",
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
                    ""groups"": ""Controller"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
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
        }
    ]
}");
        // Player Action Controlls
        m_PlayerActionControlls = asset.FindActionMap("Player Action Controlls", throwIfNotFound: true);
        m_PlayerActionControlls_Move = m_PlayerActionControlls.FindAction("Move", throwIfNotFound: true);
        m_PlayerActionControlls_ShootDirection = m_PlayerActionControlls.FindAction("Shoot Direction", throwIfNotFound: true);
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
    public struct PlayerActionControllsActions
    {
        private @MyInputSystem m_Wrapper;
        public PlayerActionControllsActions(@MyInputSystem wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerActionControlls_Move;
        public InputAction @ShootDirection => m_Wrapper.m_PlayerActionControlls_ShootDirection;
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
    public interface IPlayerActionControllsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnShootDirection(InputAction.CallbackContext context);
    }
}
