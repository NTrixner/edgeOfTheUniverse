using UnityEngine;
using UnityEngine.Events;

public class ParasiteEvent : MonoBehaviour, BaseEvent
{
    public float destructionAmount;

    public UnityEvent EndEvent;

    // Start is called before the first frame update
    void Awake()
    {
        destructionAmount = Random.Range(0.5f, 0.95f);
        EndEvent = new UnityEvent();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public string GetDescription()
    {
        return
            $"A shape-shifting parasite has infiltrated the crew! You might be able to get rid of it by killing off parts of the crew, but if you don't get it, it could kill up to {(int)(destructionAmount * 100)}% of the crew.";
    }

    public string[] GetOptions()
    {
        return new[]
        {
            $"Kill {(int)(Resources.INSTANCE.Clones * 0.1f)} clones (10%)",
            $"Kill {(int)(Resources.INSTANCE.Clones * 0.5f)} clones (50%)",
            $"Kill {(int)(Resources.INSTANCE.Clones * 0.75f)} clones (75%)",
            "Do nothing and hope for the best."
        };
    }

    public string Choice(int option)
    {
        switch (option)
        {
            case 0:
                return Kill(0.1f);
            case 1:
                return Kill(0.5f);
            case 2:
                return Kill(0.75f);
            default:
                return Kill(0);
        }
    }

    private string Kill(float killed)
    {
        float roll = Random.Range(0f, 1f);
        if (killed == 0f)
        {
            if (roll <= 0.05f)
            {
                return
                    "We were really lucky! Thankfully, one of our clones was able to kill the parasite without any further issues.";
            }

            int clonesKilledByparasite = (int)(Resources.INSTANCE.Clones * destructionAmount);
            Resources.INSTANCE.Clones -= clonesKilledByparasite;
            return $"The parasite was able to kill {clonesKilledByparasite} clones before we could finally put it down.";
        }

        int clonesToKill = (int)(Resources.INSTANCE.Clones * killed);
        Resources.INSTANCE.Clones -= clonesToKill;
        if (roll >= killed)
        {
            int clonesKilledByparasite = (int)(Resources.INSTANCE.Clones * destructionAmount);
            Resources.INSTANCE.Clones -= clonesKilledByparasite;
            return
                $"Sadly, our attempts were futile. While we culled the clones by {clonesToKill} clones, the parasite was able to take out {clonesKilledByparasite} clones before we could put it down.";
        }

        return
            $"The sacrifice of our {clonesToKill} clones was fruitful. One of them was the parasite. A tough decision, but we had to make it.";
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