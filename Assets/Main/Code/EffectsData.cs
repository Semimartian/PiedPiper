using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectsData", menuName = "EffectsData", order = 1)]
public class EffectsData : ScriptableObject
{
    public Effect[] effects;
}
