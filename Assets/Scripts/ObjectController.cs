using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    [System.NonSerialized] public bool isFixed;
    [System.NonSerialized] public bool isSound;    // true: SoundObject, false: EffectObject
    // [System.NonSerialized] public bool isPlayed;

    [System.NonSerialized] public List<GameObject> connectedLines = new List<GameObject>();

    // public float untilPlayConnectedObjs = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void PlaySound()
    {
        // if (!isSound) return;   // EffectObjectの場合実行しない

        // Debug.Log("debuglog: base isPlayed: " + isPlayed);

        // if (connectedLines != null && connectedLines.Count > 0)
        // {
        //     isPlayed = true;
        //     Invoke("ConnectToLines", untilPlayConnectedObjs);
        // }
    }

    // public void ConnectToLines()
    // {
    //     Debug.Log("debuglog: ConnectToLines");
    //     foreach (GameObject line in connectedLines)
    //     {
    //         if (line == null) continue;

    //         EffectLineController lineCtrl = line.GetComponent<EffectLineController>();
    //         lineCtrl.PlayConnectedObjs();
    //         break;
    //     }
    // }

    public virtual void OnTriggerEnter(Collider other) 
    {
        // if (other.gameObject.tag == "EffectLine")
        // {
        //     connectedLines.Add(other.gameObject);
        // }
    }

    public virtual void OnTriggerExit(Collider other) 
    {
        // if (other.gameObject.tag == "EffectLine")
        // {
        //     connectedLines.Remove(other.gameObject);
        // }
    }
}
