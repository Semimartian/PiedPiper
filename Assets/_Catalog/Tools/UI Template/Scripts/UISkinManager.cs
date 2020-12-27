using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkinManager : MonoBehaviour
{
    public UISkinConfig UiSkin;
    public bool         AlwaysUpdate;


    public static Dictionary<UiObjectType, Color> ObjectColors;

    public static UISkinManager Instance;

    public enum UiObjectType
    {
        BrightText,
        DarkText,
        ToggleButtons,
        PositiveButton,
        NegativeButton,
        NeutralButton,
        MenuTitleBackgroud,
        MenuBackgroud,
        MenuFrame,
        InGameInterface,
        InGameIcons,
        InGameTexts,
        InputObject,
        InputText,
        InputField,
    };

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("More then one 'UI Skin Manager' was created");
        }

        UpdateColors();
    }

    void Update()
    {
        if (AlwaysUpdate)
            UpdateColors();
    }

    private void UpdateColors()
    {
        ObjectColors = new Dictionary<UiObjectType, Color>()
        {
            { UiObjectType.BrightText        , UiSkin.BrightText         },
            { UiObjectType.DarkText          , UiSkin.DarkText           },
            { UiObjectType.InGameIcons       , UiSkin.InGameIcons        },
            { UiObjectType.InGameInterface   , UiSkin.InGameInterface    },
            { UiObjectType.InGameTexts       , UiSkin.InGameTexts        },
            { UiObjectType.MenuBackgroud     , UiSkin.MenuBackgroud      },
            { UiObjectType.MenuFrame         , UiSkin.MenuFrame          },
            { UiObjectType.MenuTitleBackgroud, UiSkin.MenuTitleBackgroud },
            { UiObjectType.NegativeButton    , UiSkin.NegativeButton     },
            { UiObjectType.NeutralButton     , UiSkin.NeutralButton      },
            { UiObjectType.PositiveButton    , UiSkin.PositiveButton     },
            { UiObjectType.ToggleButtons     , UiSkin.ToggleButtons      },
            { UiObjectType.InputField        , UiSkin.InputField         },
            { UiObjectType.InputObject       , UiSkin.InputObject        },
            { UiObjectType.InputText         , UiSkin.InputText          },
        };
    }
}
