using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private const string Enginespeed = "engineSpeed";
    private const string Engineefficiency = "engineEfficiency";
    private const string Fuelrecovery = "fuelRecovery";
    private const string Fueltank = "fuelTank";
    private const string Quarterssize = "quartersSize";
    private const string Clonechance = "cloneChance";
    private const string Cloneproduction = "cloneProduction";
    private const string Foodstorage = "foodStorage";
    private const string Foodproduction = "foodProduction";
    private const string Oxygentank = "oxygenTank";
    private const string Oxygenproduction = "oxygenProduction";
    private const string Materialsstorage = "materialsStorage";
    private const string Eventchance = "eventChance";
    private const string Weirdchance = "weirdChance";
    public static UpgradeManager INSTANCE;

    public class Upgrade
    {
        public string name;
        public int currentLevel = 1;
        public long lastLevelCost = 0;
        public long cost_constant = 50;
        public Action upgradeFunction;
    }

    public Dictionary<string, Upgrade> Upgrades = new()
    {
        {
            Enginespeed,
            new Upgrade()
            {
                name = "Engine Speed", currentLevel = 1, lastLevelCost = 0, cost_constant = 50,
                upgradeFunction = LevelUpEngineSpeed
            }
        },
        {
            Engineefficiency,
            new Upgrade()
            {
                name = "Energy Efficiency", currentLevel = 1, lastLevelCost = 0, cost_constant = 100,
                upgradeFunction = LevelUpEngineEfficiency
            }
        },
        {
            Fuelrecovery,
            new Upgrade()
            {
                name = "Energy Recovery", currentLevel = 1, lastLevelCost = 0, cost_constant = 50,
                upgradeFunction = LevelUpFuelRecovery
            }
        },
        {
            Fueltank,
            new Upgrade()
            {
                name = "Fuel Tank", currentLevel = 1, lastLevelCost = 0, cost_constant = 50,
                upgradeFunction = LevelUpFuelTank
            }
        },
        {
            Quarterssize,
            new Upgrade()
            {
                name = "Clone Quarters Size", currentLevel = 1, lastLevelCost = 0, cost_constant = 100,
                upgradeFunction = LevelUpQuartersSize
            }
        },
        {
            Clonechance,
            new Upgrade()
            {
                name = "Cloning Chance", currentLevel = 1, lastLevelCost = 0, cost_constant = 200,
                upgradeFunction = LevelUpCloneChance
            }
        },
        {
            Cloneproduction,
            new Upgrade()
            {
                name = "Clone Vats", currentLevel = 1, lastLevelCost = 0, cost_constant = 500,
                upgradeFunction = LevelUpCloneProduction
            }
        },
        {
            Foodstorage,
            new Upgrade()
            {
                name = "Food Storage", currentLevel = 1, lastLevelCost = 0, cost_constant = 100,
                upgradeFunction = LevelUpFoodStorage
            }
        },
        {
            Foodproduction,
            new Upgrade()
            {
                name = "Food Production", currentLevel = 1, lastLevelCost = 0, cost_constant = 200,
                upgradeFunction = LevelUpFoodProduction
            }
        },
        {
            Oxygentank,
            new Upgrade()
            {
                name = "Oxygen Tanks", currentLevel = 1, lastLevelCost = 0, cost_constant = 50,
                upgradeFunction = LevelUpOxygenTank
            }
        },
        {
            Oxygenproduction,
            new Upgrade()
            {
                name = "Oxygen Production", currentLevel = 1, lastLevelCost = 0, cost_constant = 50,
                upgradeFunction = LevelUpOxygenProduction
            }
        },
        {
            Materialsstorage,
            new Upgrade()
            {
                name = "Material Storage", currentLevel = 1, lastLevelCost = 0, cost_constant = 50,
                upgradeFunction = LevelUpMaterialsStorage
            }
        },
        {
            Eventchance,
            new Upgrade()
            {
                name = "Scanner Range", currentLevel = 1, lastLevelCost = 0, cost_constant = 200,
                upgradeFunction = LevelUpEventChance
            }
        },
        {
            Weirdchance,
            new Upgrade()
            {
                name = "Weirdness", currentLevel = 1, lastLevelCost = 0, cost_constant = 500,
                upgradeFunction = LevelUpChanceOtherEvents
            }
        }
    };


    public Dictionary<string, string> UpgradeDescriptions = new()
    {
        { Enginespeed, "Increases the speed of your engines, but also increases the amount of energy consumption." },
        { Engineefficiency, "Reduces the amount of energy that your engine uses." },
        { Fuelrecovery, "Increases the speed at which your engine recovers energy." },
        { Fueltank, "Increases the amount of fuel you can store." },
        { Quarterssize, "Increases the amount of clones you can have." },
        { Clonechance, "Increases the daily chance of a cloning going right." },
        { Cloneproduction, "Increases the chance of a clone being created." },
        { Foodstorage, "Increases the amount of food you can store." },
        { Foodproduction, "Increases the amount of food you're producing per day." },
        { Oxygentank, "Increases the amount of oxygen you can store." },
        { Oxygenproduction, "Increases the amount of oxygen you gain per day." },
        { Materialsstorage, "Increases the amount of materials you can store." },
        { Eventchance, "Increases the chance of you encountering events." },
        { Weirdchance, "Increases the chance of you encountering weird events." }
    };

    private void Awake()
    {
        if (INSTANCE != null)
        {
            Destroy(this);
            return;
        }

        INSTANCE = this;
    }

    private static void LevelUpChanceOtherEvents()
    {
        EventSpawnerSystem.INSTANCE.AdditionalWeirdChance += 1;
    }

    private static void LevelUpEventChance()
    {
        EventSpawnerSystem.INSTANCE.MinDays -= 1;
        EventSpawnerSystem.INSTANCE.MaxDays -= 1;
    }

    private static void LevelUpMaterialsStorage()
    {
        Resources.INSTANCE.MaxMaterials += 100 * INSTANCE.Upgrades[Materialsstorage].currentLevel;
    }

    private static void LevelUpOxygenProduction()
    {
        OxygenProducer.INSTANCE.ProductionRate += 15f * INSTANCE.Upgrades[Oxygenproduction].currentLevel;
    }

    private static void LevelUpOxygenTank()
    {
        Resources.INSTANCE.MaxOxygen += 50 * INSTANCE.Upgrades[Oxygentank].currentLevel;
    }

    private static void LevelUpFoodProduction()
    {
        Resources.INSTANCE.FoodProduction += 0.01f * INSTANCE.Upgrades[Foodproduction].currentLevel;
    }

    private static void LevelUpFoodStorage()
    {
        Resources.INSTANCE.MaxFood += 50 * INSTANCE.Upgrades[Foodstorage].currentLevel;
    }

    private static void LevelUpCloneProduction()
    {
        Resources.INSTANCE.CloneMultiplier++;
    }

    private static void LevelUpCloneChance()
    {
        Resources.INSTANCE.CloneChance *= 1.2f;
    }

    private static void LevelUpQuartersSize()
    {
        Resources.INSTANCE.MaxClones += 50 * INSTANCE.Upgrades[Quarterssize].currentLevel;
    }

    private static void LevelUpFuelTank()
    {
        Resources.INSTANCE.MaxEnergy += 50 * INSTANCE.Upgrades[Fueltank].currentLevel;
    }

    private static void LevelUpFuelRecovery()
    {
        FTLMovement.INSTANCE.energyRestoration += 0.5f * INSTANCE.Upgrades[Fuelrecovery].currentLevel;
    }

    private static void LevelUpEngineEfficiency()
    {
        FTLMovement.INSTANCE.energyUsage *= 0.1f;
    }

    static void LevelUpEngineSpeed()
    {
        FTLMovement.INSTANCE.speed += 10 * INSTANCE.Upgrades[Enginespeed].currentLevel;
    }

    public long CalcLevelUpCosts(string type)
    {
        var upgr = INSTANCE.Upgrades[type];
        int currentLevel = upgr.currentLevel;
        return upgr.lastLevelCost + (currentLevel + 1) * upgr.cost_constant;
    }

    public void LevelUpItem(string type)
    {
        if (!Upgrades.ContainsKey(type))
        {
            throw new ArgumentException();
        }

        var levelUpCosts = CalcLevelUpCosts(type);
        Upgrade upgrade = Upgrades[type];
        if (Resources.INSTANCE.Materials < levelUpCosts)
        {
            throw new ArgumentException();
        }

        Resources.INSTANCE.Materials -= levelUpCosts;

        upgrade.currentLevel++;
        upgrade.lastLevelCost = levelUpCosts;
        upgrade.upgradeFunction();
    }
}