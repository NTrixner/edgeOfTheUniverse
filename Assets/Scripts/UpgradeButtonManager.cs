using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject UpgradePanel;
    
    public void OnButton()
    {
        UpgradePanel.SetActive(true);
        FTLMovement.INSTANCE.Disable();
        DayCounter.INSTANCE.TimeMoving = false;
    }
}