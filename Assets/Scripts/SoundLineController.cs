using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLineController : ObjectController
{
    public GameObject audioSourceObj;
    public AudioSource audioSource;

    [System.NonSerialized] public Vector3 startPos;
    [System.NonSerialized] public Vector3 endPos;

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
        StartCoroutine(Move(audioSource.clip.length));

        // base.PlaySound();
    }

    private IEnumerator Move(float timeLength)
    {
        float moveTime = 0.0f;
        while (moveTime < timeLength)
        {
            float rate = moveTime / timeLength;
            audioSourceObj.transform.position = Vector3.Lerp(startPos, endPos, rate);
            moveTime += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        audioSource.Stop();
        audioSourceObj.transform.position = startPos;
    }
}
