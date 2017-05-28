using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
//This class control the camera depth of field and the camera Field of view to help the game appears faster
public class CameraFOV : MonoBehaviour
{
    //Main camera to which the effects will apply
    public  Camera cameraTarget;
    //Player from which i need speed values (for now i use drag in the scene TODO: instantiate a player and obtain values by code)
    // public Rigidbody player;

    public Rigidbody _PlayerRb;
    //Value i use to divide the speed and obtain a percentaje of te aperture.

    //----------------------Camera Depth ------------

    public float depthApertureCorrection;
    //Class DepthOfField default of unity
    private DepthOfField cameraDepth;

    private VignetteAndChromaticAberration vignetteAndChromatic;
    //public float blurFloat;
    //public float vignetteIntensity;
    private Fisheye fishEye;

    //----------------------Field of View ------------
    //original Value of field of view
    private float FOV;
    //Value i use to divide the speed and obtain a percentaje of te field of view.
    public float fovValueCorrection;

    private Bloom cameraBloom;
    private float originalBloomThreshold;

    private CCamera mCamera;
    private float originalCameraMinDistance;
    private float cameraCorrectionValue;


    //boolean to shutDown the field of view effect just for see the difference
    private bool doit = true;

    private CCameraShake cameraShakeClass;
    public float cameraShakeValueCorrection;

    public CCar shipTarget;
    // Use this for initialization
    void Start()
    {
        //_PlayerRb = CGameManager.inst().MPlayer.GetComponent<Rigidbody>();
        FOV = cameraTarget.fieldOfView;
        cameraDepth = cameraTarget.GetComponent<DepthOfField>();
        vignetteAndChromatic = cameraTarget.GetComponent<VignetteAndChromaticAberration>();
        cameraShakeClass = cameraTarget.GetComponent<CCameraShake>();
        fishEye = cameraTarget.GetComponent<Fisheye>();

        cameraBloom = cameraTarget.GetComponent<Bloom>();
        originalBloomThreshold = cameraBloom.bloomThreshold;

        depthApertureCorrection = 250; // TO DO: set the best fit to this value
        cameraShakeValueCorrection = 62000 ;

        mCamera = cameraTarget.GetComponent<CCamera>();
        originalCameraMinDistance = mCamera._minDistance;
        cameraCorrectionValue = 20;

        //carScript = GetComponent<CCar>();

    //if()
}

// Update is called once per frame
void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            doit = !doit;
            //cameraShakeValueCorrection = 5000;
        }
        //here i acces if the ship is doing boost TODO: State machine in the ship to set a boost and normal state
        if (shipTarget.isBoosting)
        {
            fishEye.strengthX = Mathf.Lerp(fishEye.strengthX, .4f, .2f);
            fishEye.strengthY = Mathf.Lerp(fishEye.strengthY, .4f, .2f);
        }
        else
        {
            fishEye.strengthX = Mathf.Lerp(fishEye.strengthX, 0, .2f);
            fishEye.strengthY = Mathf.Lerp(fishEye.strengthY, 0, .2f);
        }

    }
    void FixedUpdate()
    {
        //if(doit == true)
        {
            if (doit == true)
            {
                cameraTarget.fieldOfView = Mathf.Lerp(cameraTarget.fieldOfView, FOV + _PlayerRb.velocity.magnitude / fovValueCorrection - (Mathf.Abs(_PlayerRb.velocity.y / fovValueCorrection)), .05f);
                mCamera._minDistance = Mathf.Lerp(mCamera._minDistance, originalCameraMinDistance - Camera.main.fieldOfView / cameraCorrectionValue, .5f);
            }
            vignetteAndChromatic.blur = Mathf.Lerp(vignetteAndChromatic.blur, _PlayerRb.velocity.magnitude / 200 - (Mathf.Abs(_PlayerRb.velocity.y / 200)), .05f);
            vignetteAndChromatic.intensity = Mathf.Lerp(vignetteAndChromatic.intensity, _PlayerRb.velocity.magnitude / 700 - (Mathf.Abs(_PlayerRb.velocity.y / 700)), .05f); ;
            if(doit == true)
            {
                cameraBloom.bloomIntensity = Mathf.Lerp(cameraBloom.bloomIntensity, _PlayerRb.velocity.magnitude / 280 - (Mathf.Abs(_PlayerRb.velocity.y / 280)), .05f); ;
                cameraBloom.bloomThreshold = Mathf.Lerp(originalBloomThreshold, (_PlayerRb.velocity.magnitude / 17 - (Mathf.Abs(_PlayerRb.velocity.y / 17))) * -1, .05f); ;
                //cameraBloom.bloomThresholdColor = Color.yellow;
                cameraBloom.enabled = true;
            }
            else
            {
                cameraBloom.enabled = false;
                cameraBloom.bloomIntensity = 0;
                cameraBloom.bloomThreshold = 0;
            }

            //This set the effect of change the camera depth proportional to the players speed. TODO: do it with LERP and try.
            //cameraDepth.aperture = Mathf.Lerp(cameraDepth.aperture, _PlayerRb.velocity.magnitude / depthApertureCorrection - (Mathf.Abs(_PlayerRb.velocity.y / depthApertureCorrection)), .05f);
            if (_PlayerRb.velocity.magnitude - (Mathf.Abs(_PlayerRb.velocity.y)) >= 145 && doit)
            {
                cameraShakeClass.shakeSize = new Vector2((_PlayerRb.velocity.magnitude / cameraShakeValueCorrection - (Mathf.Abs(_PlayerRb.velocity.y / cameraShakeValueCorrection)) * -1), _PlayerRb.velocity.magnitude / cameraShakeValueCorrection - (Mathf.Abs(_PlayerRb.velocity.y / cameraShakeValueCorrection)));
            }
            else
            {
                cameraShakeClass.shakeSize = Vector2.Lerp(cameraShakeClass.shakeSize, Vector2.zero, .05f);
            }
        }
    }
}
