using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ClipQueue : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;

    private AudioSource _audioSource;

    private int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.clip = clips[index];
            _audioSource.Play();
            index = (index + 1) % clips.Length;
        }
    }
}
