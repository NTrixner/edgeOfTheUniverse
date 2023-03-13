using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class MenuTextHandler : MonoBehaviour
{
    [SerializeField] private bool isSuccess;

    private TextMeshProUGUI _textMeshProUGUI;
    private void Awake()
    {
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        if (isSuccess)
        {
            _textMeshProUGUI.text =
                $"You have successfully reached the Restaurant at the edge of the Universe. It took you {SceneHandler.INSTANCE.DaysPassed} days to reach it.<br> I hope you're hungry.";
        }
        else
        {
            _textMeshProUGUI.text =
                $"You have succumbed to the dangers of the Universe. You were able to survive the endlessness of space for {SceneHandler.INSTANCE.DaysPassed} days.<br> Your journey is over.";
        }
    }
    
}
