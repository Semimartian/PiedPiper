using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "SoundData", order = 1)]
public class SoundData : ScriptableObject
{
    public Sound[] sounds;
    public AudioSource oneShotSoundPreFab;
}
