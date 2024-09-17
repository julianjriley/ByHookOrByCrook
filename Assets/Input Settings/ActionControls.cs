//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.11.0
//     from Assets/Input Settings/ActionControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @ActionControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @ActionControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ActionControls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""8903f1e2-f788-46ec-951d-31e43e17e7ab"",
            ""actions"": [
                {
                    ""name"": ""Move Top-Down"",
                    ""type"": ""Value"",
                    ""id"": ""70e499d5-6edc-46aa-9c8b-a3cc1aa4c0df"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""FireWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""a3990f97-25d4-4c4c-9f85-7fd0d46ddb3e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveArena"",
                    ""type"": ""Value"",
                    ""id"": ""78108e5a-8f7b-4cda-acb8-53f7869502b1"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD Keys"",
                    ""id"": ""b7b27895-92f5-4b7f-9e99-87ba0398634d"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move Top-Down"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""93ca97d8-10b7-4865-89b2-1c3c71a89473"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Config"",
                    ""action"": ""Move Top-Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a346e03a-4738-4ec4-87b1-21b9c6443aa5"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Config"",
                    ""action"": ""Move Top-Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""3f529c1a-9a92-4701-84fe-1f823c8a2917"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Config"",
                    ""action"": ""Move Top-Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8098becf-fa85-486f-bdba-ed2e6eef525b"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Config"",
                    ""action"": ""Move Top-Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""cea4f83c-5bd1-43dc-a59c-067e959f375a"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FireWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD Keys"",
                    ""id"": ""8afa6fd5-04d5-4c8e-9337-0ac0e9db8bf0"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveArena"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""8cdb75c9-40f8-42a4-b76c-d99ed0897832"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveArena"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""49573d8a-e67c-4949-a73c-80ea6ea21cfb"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveArena"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e97704d6-5bf9-4aab-a74e-be6a2a24467b"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveArena"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""c9f67563-1b0e-4e13-8859-c674552e317a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveArena"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard Config"",
            ""bindingGroup"": ""Keyboard Config"",
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
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_MoveTopDown = m_Player.FindAction("Move Top-Down", throwIfNotFound: true);
        m_Player_FireWeapon = m_Player.FindAction("FireWeapon", throwIfNotFound: true);
        m_Player_MoveArena = m_Player.FindAction("MoveArena", throwIfNotFound: true);
    }

    ~@ActionControls()
    {
        UnityEngine.Debug.Assert(!m_Player.enabled, "This will cause a leak and performance issues, ActionControls.Player.Disable() has not been called.");
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_MoveTopDown;
    private readonly InputAction m_Player_FireWeapon;
    private readonly InputAction m_Player_MoveArena;
    public struct PlayerActions
    {
        private @ActionControls m_Wrapper;
        public PlayerActions(@ActionControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveTopDown => m_Wrapper.m_Player_MoveTopDown;
        public InputAction @FireWeapon => m_Wrapper.m_Player_FireWeapon;
        public InputAction @MoveArena => m_Wrapper.m_Player_MoveArena;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @MoveTopDown.started += instance.OnMoveTopDown;
            @MoveTopDown.performed += instance.OnMoveTopDown;
            @MoveTopDown.canceled += instance.OnMoveTopDown;
            @FireWeapon.started += instance.OnFireWeapon;
            @FireWeapon.performed += instance.OnFireWeapon;
            @FireWeapon.canceled += instance.OnFireWeapon;
            @MoveArena.started += instance.OnMoveArena;
            @MoveArena.performed += instance.OnMoveArena;
            @MoveArena.canceled += instance.OnMoveArena;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @MoveTopDown.started -= instance.OnMoveTopDown;
            @MoveTopDown.performed -= instance.OnMoveTopDown;
            @MoveTopDown.canceled -= instance.OnMoveTopDown;
            @FireWeapon.started -= instance.OnFireWeapon;
            @FireWeapon.performed -= instance.OnFireWeapon;
            @FireWeapon.canceled -= instance.OnFireWeapon;
            @MoveArena.started -= instance.OnMoveArena;
            @MoveArena.performed -= instance.OnMoveArena;
            @MoveArena.canceled -= instance.OnMoveArena;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_KeyboardConfigSchemeIndex = -1;
    public InputControlScheme KeyboardConfigScheme
    {
        get
        {
            if (m_KeyboardConfigSchemeIndex == -1) m_KeyboardConfigSchemeIndex = asset.FindControlSchemeIndex("Keyboard Config");
            return asset.controlSchemes[m_KeyboardConfigSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMoveTopDown(InputAction.CallbackContext context);
        void OnFireWeapon(InputAction.CallbackContext context);
        void OnMoveArena(InputAction.CallbackContext context);
    }
}
