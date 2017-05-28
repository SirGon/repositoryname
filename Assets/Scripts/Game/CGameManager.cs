using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CGameManager : MonoBehaviour
{
    static private CGameManager mInstance;
    public GameObject prefabPlayer;
    //private CGameState mState;
    // private CCamera mCamera; 
    public bool isPaused = false;

    private Vector3 playerOrigin;

    public CCar mPlayer;
    public CCar mPlayer2;

    public CCar MPlayer
    {
        get { return mPlayer; }
    }

    public CCar MPlayer2
    {
        get { return mPlayer2; }
    }

    private float _maxNumberOfLaps = 4;

    void Awake()
    {
        if (mInstance != null)
        {
            throw new UnityException("Error in CGame(). You are not allowed to instantiate it more than once.");
        }

        mInstance = this;
        // mCamera = new CCamera();
        //mPlayer = prefabPlayer.GetComponent<CCar>();
    }

    static public CGameManager inst()
    {
        return mInstance;
    }

    // Use this for initialization
    void Start()
    {
        //mPlayer = Instantiate(prefabPlayer, prefabPlayer.transform.position, Quaternion.Euler(Vector3.zero)).GetComponent<CCar>();
        mPlayer.setPlayerNumber(1);
        mPlayer2.setPlayerNumber(2);    
    }

    // Update is called once per frame
    void Update()
    {

    }

    public CCar GetPlayer()
    {
        return mPlayer;
    }

    public CCar GetPlayer2()
    {
        return mPlayer2;
    }

    public float GetMaxNumberOfLaps()
    {
        return _maxNumberOfLaps;
    }
}