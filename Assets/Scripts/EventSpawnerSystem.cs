using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class EventSpawnerSystem : MonoBehaviour
{
    public static EventSpawnerSystem INSTANCE;

    [SerializeField] private TextMeshProUGUI EventDescription;
    
    [SerializeField] private GameObject ActionButtonPrefab;

    [SerializeField] private GameObject ActionButtonPanel;

    [SerializeField] private EventChance[] EventChances;

    [SerializeField] private GameObject Panel;

    private BaseEvent currentEvent;
    private List<GameObject> buttons = new();

    private int daysSinceLastEvent = 0;
    private int daysUntilNextEvent = 10;

    public int MinDays = 5;
    public int MaxDays = 25;

    public int AdditionalWeirdChance = 0;
    private void Awake()
    {
        if (INSTANCE != null)
        {
            Destroy(this);
            return;
        }

        INSTANCE = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        FTLMovement.INSTANCE.CompletelyStoppedTravelling.AddListener(SpawnEvent);
        DayCounter.INSTANCE.DayPassed.AddListener(DayPassed);
    }

    private void DayPassed()
    {
        daysSinceLastEvent++;
    }

    private void SpawnEvent()
    {
        if (daysSinceLastEvent >= daysUntilNextEvent)
        {
            FTLMovement.INSTANCE.Disable();
            DayCounter.INSTANCE.TimeMoving = false;
            Panel.SetActive(true);
            GameObject eventPrefab = GetRandomEvent();
            //Now the question, does Awake get called on Initialize or not...
            GameObject spawnedEvent = Instantiate(eventPrefab);
            currentEvent = GetEventComp(spawnedEvent);
            EventDescription.text = currentEvent.GetDescription();
            string[] choices = currentEvent.GetOptions();
            for (int i = 0; i < choices.Length; i++)
            {
                GameObject newButton = Instantiate(ActionButtonPrefab, ActionButtonPanel.transform, false);
                newButton.GetComponentInChildren<TextMeshProUGUI>().text = choices[i];
                var i1 = i;
                newButton.GetComponent<Button>().onClick.AddListener(() => { ChoiceMade(i1); });
                buttons.Add(newButton);
            }
            currentEvent.GetEndEvent().AddListener(EndEvent);
            daysSinceLastEvent = 0;
            daysUntilNextEvent = Random.Range(MinDays, MaxDays);
        }
    }

    private void ChoiceMade(int i)
    {
        string result = currentEvent.Choice(i);
        EventDescription.text = result;
        EmptyButtons();
        GameObject newButton = Instantiate(ActionButtonPrefab, ActionButtonPanel.transform, false);
        newButton.GetComponentInChildren<TextMeshProUGUI>().text = "Continue";
        newButton.GetComponent<Button>().onClick.AddListener(EndEvent);
        buttons.Add(newButton);
    }
    
    private static BaseEvent GetEventComp(GameObject spawnedEvent)
    {
        Component[] eventComponents = spawnedEvent.GetComponents(typeof(Component));
        foreach (Component comp in eventComponents)
        {
            if (comp is BaseEvent)
                return comp as BaseEvent;
        }

        return null;
    }

    private void EmptyButtons()
    {
        
        foreach (GameObject button in buttons)
        {
            Destroy(button);
        }
    }

    //Love these btw.
    private GameObject GetRandomEvent()
    {
        int totalChance = 0;
        foreach(EventChance chance in EventChances)
        {
            if (!(GetEventComp(chance.EventPrefab) is StellarBodyEvent))
            {
                chance.weight += AdditionalWeirdChance;
            }
            totalChance += chance.weight;
        }

        int roll = Random.Range(1, totalChance);
        foreach (EventChance chance in EventChances)
        {
            if (roll <= chance.weight)
            {
                return chance.EventPrefab;
            }

            roll -= chance.weight;
        }
        return EventChances[^1].EventPrefab;
    }

    public void EndEvent()
    {
        FTLMovement.INSTANCE.Enable();
        DayCounter.INSTANCE.TimeMoving = true;
        EmptyButtons();
        currentEvent.GetEndEvent().RemoveListener(EndEvent);
        Destroy(currentEvent.GameObject());
        Panel.SetActive(false);
    }

    [System.Serializable]
    public class EventChance
    {
        public GameObject EventPrefab;
        public int weight;
    }
}
