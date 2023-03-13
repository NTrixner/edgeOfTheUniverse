using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FTLMovement : MonoBehaviour
{
    [SerializeField] private bool isFTL;
    [SerializeField] private TextMeshProUGUI FTLButton;
    [SerializeField] private TextMeshProUGUI ETDText;
    public double lightYearsLeftToGo; 
    
    public static FTLMovement INSTANCE { get; private set; }

    public UnityEvent StartTravelling;
    public UnityEvent StopTravelling;
    public UnityEvent CompletelyStoppedTravelling;
    public UnityEvent ReachedTheRestaurant;

    public float energyUsage;
    public float energyRestoration;

    public float speed;

    public float timePassedSinceStop;

    public bool isMoving;

    public float timeToStop = 1.5f;

    private void Awake()
    {
        if (INSTANCE != null)
        {
            Destroy(this);
            return;
        }

        INSTANCE = this;
        StartTravelling = new UnityEvent();
        StopTravelling = new UnityEvent();
        CompletelyStoppedTravelling = new UnityEvent();
        ReachedTheRestaurant = new UnityEvent();
    }

    public void OnTravelButtonClicked()
    {
        if (!isFTL)
        {
            StartTravel();
        }
        else
        {
            StopTravel();
        }
    }

    public void StartTravel()
    {
        isFTL = true;
        isMoving = true;
        FTLButton.text = "Stop";
        StartTravelling.Invoke();
    }

    public void StopTravel()
    {
        isFTL = false;
        FTLButton.text = "Engage!";
        StopTravelling.Invoke();
    }


    private void Update()
    {
        ETDText.text = $"Distance (Lightyears):<br> {(long)lightYearsLeftToGo}";

        if (isMoving && !isFTL)
        {
            timePassedSinceStop += Time.deltaTime;
            if (timePassedSinceStop >= timeToStop)
            {
                isMoving = false;
                CompletelyStoppedTravelling.Invoke();
                timePassedSinceStop = 0f;
            }
        }
        if (isFTL)
        {
            lightYearsLeftToGo -= speed * Time.deltaTime / DayCounter.INSTANCE.DayLength;
            Resources.INSTANCE.Energy = Mathf.Max(0,Resources.INSTANCE.Energy - energyUsage * Time.deltaTime);
            if (Resources.INSTANCE.Energy == 0)
            {
                OnTravelButtonClicked();
            }
        }
        else
        {
            Resources.INSTANCE.Energy = Mathf.Min(Resources.INSTANCE.Energy + energyRestoration * Time.deltaTime, Resources.INSTANCE.MaxEnergy);
        }

        if (lightYearsLeftToGo <= 0)
        {
            lightYearsLeftToGo = 0;
            StopTravel();
            DayCounter.INSTANCE.TimeMoving = false;
            ReachedTheRestaurant.Invoke();
            SceneHandler.INSTANCE.WonGame();
        }
    }

    public void Enable()
    {
        FTLButton.transform.parent.GetComponent<Button>().enabled = true;
    }

    public void Disable()
    {
        FTLButton.transform.parent.GetComponent<Button>().enabled = false;
        
    }
}
