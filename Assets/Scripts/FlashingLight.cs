using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FlashingLight : MonoBehaviour
{
    [SerializeField] private float speed;

    private float currentTime;

    private int dir = 1;
    private Image _image;
    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_image.enabled)
        {
            
            currentTime += dir * Time.deltaTime;
        
            var imageColor = _image.color;
            imageColor.a = currentTime / speed;
            _image.color = imageColor;

            if (currentTime > speed || currentTime < 0)
            {
                dir *= -1;
            }
        }
    }

    public void StartFlashing()
    {
        _image.enabled = true;
    }

    public void StopFlashing()
    {
        _image.enabled = false;
    }
}
