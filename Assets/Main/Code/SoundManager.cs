using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SoundNames : byte
{
    Bark, ChickDeath, Sizzle, ChickTweet, GooseJump
}
[Serializable]
public class CorrectedSoundClip
{
    public AudioClip clip;
    [Range(0, 1)] public float volume=1;

}
[Serializable]
public class Sound
{
    public SoundNames name;
    public CorrectedSoundClip[] audioClips;
    public float minPitch;
    public float maxPitch;
    public float volumeModifier;
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private SoundData soundData;
    private static List<AudioSource> availableAudioSources;
    private static List<AudioSource> occupiedAudioSources;
    private Sound[] sounds;
    private float LookForIdleSoundsAndReturnThemInterval = 0.5f;
    private static int poolAddition = 16;

    private void Start()
    {
        int sourcesCount = poolAddition;
        availableAudioSources = new List<AudioSource>(sourcesCount);

        Transform soundsParent = new GameObject("Sounds").transform;

        for (int i = 0; i < sourcesCount; i++)
        {
            availableAudioSources.Add(Instantiate(soundData.oneShotSoundPreFab));
            availableAudioSources[i].gameObject.SetActive(false);
            availableAudioSources[i].gameObject.transform.SetParent(soundsParent);
        }
        occupiedAudioSources = new List<AudioSource>();
        sounds = soundData.sounds;

        LookForIdleSoundsAndReturnThem();
    }

    public static void PlayOneShotSoundAt(SoundNames name, Vector3 position, float delay = 0)
    {
        instance.StartCoroutine(instance.PlayOneShotSoundAtCoroutine(name, position, delay));
    }

    public static void PlayOneShotSoundAt(SoundNames name, AudioSource audioSource, float delay = 0)
    {
        instance.StartCoroutine(instance.PlayOneShotSoundAtCoroutine(name, audioSource, delay));
    }

    private IEnumerator PlayOneShotSoundAtCoroutine(SoundNames name, Vector3 position, float delay)
    {
        if (delay > 0)
        {
            Debug.LogWarning("delay > 0)");

            yield return new WaitForSeconds(delay);
        }
        CorrectedSoundClip clip = null;
        float minPitch = 0;
        float maxPitch = 0;
        float volumeModification = 0;


        for (int i = 0; i < sounds.Length; i++)
        {
            Sound sound = sounds[i];
            if (sounds[i].name == name)
            {
                int clipIndex = 0;
                for (int j = 0; j < 8; j++)
                {
                    clipIndex = UnityEngine.Random.Range(0, sound.audioClips.Length);
                }
                clip = sound.audioClips[clipIndex];
                minPitch = sound.minPitch; maxPitch = sound.maxPitch;
                volumeModification = UnityEngine.Random.Range(0, sound.volumeModifier);
            }
        }
        if (clip == null)
        {
            Debug.LogError("NO SOUND FOUND: " + name.ToString());
        }
        else
        {
            AudioSource oneShotSound = GetAnAvailableAudioSource();

            oneShotSound.gameObject.SetActive(true);

            oneShotSound.transform.position = position;
            oneShotSound.clip = clip.clip;
            oneShotSound.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
            oneShotSound.volume = clip.volume + volumeModification;
            oneShotSound.Play();
        }
    }

    private IEnumerator PlayOneShotSoundAtCoroutine(SoundNames name, AudioSource audioSource, float delay)
    {
        if (delay > 0)
        {
            Debug.LogWarning("delay > 0)");

            yield return new WaitForSeconds(delay);
        }
        CorrectedSoundClip clip = null;
        float minPitch = 0;
        float maxPitch = 0;
        for (int i = 0; i < sounds.Length; i++)
        {
            Sound sound = sounds[i];
            if (sounds[i].name == name)
            {
                clip = sound.audioClips[UnityEngine.Random.Range(0, sound.audioClips.Length)];
                minPitch = sound.minPitch; maxPitch = sound.maxPitch;
            }
        }
        if (clip == null)
        {
            Debug.LogError("NO SOUND FOUND!");
        }
        else
        {
            audioSource.clip = clip.clip;
            audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
            audioSource.volume = clip.volume;
            audioSource.Play();
        }
    }

    private static AudioSource GetAnAvailableAudioSource()
    {
        int count = availableAudioSources.Count;
        if (count <= 0)
        {
            Debug.LogWarning("No avialable Sound Sources! Creating more.");
            count = poolAddition;
            for (int i = 0; i < count; i++)
            {
                availableAudioSources.Add(Instantiate(instance.soundData.oneShotSoundPreFab));
            }
        }
        AudioSource source = availableAudioSources[count-1];
        availableAudioSources.RemoveAt(count-1);
        occupiedAudioSources.Add(source);
        return source;
    }

    private void LookForIdleSoundsAndReturnThem()
    {
        int count = occupiedAudioSources.Count - 1;
        for (int i = count; i > -1; i--)
        {
            AudioSource source = occupiedAudioSources[i];
            if (!source.isPlaying)
            {
                availableAudioSources.Add(source);
                occupiedAudioSources.RemoveAt(i);
                source.gameObject.SetActive(false);
            }

        }

        Invoke("LookForIdleSoundsAndReturnThem", LookForIdleSoundsAndReturnThemInterval);
    }
}
