using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupScreenManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FTLMovement.INSTANCE.Disable();
        DayCounter.INSTANCE.TimeMoving = false;
    }

    public void OnButton()
    {
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        FTLMovement.INSTANCE.Enable();
        FTLMovement.INSTANCE.StartTravel();
        DayCounter.INSTANCE.TimeMoving = true;
    }
}
