using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeEntry : MonoBehaviour
{
    private string type;

    [SerializeField] private TextMeshProUGUI NameText;
    [SerializeField] private TextMeshProUGUI DescriptionText;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private TextMeshProUGUI BuyButtonText;
    [SerializeField] private Button BuyButton;

    public void SetValues(string type)
    {
        this.type = type;
        UpgradeManager.Upgrade upgr = UpgradeManager.INSTANCE.Upgrades[type];
        NameText.text = upgr.name;
        DescriptionText.text = UpgradeManager.INSTANCE.UpgradeDescriptions[type];
        LevelText.text = upgr.currentLevel + "";
    }

    void Update()
    {
        UpgradeManager.Upgrade upgr = UpgradeManager.INSTANCE.Upgrades[type];
        var levelUpCosts = UpgradeManager.INSTANCE.CalcLevelUpCosts(type);
        BuyButton.enabled = levelUpCosts <= (long)Resources.INSTANCE.Materials;
        BuyButton.interactable = levelUpCosts <= (long)Resources.INSTANCE.Materials;
        LevelText.text = upgr.currentLevel + "";
        BuyButtonText.text = levelUpCosts + "";
    }

    public void Buy()
    {
        UpgradeManager.INSTANCE.LevelUpItem(type);
    }
}