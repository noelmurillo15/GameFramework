// GENERATED AUTOMATICALLY FROM 'Assets/_Project/Data/Input/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""7083e2c6-a96b-4ea8-b783-d1672d84e0de"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a48a7013-3a4a-4f27-9d14-278a0b2e47d1"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""f7225671-931d-4a08-8e09-d7a7475843f2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ResponseKey"",
                    ""type"": ""Button"",
                    ""id"": ""a48c7163-9f0b-4916-a99a-3dc4853d5510"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ea31965d-bcfc-495b-a9c9-883579050367"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""d18cbea4-3b68-4c6b-a130-a5ebc226472d"",
                    ""path"": ""Dpad(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""bb7b8da9-e700-41c0-9942-7ece94c79479"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""fa0430e4-e354-4c4a-b7ee-b9c33f6540d1"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""94b6384f-1e59-453d-9be6-a7cef0ef2b58"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a17826f2-3e1c-40f8-897f-9a86416e3970"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c6ec838c-80ca-4599-be86-f5254451e487"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""61109b9c-eae7-44e2-b5d7-834679eb1708"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""afcf9800-1655-409e-ad7e-1b8af652f6e7"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0b54d012-ac14-45a0-9a06-512c0ae367ed"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2812388c-5811-4b49-956c-3c90e55f2e7f"",
                    ""path"": ""keyboard/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Num1-4"",
                    ""id"": ""cc00222c-e0f4-4140-a742-c9cbef1c3bcb"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ResponseKey"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""55d5ab0b-c02e-4cdc-814b-468abe37dc8f"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ResponseKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a9486b9e-f36e-447a-837d-bb6917b820f4"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ResponseKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""4b16c944-f40d-4607-99cb-186d14249e7e"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ResponseKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""c743cf44-2ef9-45d5-999f-6c1e3b752196"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ResponseKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Ui"",
            ""id"": ""5674c10e-b387-4f2c-bfcc-58b1318f6334"",
            ""actions"": [
                {
                    ""name"": ""Navigate"",
                    ""type"": ""Value"",
                    ""id"": ""c6e5def9-0b25-4d8e-befe-8e3643e26ea8"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""77773ebb-d44a-4076-a1f3-2a39d168a604"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""e76ce503-b31c-4839-a923-bf396324b2c2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Point"",
                    ""type"": ""PassThrough"",
                    ""id"": ""f1619c4f-8d8c-48a3-a418-ceb11afd77b7"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""PassThrough"",
                    ""id"": ""74ec4a92-b8f5-4b50-8e07-0bc09419f826"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ScrollWheel"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d22a80e1-01bd-4936-908e-2b24a7a7b3ba"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MiddleClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""0616610a-4725-41ed-9709-cd6471b7c9c5"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""4e661b2a-592e-42ad-97c8-166152f88b81"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""InventoryToggle"",
                    ""type"": ""Button"",
                    ""id"": ""f2fc5cad-2459-48d4-8bdb-fe33d7344615"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PauseToggle"",
                    ""type"": ""Button"",
                    ""id"": ""fe7de79b-7f18-459c-81d7-caf2192cf682"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MapToggle"",
                    ""type"": ""Button"",
                    ""id"": ""d0321659-c5f9-42f8-9f25-689508239406"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PdfViewToggle"",
                    ""type"": ""Button"",
                    ""id"": ""42bfd243-0d79-408d-80f0-70641fe03b5b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""e687f108-9e2b-4188-8e50-b37f6f777558"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""26af7e08-e3a6-4b57-bec4-2c1c96c10528"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""eb711030-efa6-4445-a566-6a29b3e8d2c8"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""bb65d6a2-fd5e-4fd6-88c4-42a52cce7d1a"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d44446b6-8149-4a36-ac84-1adff24320cb"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6bd9d809-38a2-4fbb-8fae-491de9f2511b"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""aea29593-cc13-4138-9ae9-271febf51059"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b88422e0-686a-4d52-b3c7-aa58b30465f2"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""c4a0d4e9-25d0-45d5-ba28-2fa025533258"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""e0dbe705-cbb2-4c00-b422-123f73ac4ae1"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""efb437af-4382-4315-b2c3-f68d27cb2ecb"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c5685ec7-177b-494c-9eba-ebaae6f302de"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a459f253-4159-4c05-a3a6-0d53a0337530"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""496ed30e-958c-49d2-9153-1033da812ba4"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""af55ff91-c427-497e-a1f2-4543c83d8b16"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e70f77b8-3213-4a22-bdd3-d9ea39f12e7c"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""3be120b6-4698-4bbc-b209-4b311ac9f88e"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1ff6b7d5-e1be-4ee7-ae7a-8c681c8b776e"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""c119d448-e0c7-4c5d-a303-0ff80355b29b"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c1f994d8-da22-44f3-9289-7bf58dfa97ce"",
                    ""path"": ""*/{Submit}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c1f7e7ee-37af-441a-af67-ea264519b862"",
                    ""path"": ""*/{Cancel}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1d0cccd0-fc9e-4d9a-b97d-bce6d7df4db5"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cc90d2f4-619c-45c4-b999-b877eb06ab74"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9edfbce6-31de-4c97-9204-e0647c3ba13c"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""ScrollWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5cf61461-f206-4cff-81fb-91ae73e7eb6e"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""MiddleClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""542546b3-1a57-4819-b843-508c5ed45189"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8d37100a-03c8-4c5a-bf67-9ca41d9933b0"",
                    ""path"": ""keyboard/I"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InventoryToggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ef9011cd-63d0-455f-a1c3-aa7b421dc5d8"",
                    ""path"": ""keyboard/Tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseToggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""77853c2e-6e21-48a3-a7ac-8f120bda10e7"",
                    ""path"": ""keyboard/M"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MapToggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""afb02886-9369-45ac-8aa5-cef71212035a"",
                    ""path"": ""keyboard/P"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PdfViewToggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Move = m_Gameplay.FindAction("Move", throwIfNotFound: true);
        m_Gameplay_Interact = m_Gameplay.FindAction("Interact", throwIfNotFound: true);
        m_Gameplay_ResponseKey = m_Gameplay.FindAction("ResponseKey", throwIfNotFound: true);
        // Ui
        m_Ui = asset.FindActionMap("Ui", throwIfNotFound: true);
        m_Ui_Navigate = m_Ui.FindAction("Navigate", throwIfNotFound: true);
        m_Ui_Submit = m_Ui.FindAction("Submit", throwIfNotFound: true);
        m_Ui_Cancel = m_Ui.FindAction("Cancel", throwIfNotFound: true);
        m_Ui_Point = m_Ui.FindAction("Point", throwIfNotFound: true);
        m_Ui_Click = m_Ui.FindAction("Click", throwIfNotFound: true);
        m_Ui_ScrollWheel = m_Ui.FindAction("ScrollWheel", throwIfNotFound: true);
        m_Ui_MiddleClick = m_Ui.FindAction("MiddleClick", throwIfNotFound: true);
        m_Ui_RightClick = m_Ui.FindAction("RightClick", throwIfNotFound: true);
        m_Ui_InventoryToggle = m_Ui.FindAction("InventoryToggle", throwIfNotFound: true);
        m_Ui_PauseToggle = m_Ui.FindAction("PauseToggle", throwIfNotFound: true);
        m_Ui_MapToggle = m_Ui.FindAction("MapToggle", throwIfNotFound: true);
        m_Ui_PdfViewToggle = m_Ui.FindAction("PdfViewToggle", throwIfNotFound: true);
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

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Move;
    private readonly InputAction m_Gameplay_Interact;
    private readonly InputAction m_Gameplay_ResponseKey;
    public struct GameplayActions
    {
        private @PlayerControls m_Wrapper;
        public GameplayActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Gameplay_Move;
        public InputAction @Interact => m_Wrapper.m_Gameplay_Interact;
        public InputAction @ResponseKey => m_Wrapper.m_Gameplay_ResponseKey;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Interact.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                @ResponseKey.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnResponseKey;
                @ResponseKey.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnResponseKey;
                @ResponseKey.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnResponseKey;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @ResponseKey.started += instance.OnResponseKey;
                @ResponseKey.performed += instance.OnResponseKey;
                @ResponseKey.canceled += instance.OnResponseKey;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);

    // Ui
    private readonly InputActionMap m_Ui;
    private IUiActions m_UiActionsCallbackInterface;
    private readonly InputAction m_Ui_Navigate;
    private readonly InputAction m_Ui_Submit;
    private readonly InputAction m_Ui_Cancel;
    private readonly InputAction m_Ui_Point;
    private readonly InputAction m_Ui_Click;
    private readonly InputAction m_Ui_ScrollWheel;
    private readonly InputAction m_Ui_MiddleClick;
    private readonly InputAction m_Ui_RightClick;
    private readonly InputAction m_Ui_InventoryToggle;
    private readonly InputAction m_Ui_PauseToggle;
    private readonly InputAction m_Ui_MapToggle;
    private readonly InputAction m_Ui_PdfViewToggle;
    public struct UiActions
    {
        private @PlayerControls m_Wrapper;
        public UiActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Navigate => m_Wrapper.m_Ui_Navigate;
        public InputAction @Submit => m_Wrapper.m_Ui_Submit;
        public InputAction @Cancel => m_Wrapper.m_Ui_Cancel;
        public InputAction @Point => m_Wrapper.m_Ui_Point;
        public InputAction @Click => m_Wrapper.m_Ui_Click;
        public InputAction @ScrollWheel => m_Wrapper.m_Ui_ScrollWheel;
        public InputAction @MiddleClick => m_Wrapper.m_Ui_MiddleClick;
        public InputAction @RightClick => m_Wrapper.m_Ui_RightClick;
        public InputAction @InventoryToggle => m_Wrapper.m_Ui_InventoryToggle;
        public InputAction @PauseToggle => m_Wrapper.m_Ui_PauseToggle;
        public InputAction @MapToggle => m_Wrapper.m_Ui_MapToggle;
        public InputAction @PdfViewToggle => m_Wrapper.m_Ui_PdfViewToggle;
        public InputActionMap Get() { return m_Wrapper.m_Ui; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UiActions set) { return set.Get(); }
        public void SetCallbacks(IUiActions instance)
        {
            if (m_Wrapper.m_UiActionsCallbackInterface != null)
            {
                @Navigate.started -= m_Wrapper.m_UiActionsCallbackInterface.OnNavigate;
                @Navigate.performed -= m_Wrapper.m_UiActionsCallbackInterface.OnNavigate;
                @Navigate.canceled -= m_Wrapper.m_UiActionsCallbackInterface.OnNavigate;
                @Submit.started -= m_Wrapper.m_UiActionsCallbackInterface.OnSubmit;
                @Submit.performed -= m_Wrapper.m_UiActionsCallbackInterface.OnSubmit;
                @Submit.canceled -= m_Wrapper.m_UiActionsCallbackInterface.OnSubmit;
                @Cancel.started -= m_Wrapper.m_UiActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_UiActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_UiActionsCallbackInterface.OnCancel;
                @Point.started -= m_Wrapper.m_UiActionsCallbackInterface.OnPoint;
                @Point.performed -= m_Wrapper.m_UiActionsCallbackInterface.OnPoint;
                @Point.canceled -= m_Wrapper.m_UiActionsCallbackInterface.OnPoint;
                @Click.started -= m_Wrapper.m_UiActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_UiActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_UiActionsCallbackInterface.OnClick;
                @ScrollWheel.started -= m_Wrapper.m_UiActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.performed -= m_Wrapper.m_UiActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.canceled -= m_Wrapper.m_UiActionsCallbackInterface.OnScrollWheel;
                @MiddleClick.started -= m_Wrapper.m_UiActionsCallbackInterface.OnMiddleClick;
                @MiddleClick.performed -= m_Wrapper.m_UiActionsCallbackInterface.OnMiddleClick;
                @MiddleClick.canceled -= m_Wrapper.m_UiActionsCallbackInterface.OnMiddleClick;
                @RightClick.started -= m_Wrapper.m_UiActionsCallbackInterface.OnRightClick;
                @RightClick.performed -= m_Wrapper.m_UiActionsCallbackInterface.OnRightClick;
                @RightClick.canceled -= m_Wrapper.m_UiActionsCallbackInterface.OnRightClick;
                @InventoryToggle.started -= m_Wrapper.m_UiActionsCallbackInterface.OnInventoryToggle;
                @InventoryToggle.performed -= m_Wrapper.m_UiActionsCallbackInterface.OnInventoryToggle;
                @InventoryToggle.canceled -= m_Wrapper.m_UiActionsCallbackInterface.OnInventoryToggle;
                @PauseToggle.started -= m_Wrapper.m_UiActionsCallbackInterface.OnPauseToggle;
                @PauseToggle.performed -= m_Wrapper.m_UiActionsCallbackInterface.OnPauseToggle;
                @PauseToggle.canceled -= m_Wrapper.m_UiActionsCallbackInterface.OnPauseToggle;
                @MapToggle.started -= m_Wrapper.m_UiActionsCallbackInterface.OnMapToggle;
                @MapToggle.performed -= m_Wrapper.m_UiActionsCallbackInterface.OnMapToggle;
                @MapToggle.canceled -= m_Wrapper.m_UiActionsCallbackInterface.OnMapToggle;
                @PdfViewToggle.started -= m_Wrapper.m_UiActionsCallbackInterface.OnPdfViewToggle;
                @PdfViewToggle.performed -= m_Wrapper.m_UiActionsCallbackInterface.OnPdfViewToggle;
                @PdfViewToggle.canceled -= m_Wrapper.m_UiActionsCallbackInterface.OnPdfViewToggle;
            }
            m_Wrapper.m_UiActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Navigate.started += instance.OnNavigate;
                @Navigate.performed += instance.OnNavigate;
                @Navigate.canceled += instance.OnNavigate;
                @Submit.started += instance.OnSubmit;
                @Submit.performed += instance.OnSubmit;
                @Submit.canceled += instance.OnSubmit;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @Point.started += instance.OnPoint;
                @Point.performed += instance.OnPoint;
                @Point.canceled += instance.OnPoint;
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
                @ScrollWheel.started += instance.OnScrollWheel;
                @ScrollWheel.performed += instance.OnScrollWheel;
                @ScrollWheel.canceled += instance.OnScrollWheel;
                @MiddleClick.started += instance.OnMiddleClick;
                @MiddleClick.performed += instance.OnMiddleClick;
                @MiddleClick.canceled += instance.OnMiddleClick;
                @RightClick.started += instance.OnRightClick;
                @RightClick.performed += instance.OnRightClick;
                @RightClick.canceled += instance.OnRightClick;
                @InventoryToggle.started += instance.OnInventoryToggle;
                @InventoryToggle.performed += instance.OnInventoryToggle;
                @InventoryToggle.canceled += instance.OnInventoryToggle;
                @PauseToggle.started += instance.OnPauseToggle;
                @PauseToggle.performed += instance.OnPauseToggle;
                @PauseToggle.canceled += instance.OnPauseToggle;
                @MapToggle.started += instance.OnMapToggle;
                @MapToggle.performed += instance.OnMapToggle;
                @MapToggle.canceled += instance.OnMapToggle;
                @PdfViewToggle.started += instance.OnPdfViewToggle;
                @PdfViewToggle.performed += instance.OnPdfViewToggle;
                @PdfViewToggle.canceled += instance.OnPdfViewToggle;
            }
        }
    }
    public UiActions @Ui => new UiActions(this);
    public interface IGameplayActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnResponseKey(InputAction.CallbackContext context);
    }
    public interface IUiActions
    {
        void OnNavigate(InputAction.CallbackContext context);
        void OnSubmit(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnPoint(InputAction.CallbackContext context);
        void OnClick(InputAction.CallbackContext context);
        void OnScrollWheel(InputAction.CallbackContext context);
        void OnMiddleClick(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
        void OnInventoryToggle(InputAction.CallbackContext context);
        void OnPauseToggle(InputAction.CallbackContext context);
        void OnMapToggle(InputAction.CallbackContext context);
        void OnPdfViewToggle(InputAction.CallbackContext context);
    }
}
