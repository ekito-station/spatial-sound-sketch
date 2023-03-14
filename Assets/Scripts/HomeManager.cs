using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HomeManager : MonoBehaviour
{
    [System.NonSerialized] public int modeNum = 0;    // 0: SoundMode, 1: EffectMode, 2: EraseMode

    private float pressTime;
    public float pressTh = 1.5f;
    private bool placingObj;
    private bool isWall;
    public float spreadCoef = 5.0f;
    private float size;
    public float floorDepth = 1.2f;
    public float wallThickness = 0.1f;
    public GameObject audioSources;
    public GameObject audioSource0;
    public GameObject audioSource1;
    public GameObject audioSource2;
    public GameObject audioSource3;

    private int tapCount;
    private bool isDoubleTap;
    // private bool isDoubleTapStarted;
    // private float doubleTapTime;
    public float doubleTapTh = 0.5f;

    private float fingerPosX0;
    private float fingerPosX1;
    public float posTh = 50.0f;

    public TextMeshProUGUI modeText;

    public AudioSource audioSource;
    public AudioClip soundVoice;
    public AudioClip effectVoice;
    public AudioClip eraseVoice;
    public AudioClip eraseSound;

    private bool drawingLine;

    public float front = 0.5f;

    private Vector3 prePinPos;

    public GameObject pin;
    public GameObject soundBall;
    public GameObject soundLine;
    public GameObject effectLine;
    public GameObject soundWall;
    public GameObject effectWall;
    public GameObject soundFloor;
    public GameObject effectFloor;

    private GameObject line;
    private GameObject wall;
    private GameObject floor;

    public float lineRadius = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        // Debug.Log("Tilt: " + Mathf.Asin(Mathf.Clamp(Input.acceleration.z, -1, 1)) * Mathf.Rad2Deg);

        if (Input.GetMouseButtonDown(0))
        {
            fingerPosX0 = Input.mousePosition.x;    // 指が触れた位置を取得
        }

        if (Input.GetMouseButton(0))
        {
            pressTime += Time.deltaTime;
            if (pressTime > pressTh)
            {
                if (!placingObj)
                {
                    float angle = Mathf.Asin(Mathf.Clamp(Input.acceleration.z, -1, 1)) * Mathf.Rad2Deg;
                    isWall = (angle > -45.0f) ? true : false;

                    audioSources.SetActive(true);
                    audioSources.transform.localPosition = isWall ? new Vector3(0.0f, 0.0f, front) : new Vector3(0.0f, -1 * floorDepth, front);

                    placingObj = true;
                }
                else
                {
                    // 長く押すほど音が広がる
                    size = pressTime / spreadCoef;
                    if (isWall)
                    {
                        audioSource0.transform.localPosition = new Vector3(0.0f, size, 0.0f);
                        audioSource1.transform.localPosition = new Vector3(size, 0.0f, 0.0f);
                        audioSource2.transform.localPosition = new Vector3(0.0f, -1 * size, 0.0f);
                        audioSource3.transform.localPosition = new Vector3(-1 * size, 0.0f, 0.0f);
                    }
                    else
                    {
                        audioSource0.transform.localPosition = new Vector3(0.0f, 0.0f, size);
                        audioSource1.transform.localPosition = new Vector3(size, 0.0f, 0.0f);
                        audioSource2.transform.localPosition = new Vector3(0.0f, 0.0f, -1 * size);
                        audioSource3.transform.localPosition = new Vector3(-1 * size, 0.0f, 0.0f);
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (placingObj)
            {
                size = pressTime / spreadCoef;

                if (isWall) // Wallを配置
                {
                    Vector3 objPos = CalcObjPos(false);
                    PlaceWall(objPos, size);
                }
                else        // Floorを配置
                {
                    Vector3 objPos = CalcObjPos(true);
                    PlaceFloor(objPos, size);
                }

                placingObj = false;
                pressTime = 0;
                audioSources.SetActive(false);
                return;
            }

            fingerPosX1 = Input.mousePosition.x;    // 指が離れた位置を取得
            // モード切り替え
            float diff = fingerPosX1 - fingerPosX0;
            if (Mathf.Abs(diff) > posTh)
            {
                if (diff > 0) modeNum--;
                else modeNum++;

                if (modeNum == -1) modeNum = 2;
                else if (modeNum == 3) modeNum = 0;            

                switch (modeNum)
                {
                    case 0:
                        modeText.text = "Sound";
                        audioSource.PlayOneShot(soundVoice);
                        break;
                    case 1:
                        modeText.text = "Effect";
                        audioSource.PlayOneShot(effectVoice);
                        break;
                    case 2:
                        modeText.text = "Erase";
                        audioSource.PlayOneShot(eraseVoice);
                        break;
                }
            }
            else
            {
                tapCount++;
                Invoke("DoubleTap", doubleTapTh);
            }
        }        
    }

    private Vector3 CalcObjPos(bool _isFloor)
    {
        Vector3 _objPos;
        Transform _camTrans = this.transform;
        _objPos = _isFloor ? (_camTrans.position) : (_camTrans.position + front * _camTrans.forward);
        return _objPos;
    }

    private void DoubleTap()
    {
        // Transform camTrans = this.transform;
        // Vector3 objPos = camTrans.position + front * camTrans.forward;
        Vector3 objPos = CalcObjPos(false);

        if (tapCount < 2)   // シングルタップ
        {
            if (!isDoubleTap)
            {
                // Lineを引く
                if (!drawingLine)
                {
                    Instantiate(pin, objPos, Quaternion.identity);
                    prePinPos = objPos;
                    drawingLine = true;
                    Debug.Log("drawingLine: " + drawingLine);
                }
                else
                {
                    DrawLine(objPos);
                    Debug.Log("drawingLine: " + drawingLine);
                } 
            }
            isDoubleTap = false;
        }
        else    // ダブルタップ
        {
            isDoubleTap = true;
            if (drawingLine)
            {
                // Lineを引くのを終える
                DrawLine(objPos);
                drawingLine = false;
                Debug.Log("drawingLine: " + drawingLine);
            }
            else
            {
                // Ballを置く
                if (modeNum == 0) Instantiate(soundBall, objPos, Quaternion.identity);
                Debug.Log("drawingLine: " + drawingLine);
            }
        }
        tapCount = 0;
    }

    private void DrawLine(Vector3 _objPos)
    {
        // ピンを置く
        Instantiate(pin, _objPos, Quaternion.identity);
        // Lineの中点を取得
        Vector3 lineVec = _objPos - prePinPos;  // Lineの方向を取得
        float dist = lineVec.magnitude;         // Lineの長さを取得
        Vector3 lineY = new Vector3(0f, dist, 0f);
        Vector3 halfLineVec = lineVec * 0.5f;
        Vector3 centerCoord = prePinPos + halfLineVec;
        // Lineを引く
        if (modeNum == 0)   // SoundLineを引く
        {
            line = Instantiate(soundLine, centerCoord, Quaternion.identity);
            Transform lineTrans = line.transform;
            // AudioSourceObjを始点に移動
            GameObject audioSourceObj = lineTrans.GetChild(0).gameObject;
            audioSourceObj.transform.position = prePinPos;
            // 始点と終点を記録
            SoundLineController soundLineCtrl = line.GetComponent<SoundLineController>();
            soundLineCtrl.startPos = prePinPos;
            soundLineCtrl.endPos = _objPos;
        }
        else if (modeNum == 1)  // EffectLineを引く
        {
            line = Instantiate(effectLine, centerCoord, Quaternion.identity);           
        }
        line.transform.rotation = Quaternion.FromToRotation(lineY, lineVec);        // 方向を調整
        line.transform.localScale = new Vector3(lineRadius, dist/2, lineRadius);    // 長さを調整 
        // PrePinPosを更新
        prePinPos = _objPos;
    }

    private void PlaceWall(Vector3 _objPos, float _size)
    {
        if (modeNum == 0)   // SoundWallを置く
        {
            wall = Instantiate(soundWall, _objPos, this.transform.rotation);
        }
        else if (modeNum == 1)  // EffectWallを置く
        {
            wall = Instantiate(effectWall, _objPos, this.transform.rotation);
        }
        wall.transform.localScale = new Vector3(2 * _size, 2 * _size, wallThickness);   // 大きさを調整
    }

    private void PlaceFloor(Vector3 _objPos, float _size)
    {
        if (modeNum == 0)   // SoundFloorを置く
        {
            floor = Instantiate(soundFloor, _objPos, Quaternion.identity);
        }
        else if (modeNum == 1)  // EffectFloorを置く
        {
            floor = Instantiate(effectFloor, _objPos, Quaternion.identity);
        }
        floor.transform.localScale = new Vector3(2 * _size, floorDepth, 2 * _size);   // 大きさを調整
    }

    private void OnTriggerEnter(Collider other) 
    {
        GameObject _targetObj = other.gameObject;
        ObjectController _objCntl = _targetObj.GetComponent<ObjectController>();

        if (modeNum == 2)   // EraseModeのとき削除
        {
            if (!_objCntl.isFixed)
            {
                audioSource.PlayOneShot(eraseSound);
                Destroy(_targetObj);
            }
        }
        else    // その他のモードのときSoundObjectなら音を鳴らす
        {
            _objCntl.PlaySound();
        }        
    }
}
