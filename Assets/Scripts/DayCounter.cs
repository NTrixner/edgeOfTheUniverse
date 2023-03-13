using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DayCounter : MonoBehaviour
{

    public static DayCounter INSTANCE { get; private set; }
    
    [SerializeField] private TextMeshProUGUI DaysNumber;

    public float DayLength = 5f; // In seconds

    public long DaysPassed = 0;
    
    public bool TimeMoving = true;
    
    private float dayTime = 0;

    public UnityEvent DayPassed;
    private void Awake()
    {
        if (INSTANCE != null)
        {
            Destroy(this);
            return;
        }

        INSTANCE = this;

        DayPassed = new UnityEvent();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeMoving)
        {
            dayTime += Time.deltaTime;
            while (dayTime >= DayLength) // If we're REALLY SLOW this might happen multiple times
            {
                DayPassed.Invoke();
                DaysPassed++;
                dayTime -= DayLength;
                SceneHandler.INSTANCE.DaysPassed = DaysPassed;
            }
        }

        DaysNumber.text = $"{DaysPassed} Days";
        DaysNumber.SetAllDirty();
    }
}
