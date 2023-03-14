using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFloorController : ObjectController
{
    public AudioReverbZone reverbZone;

    // Start is called before the first frame update
    void Start()
    {
        isSound = false;
        float radius = this.transform.localScale.x / 2;
        reverbZone.maxDistance = radius;
        reverbZone.minDistance = radius;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
