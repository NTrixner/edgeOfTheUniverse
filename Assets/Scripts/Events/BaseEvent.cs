using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface BaseEvent
{
    public string GetDescription();
    public string[] GetOptions();
    public string Choice(int option);
    
    public UnityEvent GetEndEvent();

    public GameObject GameObject();
}
