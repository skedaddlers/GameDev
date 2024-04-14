//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Scripts/Entity/Controls.inputactions
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

public partial class @Controls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""533e0798-be52-406f-b4d0-faaa40582f58"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""916651e3-704b-4c8d-8b85-83feb634d8ae"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""NormalizeVector2"",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Exit"",
                    ""type"": ""Button"",
                    ""id"": ""16638385-a186-47b7-b06e-f3baa8a8482c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""View"",
                    ""type"": ""Button"",
                    ""id"": ""91a378a4-1a59-4f9e-821a-60004503698f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""2b369b1e-791a-452e-bca5-9d7bc6682958"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pickup"",
                    ""type"": ""Button"",
                    ""id"": ""638871e0-c2d3-43e7-9d67-cb0e9017ea87"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Drop"",
                    ""type"": ""Button"",
                    ""id"": ""eb70060b-ea18-41dd-9c58-7a8915075c13"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""048f3745-f839-429a-b3df-28c5b401e9d5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""DropWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""6b8b60d9-99a2-4174-8525-1142ca0978dd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""16d01a59-99ed-495a-a4df-10522335202a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Info"",
                    ""type"": ""Button"",
                    ""id"": ""3e3e8508-8170-4e7a-b7d2-7dfba77675cd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Skill1"",
                    ""type"": ""Button"",
                    ""id"": ""1a1c8a1f-759a-4740-82b3-9cb5567666c5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Skill2"",
                    ""type"": ""Button"",
                    ""id"": ""0508c35d-2bf3-4d1e-b1e3-db37c8cff202"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Skill3"",
                    ""type"": ""Button"",
                    ""id"": ""1a4c17a5-e468-4451-b67c-390b806fbe5d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Skill4"",
                    ""type"": ""Button"",
                    ""id"": ""e63a2f8b-19f1-4185-a6f6-cb4b8f1ce413"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Skill5"",
                    ""type"": ""Button"",
                    ""id"": ""81e021b0-9c3c-4c88-884d-4c9ccbdd3c99"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Skill6"",
                    ""type"": ""Button"",
                    ""id"": ""62cdd748-a28d-490b-85fe-98aab7212549"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""784c6430-6ff4-4515-98b5-64d1d1155d60"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""cedd0be8-c60e-49be-a2c7-a13ee8168449"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""af5f03b5-b858-4821-a35d-ab5fd3957f14"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Arrow Keys"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""eeca468e-120f-464e-b902-0b40cd89a2dc"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Arrow Keys"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""28fff534-722d-4e82-96fb-2faacf497222"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Arrow Keys"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""61f4247f-7aa5-4c82-be0a-52446053734c"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Arrow Keys"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""e1250730-d19f-49e4-a4e9-b8712d5e2938"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Arrow Keys"",
                    ""action"": ""Exit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ee8cee1a-7dfb-4a5a-9afc-691e2c0e6666"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Arrow Keys"",
                    ""action"": ""View"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9ad81efa-16f0-47ce-90de-580372084cf1"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Arrow Keys"",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2bd0899d-80ea-4adb-979d-0ac0db824f43"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Arrow Keys"",
                    ""action"": ""Pickup"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c15f20f6-96b5-406b-bdcb-95b1864aa4ac"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Arrow Keys"",
                    ""action"": ""Drop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1f784235-750e-4dc3-9baf-2546a36c5599"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Arrow Keys"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3e2af82f-5305-4916-bde6-07338bf6dcc2"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Arrow Keys"",
                    ""action"": ""DropWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""16fbdf50-8361-4c99-9118-42686f0e1228"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Arrow Keys"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2bda5ace-a761-4cf9-b31f-533b45ce12c5"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Arrow Keys"",
                    ""action"": ""Info"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b29330cc-6621-4573-9d18-5c2b584200b4"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Arrow Keys"",
                    ""action"": ""Skill1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1e757a79-0185-47bf-8517-bf3ada6b19b0"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Arrow Keys"",
                    ""action"": ""Skill2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eae7cef5-8807-49d1-98ce-791c7ba27fd8"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Arrow Keys"",
                    ""action"": ""Skill3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bcb564d8-0792-4bc6-8d90-58b01d250ab2"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Arrow Keys"",
                    ""action"": ""Skill4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a14e18b7-e0ca-4b27-ac2f-91c55461c665"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Skill5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""11c2a1db-ef7f-49b5-81ae-03b98e2338c7"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Arrow Keys"",
                    ""action"": ""Skill6"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b402c8e8-d677-4c15-adbd-a6129eb2410d"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Arrow Keys"",
            ""bindingGroup"": ""Arrow Keys"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_Exit = m_Player.FindAction("Exit", throwIfNotFound: true);
        m_Player_View = m_Player.FindAction("View", throwIfNotFound: true);
        m_Player_Inventory = m_Player.FindAction("Inventory", throwIfNotFound: true);
        m_Player_Pickup = m_Player.FindAction("Pickup", throwIfNotFound: true);
        m_Player_Drop = m_Player.FindAction("Drop", throwIfNotFound: true);
        m_Player_Click = m_Player.FindAction("Click", throwIfNotFound: true);
        m_Player_DropWeapon = m_Player.FindAction("DropWeapon", throwIfNotFound: true);
        m_Player_Dash = m_Player.FindAction("Dash", throwIfNotFound: true);
        m_Player_Info = m_Player.FindAction("Info", throwIfNotFound: true);
        m_Player_Skill1 = m_Player.FindAction("Skill1", throwIfNotFound: true);
        m_Player_Skill2 = m_Player.FindAction("Skill2", throwIfNotFound: true);
        m_Player_Skill3 = m_Player.FindAction("Skill3", throwIfNotFound: true);
        m_Player_Skill4 = m_Player.FindAction("Skill4", throwIfNotFound: true);
        m_Player_Skill5 = m_Player.FindAction("Skill5", throwIfNotFound: true);
        m_Player_Skill6 = m_Player.FindAction("Skill6", throwIfNotFound: true);
        m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
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
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_Exit;
    private readonly InputAction m_Player_View;
    private readonly InputAction m_Player_Inventory;
    private readonly InputAction m_Player_Pickup;
    private readonly InputAction m_Player_Drop;
    private readonly InputAction m_Player_Click;
    private readonly InputAction m_Player_DropWeapon;
    private readonly InputAction m_Player_Dash;
    private readonly InputAction m_Player_Info;
    private readonly InputAction m_Player_Skill1;
    private readonly InputAction m_Player_Skill2;
    private readonly InputAction m_Player_Skill3;
    private readonly InputAction m_Player_Skill4;
    private readonly InputAction m_Player_Skill5;
    private readonly InputAction m_Player_Skill6;
    private readonly InputAction m_Player_Interact;
    public struct PlayerActions
    {
        private @Controls m_Wrapper;
        public PlayerActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @Exit => m_Wrapper.m_Player_Exit;
        public InputAction @View => m_Wrapper.m_Player_View;
        public InputAction @Inventory => m_Wrapper.m_Player_Inventory;
        public InputAction @Pickup => m_Wrapper.m_Player_Pickup;
        public InputAction @Drop => m_Wrapper.m_Player_Drop;
        public InputAction @Click => m_Wrapper.m_Player_Click;
        public InputAction @DropWeapon => m_Wrapper.m_Player_DropWeapon;
        public InputAction @Dash => m_Wrapper.m_Player_Dash;
        public InputAction @Info => m_Wrapper.m_Player_Info;
        public InputAction @Skill1 => m_Wrapper.m_Player_Skill1;
        public InputAction @Skill2 => m_Wrapper.m_Player_Skill2;
        public InputAction @Skill3 => m_Wrapper.m_Player_Skill3;
        public InputAction @Skill4 => m_Wrapper.m_Player_Skill4;
        public InputAction @Skill5 => m_Wrapper.m_Player_Skill5;
        public InputAction @Skill6 => m_Wrapper.m_Player_Skill6;
        public InputAction @Interact => m_Wrapper.m_Player_Interact;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Exit.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnExit;
                @Exit.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnExit;
                @Exit.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnExit;
                @View.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnView;
                @View.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnView;
                @View.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnView;
                @Inventory.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInventory;
                @Inventory.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInventory;
                @Inventory.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInventory;
                @Pickup.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPickup;
                @Pickup.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPickup;
                @Pickup.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPickup;
                @Drop.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDrop;
                @Drop.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDrop;
                @Drop.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDrop;
                @Click.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnClick;
                @DropWeapon.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDropWeapon;
                @DropWeapon.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDropWeapon;
                @DropWeapon.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDropWeapon;
                @Dash.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @Info.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInfo;
                @Info.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInfo;
                @Info.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInfo;
                @Skill1.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkill1;
                @Skill1.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkill1;
                @Skill1.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkill1;
                @Skill2.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkill2;
                @Skill2.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkill2;
                @Skill2.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkill2;
                @Skill3.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkill3;
                @Skill3.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkill3;
                @Skill3.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkill3;
                @Skill4.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkill4;
                @Skill4.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkill4;
                @Skill4.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkill4;
                @Skill5.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkill5;
                @Skill5.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkill5;
                @Skill5.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkill5;
                @Skill6.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkill6;
                @Skill6.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkill6;
                @Skill6.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSkill6;
                @Interact.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Exit.started += instance.OnExit;
                @Exit.performed += instance.OnExit;
                @Exit.canceled += instance.OnExit;
                @View.started += instance.OnView;
                @View.performed += instance.OnView;
                @View.canceled += instance.OnView;
                @Inventory.started += instance.OnInventory;
                @Inventory.performed += instance.OnInventory;
                @Inventory.canceled += instance.OnInventory;
                @Pickup.started += instance.OnPickup;
                @Pickup.performed += instance.OnPickup;
                @Pickup.canceled += instance.OnPickup;
                @Drop.started += instance.OnDrop;
                @Drop.performed += instance.OnDrop;
                @Drop.canceled += instance.OnDrop;
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
                @DropWeapon.started += instance.OnDropWeapon;
                @DropWeapon.performed += instance.OnDropWeapon;
                @DropWeapon.canceled += instance.OnDropWeapon;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @Info.started += instance.OnInfo;
                @Info.performed += instance.OnInfo;
                @Info.canceled += instance.OnInfo;
                @Skill1.started += instance.OnSkill1;
                @Skill1.performed += instance.OnSkill1;
                @Skill1.canceled += instance.OnSkill1;
                @Skill2.started += instance.OnSkill2;
                @Skill2.performed += instance.OnSkill2;
                @Skill2.canceled += instance.OnSkill2;
                @Skill3.started += instance.OnSkill3;
                @Skill3.performed += instance.OnSkill3;
                @Skill3.canceled += instance.OnSkill3;
                @Skill4.started += instance.OnSkill4;
                @Skill4.performed += instance.OnSkill4;
                @Skill4.canceled += instance.OnSkill4;
                @Skill5.started += instance.OnSkill5;
                @Skill5.performed += instance.OnSkill5;
                @Skill5.canceled += instance.OnSkill5;
                @Skill6.started += instance.OnSkill6;
                @Skill6.performed += instance.OnSkill6;
                @Skill6.canceled += instance.OnSkill6;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_ArrowKeysSchemeIndex = -1;
    public InputControlScheme ArrowKeysScheme
    {
        get
        {
            if (m_ArrowKeysSchemeIndex == -1) m_ArrowKeysSchemeIndex = asset.FindControlSchemeIndex("Arrow Keys");
            return asset.controlSchemes[m_ArrowKeysSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnExit(InputAction.CallbackContext context);
        void OnView(InputAction.CallbackContext context);
        void OnInventory(InputAction.CallbackContext context);
        void OnPickup(InputAction.CallbackContext context);
        void OnDrop(InputAction.CallbackContext context);
        void OnClick(InputAction.CallbackContext context);
        void OnDropWeapon(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnInfo(InputAction.CallbackContext context);
        void OnSkill1(InputAction.CallbackContext context);
        void OnSkill2(InputAction.CallbackContext context);
        void OnSkill3(InputAction.CallbackContext context);
        void OnSkill4(InputAction.CallbackContext context);
        void OnSkill5(InputAction.CallbackContext context);
        void OnSkill6(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
    }
}
