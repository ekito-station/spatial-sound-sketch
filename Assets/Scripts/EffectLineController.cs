using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectLineController : ObjectController
{
    [System.NonSerialized] public List<GameObject> connectedObjs = new List<GameObject>();

    public float untilNextSound = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        isSound = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PlaySound()
    {
        if (connectedObjs != null && connectedObjs.Count > 0)
        {
            StartCoroutine("PlaySounds");
        }
    }

    // public void PlayConnectedObjs()
    // {
    //     Debug.Log("debuglog: PlayConnectedObjs");
    //     if (connectedObjs != null && connectedObjs.Count > 0)
    //     {
    //         Debug.Log("debuglog: " + connectedObjs);
    //         int count = 0;
    //         foreach (GameObject obj in connectedObjs)
    //         {
    //             if (obj == null) continue;  
    //             Debug.Log("debuglog: " + obj);
    //             if (obj.tag == "MainCamera") continue;

    //             ObjectController objCtrl = obj.GetComponent<ObjectController>();
    //             if (!objCtrl.isSound) continue;
    //             if (!objCtrl.isPlayed) count++;
    //             Debug.Log("debuglog: Checked isPlayed: " + objCtrl.isPlayed);
    //         }

    //         Debug.Log("debuglog: count: " + count);
    //         if (count == 0) // 繋がれたオブジェクトが全て再生済みの場合
    //         {
    //             foreach (GameObject obj in connectedObjs)
    //             {
    //                 if (obj == null) continue;
    //                 if (obj.tag == "MainCamera") continue;

    //                 ObjectController objCtrl = obj.GetComponent<ObjectController>();
    //                 objCtrl.isPlayed = false;
    //             }
    //         }
    //         else
    //         {
    //             foreach (GameObject obj in connectedObjs)
    //             {
    //                 if (obj == null) continue;
    //                 if (obj.tag == "MainCamera") continue;

    //                 ObjectController objCtrl = obj.GetComponent<ObjectController>();
    //                 objCtrl.PlaySound();
    //             }
    //         }
    //     }
    // }

    private IEnumerator PlaySounds()
    {
        Debug.Log("debuglog: PlaySounds");
        foreach (GameObject obj in connectedObjs)
        {
            Debug.Log("debuglog: " + obj);
            if (obj == null) continue;
            if (obj.tag == "MainCamera") continue;

            ObjectController objCtrl = obj.GetComponent<ObjectController>();
            objCtrl.PlaySound();
            Debug.Log("debuglog: Played sound.");

            yield return new WaitForSeconds(untilNextSound);
        }
    }

    public override void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "MainCamera") return;
        connectedObjs.Add(other.gameObject);
    }

    public override void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.tag == "MainCamera") return;
        connectedObjs.Remove(other.gameObject);
    }
}
