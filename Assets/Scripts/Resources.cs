using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Resources : MonoBehaviour
{
    public static Resources INSTANCE { get; private set; }

    public int Clones = 50;

    public int MaxClones = 100;

    public float Food = 50;

    public float MaxFood = 100;

    public float Materials = 0;

    public float MaxMaterials = 100;

    public float Oxygen = 100;

    public float MaxOxygen = 100;

    public float Energy = 100;

    public float MaxEnergy = 100;

    public float CloneChance = 0.5f;

    public bool cloning = true;

    public int CloneMultiplier = 1;

    public float FoodProduction = 0.05f;


    [SerializeField] private TextMeshProUGUI ClonesNumber;
    [SerializeField] private TextMeshProUGUI FoodNumber;
    [SerializeField] private TextMeshProUGUI MaterialsNumber;
    [SerializeField] private TextMeshProUGUI OxygenNumber;
    [SerializeField] private TextMeshProUGUI EnergyNumber;
    [SerializeField] private Slider EnergySlider;
    [SerializeField] private FlashingLight FoodWarning;
    [SerializeField] private FlashingLight OxygenWarning;
    [SerializeField] private AudioSource BeepingSource;


    private bool foodWarningPlayedSound;
    private bool oxygenWarningPlayedSound;
    private void Awake()
    {
        if (INSTANCE != null)
        {
            Destroy(this);
            return;
        }

        INSTANCE = this;
    }

    private void Start()
    {
        DayCounter.INSTANCE.DayPassed.AddListener(TrackDays);
    }

    // Update is called once per frame
    void Update()
    {
        ClonesNumber.text = $"{Clones}/{MaxClones}";
        ClonesNumber.SetAllDirty();
        FoodNumber.text = $"{(int)Food}/{(int)MaxFood}";
        FoodNumber.SetAllDirty();
        MaterialsNumber.text = $"{(int)Materials}/{(int)MaxMaterials}";
        MaterialsNumber.SetAllDirty();
        OxygenNumber.text = $"{(int)Oxygen}/{(int)MaxOxygen}";
        OxygenNumber.SetAllDirty();
        EnergyNumber.text = $"{(int)Energy}/{(int)MaxEnergy}";
        EnergyNumber.SetAllDirty();
        EnergySlider.value = Energy / (float)MaxEnergy;
        CheckIfNoClonesLeft();
    }

    void TrackDays()
    {
        ProduceFoodDaily();
        ReduceFoodDaily();
        CalcCloneChanceDaily();
        ProduceOxygen();
        ReduceOxygen();
        CheckIfWarning();
    }

    private void ProduceFoodDaily()
    {
        Food = Math.Min(Food + FoodProduction, MaxFood);
    }

    private void CheckIfWarning()
    {
        if(Food <= MaxFood * 0.1f)
        {
            FoodWarning.StartFlashing();
        }
        else
        {
            FoodWarning.StopFlashing();
        }

        if (Oxygen <= MaxOxygen * 0.1f)
        {
            OxygenWarning.StartFlashing();
        }
        else
        {
            OxygenWarning.StopFlashing();
        }

        if ((Food <= MaxFood * 0.1f && !foodWarningPlayedSound) || (Oxygen <= MaxOxygen * 0.1f && !oxygenWarningPlayedSound))
        {
            BeepingSource.Play();
            foodWarningPlayedSound = true;
            oxygenWarningPlayedSound = true;
        }
        else if(foodWarningPlayedSound && oxygenWarningPlayedSound)
        {
            foodWarningPlayedSound = false;
            oxygenWarningPlayedSound = false;
            BeepingSource.Stop();
        }
    }

    private void CheckIfNoClonesLeft()
    {
        if (Clones == 0)
        {
            DayCounter.INSTANCE.TimeMoving = false;
            FTLMovement.INSTANCE.StopTravel();
            SceneHandler.INSTANCE.LostGame();
        }
    }

    private void ProduceOxygen()
    {
        Oxygen = Mathf.Min(MaxOxygen, Oxygen + OxygenProducer.INSTANCE.ProductionRate);
    }

    private void ReduceOxygen()
    {
        //A person needs 550kg of oxygen per day
        Oxygen = Math.Max(0f, Oxygen -= Clones * 0.55f);
        if (Oxygen == 0)
        {
            Clones = Mathf.FloorToInt(Clones * 0.5f); //everyday without oxygen, 50% of clones die
        }
    }

    private void CalcCloneChanceDaily()
    {
        if (cloning)
        {
            //Every day, there's a certain chance that we produce a new clone
            float roll = Random.Range(0f, 1f);
            if (roll <= CloneChance)
            {
                Clones = Math.Min(MaxClones, Clones + CloneMultiplier);
            }
        }
    }

    private void ReduceFoodDaily()
    {
        //1 Food = 1 Ton of Wheat, which has 340 Cal per 100g, and one clone needs 2800 Cal per day
        Food = Mathf.Max(0f, Food - Clones / 1250f);
        if (Food == 0)
        {
            Clones = Mathf.FloorToInt(Clones * 0.8f); //everyday without food, 20% of clones die
        }
    }

    public void ToggleCloning()
    {
        cloning = !cloning;
    }

}