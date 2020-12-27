using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectNames : byte
{
    Explosion, Blood
}

[System.Serializable]
public class Effect
{
    public EffectNames name;
    public ParticleSystem preFab;
}

public class EffectsManager : MonoBehaviour
{
    private static EffectsManager instance;
    private void Awake()
    {
        instance = this;
        Routine();
    }

    [SerializeField] private EffectsData effectsData;
    private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

    public static void PlayEffectAt(EffectNames name, Vector3 position, float delay = 0)
    {
        instance.StartCoroutine(instance.PlayEffectAtCoroutine(name, position, delay));
    }

    private IEnumerator PlayEffectAtCoroutine(EffectNames name, Vector3 position, float delay)
    {
        if (delay > 0)
        {
            Debug.LogWarning("delay > 0)");

            yield return new WaitForSeconds(delay);
        }

        Effect[] effects = effectsData.effects;
        bool effectFound = false;

        for (int i = 0; i < effects.Length; i++)
        {
            Effect effect = effects[i];
            if (effect.name == name)
            {
                ParticleSystem particleSystem = Instantiate(effect.preFab);
                particleSystem.transform.position = position;
                particleSystems.Add(particleSystem);
                effectFound = true;
                break;
            }
        }

        if (!effectFound)
        {
            Debug.LogWarning("The requested effect was not found.");

        }
    }

    private void Routine()
    {
        for (int i = particleSystems.Count-1; i >= 0 ; i--)
        {
            ParticleSystem effect = particleSystems[i];
            if (!effect.isPlaying)
            {
                Destroy(effect.gameObject);
                particleSystems.RemoveAt(i);
            }
        }
        Invoke("Routine", 0.2f);
    }
}
