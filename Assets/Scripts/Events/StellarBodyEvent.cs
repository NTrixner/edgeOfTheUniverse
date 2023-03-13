using UnityEngine;
using UnityEngine.Events;

public class StellarBodyEvent : MonoBehaviour, BaseEvent
{
    public string type;
    public string name;
    public float chance;
    
    private static string[] AsteroidPrefixes = new[] { "XY - ", "ST", "AST-", "Betty", "" };

    private static string[] AsteroidPostfixes = new[] { "KD", "AA", "AB", "AC", "TM", "CCC" };

    private static string[] consonants = new[]
    {
        "q", "w", "r", "t", "z", "k", "p", "s", "d", "f", "g", "j", "h", "j", "l", "x", "c", "v", "b", "n", "m"
    };

    private static string[] vowals = new[]
    {
        "e", "u", "o", "a", "y", "i"
    };

    private UnityEvent EndEvent;
    // Start is called before the first frame update
    void Awake()
    {
        EndEvent = new UnityEvent();
        type = Random.Range(0f, 1f) <= 0.5f ? "Asteroid" : "Planet";
        name = GetStellarBodyName(type);
        name = char.ToUpper(name[0]) + name.Substring(1);
        chance = Random.Range(0.15f, 0.5f);
    }

    public string GetDescription()
    {
        return
            $"We have encountered a{GetVowelNIfNeeded(type)} {type}! Our scientists have named it {name}. We could send some clones down to inspect it. Would you like to send them? <br>"
            + $"The more we send, the more resources we might gather. But if they fail, there's a {(int)(chance * 100f)}% chance that half of them die.";
    }

    private string GetVowelNIfNeeded(string s)
    {
        return s == "Asteroid" ? "n" : "";
    }

    private string GetStellarBodyName(string s)
    {
        switch (s)
        {
            case "Asteroid": return GetAsteroidName();
            case "Planet": return GetPlanetName();
            default: return "Steven";
        }
    }

    private string GetPlanetName()
    {
        int syllables = Random.Range(2, 10);
        string name = "";
        for (int i = 0; i < syllables; i++)
        {
            name += $"{consonants[Random.Range(0, consonants.Length - 1)]}{vowals[Random.Range(0, vowals.Length - 1)]}";

        }
        return name;
    }

    private string GetAsteroidName()
    {
        int number = (int)Random.Range(0f, 9999);
        bool postFix = Random.Range(0f, 1f) <= 0.5f;
        if (postFix)
        {
            return $"{number} {AsteroidPostfixes[Random.Range(0, AsteroidPostfixes.Length - 1)]}";
        }
        else
        {
            return $"{AsteroidPrefixes[Random.Range(0, AsteroidPrefixes.Length - 1)]}{number}";
        }
    }

    public string[] GetOptions()
    {
        return new[]
        {
            $"Send in {(int)(Resources.INSTANCE.Clones * 0.10f)} Clones.",
            $"Send in {(int)(Resources.INSTANCE.Clones * 0.25f)} Clones.",
            $"Send in {(int)(Resources.INSTANCE.Clones * 0.50f)} Clones.",
            $"Leave it be."
        };
    }

    public string Choice(int option)
    {
        switch (option)
        {
            case 0:
                return  SendInClones(0.10f);
            case 1:
                return SendInClones(0.25f);
            case 2:
                return SendInClones(0.50f);
            default:
                EndEvent.Invoke();
                Destroy(gameObject);
                return "";
        }
    }

    private string SendInClones(float p0)
    {
        int clonesSent = (int)(Resources.INSTANCE.Clones * p0);
        float deathRoll = Random.Range(0f, 1f);
        if (deathRoll <= chance)
        {
            int clonesDead = (int)(clonesSent / 2f);
            if (Resources.INSTANCE.Clones <= clonesDead)
            {
                clonesDead = Resources.INSTANCE.Clones;
            }

            Resources.INSTANCE.Clones -= clonesDead;
            return $"Oh no! The ground mission was a disaster. A total of {clonesDead} died";
        }
        else //if(deathRoll < 1) TODO: Random Upgrade on a roll of 1
        {
            if (type == "Planet")
            {
                float amountFoodFound = Random.Range(0.1f, 0.5f) * clonesSent * 10f;
                if (amountFoodFound > Resources.INSTANCE.MaxFood - Resources.INSTANCE.Food)
                {
                    amountFoodFound = Resources.INSTANCE.MaxFood - Resources.INSTANCE.Food;
                }

                Resources.INSTANCE.Food += amountFoodFound;
                float amountMatsFound = Random.Range(01f, 0.5f) * clonesSent * 5f;
                if (amountMatsFound > Resources.INSTANCE.MaxMaterials - Resources.INSTANCE.Materials)
                {
                    amountMatsFound = Resources.INSTANCE.MaxMaterials - Resources.INSTANCE.Materials;
                }

                Resources.INSTANCE.Materials += amountMatsFound;

                Resources.INSTANCE.Oxygen = Resources.INSTANCE.MaxOxygen;
                return $"The mission was a success! We were able to gather a total of {(int)amountFoodFound} Food Items and {(int)amountMatsFound} Materials. <br> We also filled our Oxygen tanks.";
            }
            else
            {
                
                float amountMatsFound = Random.Range(0.1f, 0.5f) * clonesSent * 8f;
                if (amountMatsFound > Resources.INSTANCE.MaxMaterials - Resources.INSTANCE.Materials)
                {
                    amountMatsFound = Resources.INSTANCE.MaxMaterials - Resources.INSTANCE.Materials;
                }

                Resources.INSTANCE.Materials += amountMatsFound;
                return $"The mission was a success! We were able to gather a total of {(int)amountMatsFound} Materials";
            }

        }
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
