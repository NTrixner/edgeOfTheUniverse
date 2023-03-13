using System;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class FTLParticles : MonoBehaviour
{
    [SerializeField] private bool endParticlesOnStop;
    private float accelerationDuration;
    private float currentAcc = 0f;

    private ParticleSystem _particleSystem;

    private bool acc = false;

    private bool dec = false;

    private float rateOverTime;

    private Light light;
    
    // Start is called before the first frame update
    void Start()
    {
        FTLMovement.INSTANCE.StartTravelling.AddListener(StartParticles);
        FTLMovement.INSTANCE.StopTravelling.AddListener(StopOrPauseParticles);
        _particleSystem = GetComponent<ParticleSystem>();
        rateOverTime = _particleSystem.emission.rateOverTimeMultiplier;
        light = _particleSystem.lights.light;
        accelerationDuration = FTLMovement.INSTANCE.timeToStop;
    }

    void StartParticles()
    {
        acc = true;
        dec = false;
        _particleSystem.Play(true);
    }

    void StopOrPauseParticles()
    {
        acc = false;
        dec = true;
    }

    private void Update()
    {
        if (acc)
        {
            currentAcc += Time.deltaTime;
            var particleSystemEmission = _particleSystem.emission;
            particleSystemEmission.rateOverTimeMultiplier =  rateOverTime * (currentAcc / accelerationDuration);
            if (light != null)
            {
                light.intensity = currentAcc / accelerationDuration;
            }
            if (currentAcc >= accelerationDuration)
            {
                currentAcc = 0f;
                acc = false;
            }
        }

        if (dec)
        {
            currentAcc += Time.deltaTime;
            var particleSystemEmission = _particleSystem.emission;
            particleSystemEmission.rateOverTimeMultiplier = rateOverTime * (1 - currentAcc / accelerationDuration);
            if (light != null)
            {
                light.intensity = 1 - currentAcc / accelerationDuration;
            }
            if (currentAcc >= accelerationDuration)
            {
                dec = false;
                currentAcc = 0f;
                CompletelyStop();
            }
        }
    }


    void CompletelyStop()
    {
        if (endParticlesOnStop)
        {
            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        else
        {
            _particleSystem.Pause();
        }
    }

    private void OnDestroy()
    {
        FTLMovement.INSTANCE.StartTravelling.RemoveListener(StartParticles);
        FTLMovement.INSTANCE.StopTravelling.RemoveListener(StopOrPauseParticles);
    }
}