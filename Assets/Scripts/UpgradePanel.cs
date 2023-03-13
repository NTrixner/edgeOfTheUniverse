using System;
using TMPro;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private GameObject UpgradeEntryPrefab;
    [SerializeField] private GameObject MasterPanel;
    [SerializeField] private TextMeshProUGUI MaterialText;

    public void OnExitClicked()
    {
        FTLMovement.INSTANCE.Enable();
        DayCounter.INSTANCE.TimeMoving = true;
        MasterPanel.SetActive(false);
        Debug.Log("Exit clicked");
    }

    private void Awake()
    {
        foreach (string type in UpgradeManager.INSTANCE.Upgrades.Keys)
        {
            GameObject newEntry = Instantiate(UpgradeEntryPrefab, transform, false);
            newEntry.GetComponent<UpgradeEntry>().SetValues(type);
        }
    }

    private void Update()
    {
        MaterialText.text = (int)Resources.INSTANCE.Materials + "";
    }
}