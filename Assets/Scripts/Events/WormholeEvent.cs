using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class WormholeEvent : MonoBehaviour, BaseEvent
{
    public UnityEvent EndEvent;

    public long maxDistanceGain;

    public long maxDistanceLoss;

    // Start is called before the first frame update
    void Awake()
    {
        EndEvent = new UnityEvent();
        maxDistanceGain = Random.Range(5000, 999999999);
        maxDistanceLoss = Random.Range(5000, 999999999);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public string GetDescription()
    {
        return
            $"We have encountered a Wormhole! If we enter it, we might jump {maxDistanceGain} light years closer to our goal. Or we might be set up to {maxDistanceLoss} light years back. Or something in between, I don't know.";
    }

    public string[] GetOptions()
    {
        return new[]
        {
            "Jump in!",
            "Don't jump in."
        };
    }

    public string Choice(int option)
    {
        switch (option)
        {
            case 0:
                return Jump();
            default:
                return
                    "You're right, this would be a silly idea. I would never want to do that. It wouldn't be fun at all.";
        }
    }

    private string Jump()
    {
        float roll = Random.Range(0f, 1f);
        if (roll < 0.5f)
        {
            // Backwards
            long distanceChange = (long)Random.Range(0, maxDistanceLoss);
            
            FTLMovement.INSTANCE.lightYearsLeftToGo -= distanceChange;
            if (distanceChange >= 10000)
            {
                return
                    $"Well that sucks, we landed {(long)Math.Abs(distanceChange)} light years further back than we started. Oh well";
            }
            if (distanceChange > 0)
            {
                return
                    $"Well, we didn't change MUCH. {(long)Math.Abs(distanceChange)} light years back. We'll get that back in no time.";
            }

        }
        else if(roll > 0.5f)
        {
            //Forward
            long distanceChange = (long)Random.Range(0, maxDistanceGain);
            
            FTLMovement.INSTANCE.lightYearsLeftToGo += distanceChange;

            if (distanceChange >= 10000)
            {
                return $"Awesome! We were catapulted {distanceChange} light years forward. That was real fun!";
            }

            if (distanceChange > 0)
            {
                return
                    $"Well, we didn't change MUCH. {(long)Math.Abs(distanceChange)} light years forward. Better than nothing I guess.";
            }
        }

        return $"Wait, what? We didn't move at all! What a scam.";
    }

    public UnityEvent GetEndEvent()
    {
        return EndEvent;
    }

    public GameObject GameObject()
    {
        return gameObject;
    }
}