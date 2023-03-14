using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFloorController : ObjectController
{
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        isSound = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PlaySound()
    {
        // if (isPlayed) return;   // すでに再生されていたら実行しない

        audioSource.Play();

        // base.PlaySound();
    }

    public override void OnTriggerExit(Collider other) 
    {
        // base.OnTriggerExit(other);

        if (other.gameObject.tag == "MainCamera")
        {
            audioSource.Stop();
        }
    }
}
