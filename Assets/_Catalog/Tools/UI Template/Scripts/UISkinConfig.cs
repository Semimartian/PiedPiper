using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ui Skin Config")]
public class UISkinConfig : ScriptableObject
{
    [Header("Texts")]
    public Color BrightText;
    public Color DarkText;

    [Header("Buttons")]
    public Color ToggleButtons;
    public Color PositiveButton;
    public Color NegativeButton; 
    public Color NeutralButton; 

    [Header("Menu")]
    public Color MenuTitleBackgroud;
    public Color MenuBackgroud;
    public Color MenuFrame;

    [Header("In Game")]
    public Color InGameInterface;
    public Color InGameIcons;
    public Color InGameTexts;

    [Header("Input Objects")]
    public Color InputObject;
    public Color InputText;
    public Color InputField;

}
