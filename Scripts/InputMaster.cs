// GENERATED AUTOMATICALLY FROM 'Assets/Common/InputMaster.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputMaster : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""Menu"",
            ""id"": ""be5e3022-2574-4c8d-b344-daf446c91ca1"",
            ""actions"": [
                {
                    ""name"": ""Confirm"",
                    ""type"": ""Button"",
                    ""id"": ""20dc2c58-28e8-451d-be72-1ea0b8d37986"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""moveUp"",
                    ""type"": ""Button"",
                    ""id"": ""21d18db9-8997-4f68-9a41-5dc69ffb6b13"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""moveDown"",
                    ""type"": ""Button"",
                    ""id"": ""bcfd2ff8-bfb8-42f0-a2fd-76983819c80e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Back"",
                    ""type"": ""Button"",
                    ""id"": ""5a5f2bbc-c6f3-46d0-97a1-ad096f699532"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""771c3c54-90b6-42ee-861f-4666673314fa"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8a830830-ba1d-4443-9ed8-41a5bdb545d1"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""80417798-6e1e-44e6-b174-272b952160d2"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""moveUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0b5c07d5-9233-4035-9a01-376c9b61a477"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""moveUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b26eb0b8-98c8-409c-9f2f-7cc23dea9b19"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""moveDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cfe814de-3855-4557-a31e-24d35c4205cc"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""moveDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e7b8ffa8-a246-4098-a707-6063fc037a28"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""DungeonCrawlerPlayer"",
            ""id"": ""78350479-4973-48e9-aff0-109f4208d352"",
            ""actions"": [
                {
                    ""name"": ""strafeLeft"",
                    ""type"": ""Button"",
                    ""id"": ""ffe1bb6b-ea04-4c24-ab67-354ac64ee8fa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""strafeRight"",
                    ""type"": ""Button"",
                    ""id"": ""e8ef0634-3175-49a7-99a8-49aeddad1338"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""moveForward"",
                    ""type"": ""Button"",
                    ""id"": ""84758562-d6c3-49e0-a822-3e152d61cd29"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""moveBackward"",
                    ""type"": ""Button"",
                    ""id"": ""ade0214b-1040-4b06-9e90-38234fa5fd2b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""turnLeft"",
                    ""type"": ""Button"",
                    ""id"": ""a7dd0775-b0a4-4791-9416-17a9a4e0f44c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""turnRight"",
                    ""type"": ""Button"",
                    ""id"": ""9e20c403-6a5c-4e8c-a01a-feee77c13d56"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c6b04053-d7e7-4c6b-a905-ab3d6fa3d192"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""strafeLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7862857e-9949-44b1-adc9-74cf4315c89e"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""strafeRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cb65f788-705a-485c-b0ef-0cb2b94471f9"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""moveForward"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1b309736-e71c-4810-bfc8-844474391206"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""moveForward"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4cd67136-953e-44e6-9f62-18e530e77d2e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""moveBackward"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f9249582-e843-4055-8128-63520a4ec68a"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""moveBackward"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aa4222d3-7724-4594-8797-ca5af7e7e4df"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""turnLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d3468139-0410-40ac-9763-05b062d79874"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""turnRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c6ae9212-3576-4229-9198-6271cf7e0d4c"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""turnRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e59b3f93-de2d-4ce6-adaa-07b25e860e68"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""turnLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Menu
        m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
        m_Menu_Confirm = m_Menu.FindAction("Confirm", throwIfNotFound: true);
        m_Menu_moveUp = m_Menu.FindAction("moveUp", throwIfNotFound: true);
        m_Menu_moveDown = m_Menu.FindAction("moveDown", throwIfNotFound: true);
        m_Menu_Back = m_Menu.FindAction("Back", throwIfNotFound: true);
        // DungeonCrawlerPlayer
        m_DungeonCrawlerPlayer = asset.FindActionMap("DungeonCrawlerPlayer", throwIfNotFound: true);
        m_DungeonCrawlerPlayer_strafeLeft = m_DungeonCrawlerPlayer.FindAction("strafeLeft", throwIfNotFound: true);
        m_DungeonCrawlerPlayer_strafeRight = m_DungeonCrawlerPlayer.FindAction("strafeRight", throwIfNotFound: true);
        m_DungeonCrawlerPlayer_moveForward = m_DungeonCrawlerPlayer.FindAction("moveForward", throwIfNotFound: true);
        m_DungeonCrawlerPlayer_moveBackward = m_DungeonCrawlerPlayer.FindAction("moveBackward", throwIfNotFound: true);
        m_DungeonCrawlerPlayer_turnLeft = m_DungeonCrawlerPlayer.FindAction("turnLeft", throwIfNotFound: true);
        m_DungeonCrawlerPlayer_turnRight = m_DungeonCrawlerPlayer.FindAction("turnRight", throwIfNotFound: true);
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

    // Menu
    private readonly InputActionMap m_Menu;
    private IMenuActions m_MenuActionsCallbackInterface;
    private readonly InputAction m_Menu_Confirm;
    private readonly InputAction m_Menu_moveUp;
    private readonly InputAction m_Menu_moveDown;
    private readonly InputAction m_Menu_Back;
    public struct MenuActions
    {
        private @InputMaster m_Wrapper;
        public MenuActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Confirm => m_Wrapper.m_Menu_Confirm;
        public InputAction @moveUp => m_Wrapper.m_Menu_moveUp;
        public InputAction @moveDown => m_Wrapper.m_Menu_moveDown;
        public InputAction @Back => m_Wrapper.m_Menu_Back;
        public InputActionMap Get() { return m_Wrapper.m_Menu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
        public void SetCallbacks(IMenuActions instance)
        {
            if (m_Wrapper.m_MenuActionsCallbackInterface != null)
            {
                @Confirm.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnConfirm;
                @Confirm.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnConfirm;
                @Confirm.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnConfirm;
                @moveUp.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnMoveUp;
                @moveUp.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnMoveUp;
                @moveUp.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnMoveUp;
                @moveDown.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnMoveDown;
                @moveDown.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnMoveDown;
                @moveDown.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnMoveDown;
                @Back.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnBack;
                @Back.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnBack;
                @Back.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnBack;
            }
            m_Wrapper.m_MenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Confirm.started += instance.OnConfirm;
                @Confirm.performed += instance.OnConfirm;
                @Confirm.canceled += instance.OnConfirm;
                @moveUp.started += instance.OnMoveUp;
                @moveUp.performed += instance.OnMoveUp;
                @moveUp.canceled += instance.OnMoveUp;
                @moveDown.started += instance.OnMoveDown;
                @moveDown.performed += instance.OnMoveDown;
                @moveDown.canceled += instance.OnMoveDown;
                @Back.started += instance.OnBack;
                @Back.performed += instance.OnBack;
                @Back.canceled += instance.OnBack;
            }
        }
    }
    public MenuActions @Menu => new MenuActions(this);

    // DungeonCrawlerPlayer
    private readonly InputActionMap m_DungeonCrawlerPlayer;
    private IDungeonCrawlerPlayerActions m_DungeonCrawlerPlayerActionsCallbackInterface;
    private readonly InputAction m_DungeonCrawlerPlayer_strafeLeft;
    private readonly InputAction m_DungeonCrawlerPlayer_strafeRight;
    private readonly InputAction m_DungeonCrawlerPlayer_moveForward;
    private readonly InputAction m_DungeonCrawlerPlayer_moveBackward;
    private readonly InputAction m_DungeonCrawlerPlayer_turnLeft;
    private readonly InputAction m_DungeonCrawlerPlayer_turnRight;
    public struct DungeonCrawlerPlayerActions
    {
        private @InputMaster m_Wrapper;
        public DungeonCrawlerPlayerActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @strafeLeft => m_Wrapper.m_DungeonCrawlerPlayer_strafeLeft;
        public InputAction @strafeRight => m_Wrapper.m_DungeonCrawlerPlayer_strafeRight;
        public InputAction @moveForward => m_Wrapper.m_DungeonCrawlerPlayer_moveForward;
        public InputAction @moveBackward => m_Wrapper.m_DungeonCrawlerPlayer_moveBackward;
        public InputAction @turnLeft => m_Wrapper.m_DungeonCrawlerPlayer_turnLeft;
        public InputAction @turnRight => m_Wrapper.m_DungeonCrawlerPlayer_turnRight;
        public InputActionMap Get() { return m_Wrapper.m_DungeonCrawlerPlayer; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DungeonCrawlerPlayerActions set) { return set.Get(); }
        public void SetCallbacks(IDungeonCrawlerPlayerActions instance)
        {
            if (m_Wrapper.m_DungeonCrawlerPlayerActionsCallbackInterface != null)
            {
                @strafeLeft.started -= m_Wrapper.m_DungeonCrawlerPlayerActionsCallbackInterface.OnStrafeLeft;
                @strafeLeft.performed -= m_Wrapper.m_DungeonCrawlerPlayerActionsCallbackInterface.OnStrafeLeft;
                @strafeLeft.canceled -= m_Wrapper.m_DungeonCrawlerPlayerActionsCallbackInterface.OnStrafeLeft;
                @strafeRight.started -= m_Wrapper.m_DungeonCrawlerPlayerActionsCallbackInterface.OnStrafeRight;
                @strafeRight.performed -= m_Wrapper.m_DungeonCrawlerPlayerActionsCallbackInterface.OnStrafeRight;
                @strafeRight.canceled -= m_Wrapper.m_DungeonCrawlerPlayerActionsCallbackInterface.OnStrafeRight;
                @moveForward.started -= m_Wrapper.m_DungeonCrawlerPlayerActionsCallbackInterface.OnMoveForward;
                @moveForward.performed -= m_Wrapper.m_DungeonCrawlerPlayerActionsCallbackInterface.OnMoveForward;
                @moveForward.canceled -= m_Wrapper.m_DungeonCrawlerPlayerActionsCallbackInterface.OnMoveForward;
                @moveBackward.started -= m_Wrapper.m_DungeonCrawlerPlayerActionsCallbackInterface.OnMoveBackward;
                @moveBackward.performed -= m_Wrapper.m_DungeonCrawlerPlayerActionsCallbackInterface.OnMoveBackward;
                @moveBackward.canceled -= m_Wrapper.m_DungeonCrawlerPlayerActionsCallbackInterface.OnMoveBackward;
                @turnLeft.started -= m_Wrapper.m_DungeonCrawlerPlayerActionsCallbackInterface.OnTurnLeft;
                @turnLeft.performed -= m_Wrapper.m_DungeonCrawlerPlayerActionsCallbackInterface.OnTurnLeft;
                @turnLeft.canceled -= m_Wrapper.m_DungeonCrawlerPlayerActionsCallbackInterface.OnTurnLeft;
                @turnRight.started -= m_Wrapper.m_DungeonCrawlerPlayerActionsCallbackInterface.OnTurnRight;
                @turnRight.performed -= m_Wrapper.m_DungeonCrawlerPlayerActionsCallbackInterface.OnTurnRight;
                @turnRight.canceled -= m_Wrapper.m_DungeonCrawlerPlayerActionsCallbackInterface.OnTurnRight;
            }
            m_Wrapper.m_DungeonCrawlerPlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @strafeLeft.started += instance.OnStrafeLeft;
                @strafeLeft.performed += instance.OnStrafeLeft;
                @strafeLeft.canceled += instance.OnStrafeLeft;
                @strafeRight.started += instance.OnStrafeRight;
                @strafeRight.performed += instance.OnStrafeRight;
                @strafeRight.canceled += instance.OnStrafeRight;
                @moveForward.started += instance.OnMoveForward;
                @moveForward.performed += instance.OnMoveForward;
                @moveForward.canceled += instance.OnMoveForward;
                @moveBackward.started += instance.OnMoveBackward;
                @moveBackward.performed += instance.OnMoveBackward;
                @moveBackward.canceled += instance.OnMoveBackward;
                @turnLeft.started += instance.OnTurnLeft;
                @turnLeft.performed += instance.OnTurnLeft;
                @turnLeft.canceled += instance.OnTurnLeft;
                @turnRight.started += instance.OnTurnRight;
                @turnRight.performed += instance.OnTurnRight;
                @turnRight.canceled += instance.OnTurnRight;
            }
        }
    }
    public DungeonCrawlerPlayerActions @DungeonCrawlerPlayer => new DungeonCrawlerPlayerActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IMenuActions
    {
        void OnConfirm(InputAction.CallbackContext context);
        void OnMoveUp(InputAction.CallbackContext context);
        void OnMoveDown(InputAction.CallbackContext context);
        void OnBack(InputAction.CallbackContext context);
    }
    public interface IDungeonCrawlerPlayerActions
    {
        void OnStrafeLeft(InputAction.CallbackContext context);
        void OnStrafeRight(InputAction.CallbackContext context);
        void OnMoveForward(InputAction.CallbackContext context);
        void OnMoveBackward(InputAction.CallbackContext context);
        void OnTurnLeft(InputAction.CallbackContext context);
        void OnTurnRight(InputAction.CallbackContext context);
    }
}
