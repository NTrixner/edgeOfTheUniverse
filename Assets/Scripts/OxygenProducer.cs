using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenProducer : MonoBehaviour
{
    public static OxygenProducer INSTANCE { get; private set; }
    public float ProductionRate; //This should produce enough oxygen for 50 clones, so we should average around that
    // Start is called before the first frame update
    

    private void Awake()
    {
        if (INSTANCE != null)
        {
            Destroy(this);
            return;
        }

        INSTANCE = this;
    }
    // Update is called once per frame
    void Update()
    {
    }
}
