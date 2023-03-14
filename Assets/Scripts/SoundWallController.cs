using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWallController : ObjectController
{
    public AudioSource[] audioSources;
    public GameObject[] audioSourceObjs;
    // public GameObject audioSourceObj0;
    // public GameObject audioSourceObj1;
    // public GameObject audioSourceObj2;
    // public GameObject audioSourceObj3;

    private float dist;

    // Start is called before the first frame update
    void Start()
    {
        isSound = true;
        dist = this.transform.localScale.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PlaySound()
    {
        // if (isPlayed) return;   // すでに再生されていたら実行しない

        // 音の再生開始
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.Play();
        }
        // 音源の移動開始
        StartCoroutine(Move(audioSources[0].clip.length));

        // base.PlaySound();
    }

    private IEnumerator Move(float timeLength)
    {
        float moveTime = 0.0f;
        while (moveTime < timeLength)
        {
            Vector3 goal0 = new Vector3(0.0f, dist, 0.0f);
            Vector3 goal1 = new Vector3(dist, 0.0f, 0.0f);
            Vector3 goal2 = new Vector3(0.0f, -1 * dist, 0.0f);
            Vector3 goal3 = new Vector3(-1 * dist, 0.0f, 0.0f);
            Vector3[] goals = new Vector3[4] {goal0, goal1, goal2, goal3};
            float rate = moveTime / timeLength;

            for (int i = 0; i < 4; i++)
            {
                audioSourceObjs[i].transform.localPosition = Vector3.Lerp(Vector3.zero, goals[i], rate);
            }
            // audioSourceObj0.transform.localPosition = Vector3.Lerp(Vector3.zero, goal0, rate);
            // audioSourceObj1.transform.localPosition = Vector3.Lerp(Vector3.zero, goal1, rate);
            // audioSourceObj2.transform.localPosition = Vector3.Lerp(Vector3.zero, goal2, rate);
            // audioSourceObj3.transform.localPosition = Vector3.Lerp(Vector3.zero, goal3, rate);

            moveTime += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        // 音の再生停止
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.Stop();
        }
        // 音源を元の位置に戻す
        foreach (GameObject audioSourceObj in audioSourceObjs)
        {
            audioSourceObj.transform.localPosition = Vector3.zero;
        }
    }
}
