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
                    ""name"": ""Mouse Position"",
                    ""type"": ""Value"",
                    ""id"": ""fd9892de-5789-4223-914c-f0df68645461"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""5062e6e8-5b71-497f-a235-f33a2162389a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
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
                },
                {
                    ""name"": ""Fishing"",
                    ""type"": ""Button"",
                    ""id"": ""b5b4f0af-5de7-4ad0-982e-bab1d0685db7"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""JumpAction"",
                    ""type"": ""Button"",
                    ""id"": ""f85f16af-e833-4a81-9f51-31618fecb3d1"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""1e142d78-0024-419c-8b0f-2279feefc4f3"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SwitchWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""36758d1c-4e56-4854-ab48-ae56dcf65f5f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
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
                    ""id"": ""0b490b19-3ede-45d5-80e8-c9657a8e5d78"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard Config"",
                    ""action"": ""Mouse Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
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
                    ""name"": """",
                    ""id"": ""f76f5ef8-4f23-4446-a73f-93950ebb29fa"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard Config"",
                    ""action"": ""Interact"",
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
                    ""path"": ""none"",
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
                },
                {
                    ""name"": """",
                    ""id"": ""645edc6c-eb00-4b9e-95a7-5f54fef4b760"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fishing"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""52098013-c4ad-4dd8-b18f-608a931824f2"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""JumpAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""95c09b18-20bb-47ac-b047-18268ba36c8e"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""fb8ddd35-8a4c-4fcd-b043-da381adaf240"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchWeapon"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""92fa1186-7523-4076-a0c8-070a11cc526f"",
                    ""path"": ""<Mouse>/scroll/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""6f64a38f-dda7-4d06-85fd-2889c4a833de"",
                    ""path"": ""<Mouse>/scroll/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""98706d55-b693-4de2-a31f-442837636714"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchWeapon"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""500cfb83-4cde-43c3-9916-1aa473be8fff"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""5d1d93e9-dec9-43c7-b74f-476ad96ad6b6"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchWeapon"",
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
        m_Player_MousePosition = m_Player.FindAction("Mouse Position", throwIfNotFound: true);
        m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
        m_Player_FireWeapon = m_Player.FindAction("FireWeapon", throwIfNotFound: true);
        m_Player_MoveArena = m_Player.FindAction("MoveArena", throwIfNotFound: true);
        m_Player_Fishing = m_Player.FindAction("Fishing", throwIfNotFound: true);
        m_Player_JumpAction = m_Player.FindAction("JumpAction", throwIfNotFound: true);
        m_Player_Dash = m_Player.FindAction("Dash", throwIfNotFound: true);
        m_Player_SwitchWeapon = m_Player.FindAction("SwitchWeapon", throwIfNotFound: true);
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
    private readonly InputAction m_Player_MousePosition;
    private readonly InputAction m_Player_Interact;
    private readonly InputAction m_Player_FireWeapon;
    private readonly InputAction m_Player_MoveArena;
    private readonly InputAction m_Player_Fishing;
    private readonly InputAction m_Player_JumpAction;
    private readonly InputAction m_Player_Dash;
    private readonly InputAction m_Player_SwitchWeapon;
    public struct PlayerActions
    {
        private @ActionControls m_Wrapper;
        public PlayerActions(@ActionControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveTopDown => m_Wrapper.m_Player_MoveTopDown;
        public InputAction @MousePosition => m_Wrapper.m_Player_MousePosition;
        public InputAction @Interact => m_Wrapper.m_Player_Interact;
        public InputAction @FireWeapon => m_Wrapper.m_Player_FireWeapon;
        public InputAction @MoveArena => m_Wrapper.m_Player_MoveArena;
        public InputAction @Fishing => m_Wrapper.m_Player_Fishing;
        public InputAction @JumpAction => m_Wrapper.m_Player_JumpAction;
        public InputAction @Dash => m_Wrapper.m_Player_Dash;
        public InputAction @SwitchWeapon => m_Wrapper.m_Player_SwitchWeapon;
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
            @MousePosition.started += instance.OnMousePosition;
            @MousePosition.performed += instance.OnMousePosition;
            @MousePosition.canceled += instance.OnMousePosition;
            @Interact.started += instance.OnInteract;
            @Interact.performed += instance.OnInteract;
            @Interact.canceled += instance.OnInteract;
            @FireWeapon.started += instance.OnFireWeapon;
            @FireWeapon.performed += instance.OnFireWeapon;
            @FireWeapon.canceled += instance.OnFireWeapon;
            @MoveArena.started += instance.OnMoveArena;
            @MoveArena.performed += instance.OnMoveArena;
            @MoveArena.canceled += instance.OnMoveArena;
            @Fishing.started += instance.OnFishing;
            @Fishing.performed += instance.OnFishing;
            @Fishing.canceled += instance.OnFishing;
            @JumpAction.started += instance.OnJumpAction;
            @JumpAction.performed += instance.OnJumpAction;
            @JumpAction.canceled += instance.OnJumpAction;
            @Dash.started += instance.OnDash;
            @Dash.performed += instance.OnDash;
            @Dash.canceled += instance.OnDash;
            @SwitchWeapon.started += instance.OnSwitchWeapon;
            @SwitchWeapon.performed += instance.OnSwitchWeapon;
            @SwitchWeapon.canceled += instance.OnSwitchWeapon;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @MoveTopDown.started -= instance.OnMoveTopDown;
            @MoveTopDown.performed -= instance.OnMoveTopDown;
            @MoveTopDown.canceled -= instance.OnMoveTopDown;
            @MousePosition.started -= instance.OnMousePosition;
            @MousePosition.performed -= instance.OnMousePosition;
            @MousePosition.canceled -= instance.OnMousePosition;
            @Interact.started -= instance.OnInteract;
            @Interact.performed -= instance.OnInteract;
            @Interact.canceled -= instance.OnInteract;
            @FireWeapon.started -= instance.OnFireWeapon;
            @FireWeapon.performed -= instance.OnFireWeapon;
            @FireWeapon.canceled -= instance.OnFireWeapon;
            @MoveArena.started -= instance.OnMoveArena;
            @MoveArena.performed -= instance.OnMoveArena;
            @MoveArena.canceled -= instance.OnMoveArena;
            @Fishing.started -= instance.OnFishing;
            @Fishing.performed -= instance.OnFishing;
            @Fishing.canceled -= instance.OnFishing;
            @JumpAction.started -= instance.OnJumpAction;
            @JumpAction.performed -= instance.OnJumpAction;
            @JumpAction.canceled -= instance.OnJumpAction;
            @Dash.started -= instance.OnDash;
            @Dash.performed -= instance.OnDash;
            @Dash.canceled -= instance.OnDash;
            @SwitchWeapon.started -= instance.OnSwitchWeapon;
            @SwitchWeapon.performed -= instance.OnSwitchWeapon;
            @SwitchWeapon.canceled -= instance.OnSwitchWeapon;
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
        void OnMousePosition(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnFireWeapon(InputAction.CallbackContext context);
        void OnMoveArena(InputAction.CallbackContext context);
        void OnFishing(InputAction.CallbackContext context);
        void OnJumpAction(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnSwitchWeapon(InputAction.CallbackContext context);
    }
}
