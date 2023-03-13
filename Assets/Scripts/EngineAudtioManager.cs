using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EngineAudtioManager : MonoBehaviour
{
    private AudioSource _audioSource;

    private float accelerationDuration;
    private float currentAcc = 0f;

    private bool acc = false;

    private bool dec = false;

    // Start is called before the first frame update
    void Start()
    {
        FTLMovement.INSTANCE.StartTravelling.AddListener(StartEngine);
        FTLMovement.INSTANCE.StopTravelling.AddListener(StopEngine);
        _audioSource = GetComponent<AudioSource>();
        accelerationDuration = FTLMovement.INSTANCE.timeToStop;
    }

    private void StartEngine()
    {
        acc = true;
        dec = false;
        _audioSource.Play();
    }

    void StopEngine()
    {
        acc = false;
        dec = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (acc)
        {
            currentAcc += Time.deltaTime;
            _audioSource.volume = (currentAcc / accelerationDuration);
            if (currentAcc >= accelerationDuration)
            {
                currentAcc = 0f;
                acc = false;
            }
        }

        if (dec)
        {
            currentAcc += Time.deltaTime;
            _audioSource.volume = (1 - currentAcc / accelerationDuration);
            if (currentAcc >= accelerationDuration)
            {
                dec = false;
                currentAcc = 0f;
                _audioSource.Stop();
            }
        }
    }
}