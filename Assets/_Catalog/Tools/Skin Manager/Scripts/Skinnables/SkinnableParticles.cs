using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinnableParticles : SkinnableGeneral
{
    public bool            changeParticleColors;

    private ParticleSystem _particleSystems;

    internal override void Awake()
    {
        base.Awake();
        Setup();
    }

    internal override void Setup()
    {
        _particleSystems = new ParticleSystem();
        _particleSystems = GetComponent<ParticleSystem>();

        base.Setup();
    }

    public override void ApplySkin()
    {
        base.ApplySkin();

        ApplyParticleColors();
    }

    private void ApplyParticleColors()
    {
        if (changeParticleColors && _skinObject.particlesColors.Length > 0)
        {
            for (int c = 0; c < _skinObject.particlesColors.Length; c++)
            {
                switch (_skinObject.particlesColors[c].gradType)
                {
                    case ParticleGradType.StartColor:
                        ParticleSystem.MainModule main = _particleSystems.main;
                        main.startColor                = _skinObject.particlesColors[c].particleSystemGrad;
                        break;

                    case ParticleGradType.OverLife:
                        var overLT    = _particleSystems.colorOverLifetime;
                        overLT.color  = _skinObject.particlesColors[c].particleSystemGrad;
                        break;

                    case ParticleGradType.BySpeed:
                        var bySpeed   = _particleSystems.colorBySpeed;
                        bySpeed.color = _skinObject.particlesColors[c].particleSystemGrad;
                        break;

                    default:
                        break;
                }
            }
        }
    }

    public Dictionary<ParticleGradType, ParticlesColor> GetParticleColors()
    {
        if (_particleSystems == null)
            _particleSystems = GetComponent<ParticleSystem>();

        Dictionary<ParticleGradType, ParticlesColor> particlesColors = new Dictionary<ParticleGradType, ParticlesColor>();

        ParticlesColor startColor = new ParticlesColor();
        ParticlesColor overLife   = new ParticlesColor();
        ParticlesColor bySpeed    = new ParticlesColor();

        startColor.gradType = ParticleGradType.StartColor;
        overLife.gradType   = ParticleGradType.OverLife;
        bySpeed.gradType    = ParticleGradType.BySpeed;

        startColor.particleSystemGrad = _particleSystems.main.startColor.gradient;
        overLife.particleSystemGrad   = _particleSystems.colorOverLifetime.color.gradient;
        bySpeed.particleSystemGrad    = _particleSystems.colorBySpeed.color.gradient;

        particlesColors.Add(ParticleGradType.StartColor, startColor);
        particlesColors.Add(ParticleGradType.OverLife  , overLife);
        particlesColors.Add(ParticleGradType.BySpeed   , bySpeed);

        return particlesColors;
    }
}
