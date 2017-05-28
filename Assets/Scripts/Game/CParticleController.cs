using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CParticleController : MonoBehaviour
    //This class control the different changes in the fire, water, speed related particles
{
    //public List<ParticleSystem> fireParticlesSistem;
    public List<ParticleSystem> waterParticleSistem;
    private ParticleSystem speedLinesParticle;
    public ParticleSystem speedLinesParticle1;
    public ParticleSystem speedLinesParticle2;

    public Rigidbody _playerRb;
    public Rigidbody _player2Rb;
    private CCar _player;
    private CCar _player2;

    //value used to obtain a percentaje of the speed magnitude
    public float balancingValue;

    private ParticleSystem.MainModule mainModuleSpeedLines;
    private ParticleSystem.EmissionModule emissionModuleSpeedLines;

    private CCollisionCheck collisionCheckScript;
    // Use this for initialization
    void Start ()
    {
        _playerRb = CGameManager.inst().MPlayer.GetComponent<Rigidbody>();
        _player2Rb = CGameManager.inst().MPlayer2.GetComponent<Rigidbody>();
        _player = CGameManager.inst().MPlayer.GetComponent<CCar>();
        _player2 = CGameManager.inst().MPlayer2.GetComponent<CCar>();

        balancingValue = 40;

        speedLinesParticle = GetComponent<ParticleSystem>();
        //mainModuleSpeedLines = speedLinesParticle.main;
        //emissionModuleSpeedLines = speedLinesParticle.emission;
        speedLinesParticle1.Stop();
        speedLinesParticle2.Stop();
        collisionCheckScript = GetComponent<CCollisionCheck>();
        //mainModuleSpeedLines = speedLinesParticle.GetComponent<ParticleSystem.MainModule>();
        //this for ensure the particles start stopped
        //for (int i = 0; i < fireParticlesSistem.Count; i++)
        //{
        //    fireParticlesSistem[i].Stop();
        //}

        for (int i = 0; i < waterParticleSistem.Count; i++)
        {
            waterParticleSistem[i].Stop();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
    }
    void FixedUpdate()
    {
        particleEffectsWithVelocity(_player, _playerRb, 1);
        particleEffectsWithVelocity(_player2, _player2Rb, 2);

        //if the ship speed magnitude is more than cero, then the particles start changing directly-proportional to the speed of the ship

    }

    private void particleEffectsWithVelocity(CCar _playerNumber, Rigidbody _playerRigidbody, int PlayerParticleSystemNumber)
    {
        Debug.Log(_playerRigidbody.velocity.magnitude - Mathf.Abs(_playerRigidbody.velocity.y));

        if (PlayerParticleSystemNumber == 1)
        {
            speedLinesParticle = speedLinesParticle1;
        }
        else if(PlayerParticleSystemNumber == 2)
        {
            speedLinesParticle = speedLinesParticle2;
        }
        mainModuleSpeedLines = speedLinesParticle.main;
        emissionModuleSpeedLines = speedLinesParticle.emission;

        if (_player.hasDamage == false)
        {
            speedLinesParticle.startColor = Color.white;
        }
        else
        {
            speedLinesParticle.startColor = Color.red;
        }
        if (_playerRigidbody.velocity.magnitude - Mathf.Abs(_playerRigidbody.velocity.y) > 40)
        {
            if (_playerNumber.hasDamage == false)
            {
                mainModuleSpeedLines.startColor = Color.white;
                speedLinesParticle.Play();
                mainModuleSpeedLines.simulationSpeed = Mathf.Lerp(mainModuleSpeedLines.simulationSpeed, _playerRigidbody.velocity.magnitude / balancingValue - (Mathf.Abs(_playerRigidbody.velocity.y / balancingValue)), .5f);
                emissionModuleSpeedLines.rateOverTime = _playerRigidbody.velocity.magnitude * 15 - (Mathf.Abs(_playerRigidbody.velocity.y * 15));
            }

            else if (_playerNumber.hasDamage == true)
            {
                mainModuleSpeedLines.startColor = Color.red;
                emissionModuleSpeedLines.rateOverTime = _playerRigidbody.velocity.magnitude * 50 - (Mathf.Abs(_playerRigidbody.velocity.y * 50));
                mainModuleSpeedLines.simulationSpeed = Mathf.Lerp(mainModuleSpeedLines.simulationSpeed, _playerRigidbody.velocity.magnitude / balancingValue - (Mathf.Abs(_playerRigidbody.velocity.y / balancingValue)), .5f);
                mainModuleSpeedLines.startLifetime = 5;
            }
            //mainModuleSpeedLines. = _playerRb.velocity.magnitude / balancingValue - (Mathf.Abs(_playerRb.velocity.y / balancingValue));


            #region //for (int i = 1; i < fireParticlesSistem.Count; i++)
            //{
            //    fireParticlesSistem[i].Play();
            //    //TO DO: consultar a mauri como hacer que esto quede bien para las siguientes versiones de unity
            //    fireParticlesSistem[i].startLifetime = _playerRb.velocity.magnitude / balancingValue * .3f - (Mathf.Abs(_playerRb.velocity.y / balancingValue * .3f));
            //    fireParticlesSistem[i].startSize = _playerRb.velocity.magnitude / balancingValue * 2 - (Mathf.Abs(_playerRb.velocity.y / balancingValue * 2));
            //}
            ////i had to separate the central fire from the list to make it bigger an larger than the lateral fires
            //fireParticlesSistem[0].Play();
            //fireParticlesSistem[0].startLifetime = _playerRb.velocity.magnitude / balancingValue * .5f - (Mathf.Abs(_playerRb.velocity.y / balancingValue * .5f));
            //fireParticlesSistem[0].startSize = _playerRb.velocity.magnitude / balancingValue * 3f - (Mathf.Abs(_playerRb.velocity.y / balancingValue * 3f));
            #endregion

            for (int i = 0; i < waterParticleSistem.Count; i++)
            {
                waterParticleSistem[i].Play();
                waterParticleSistem[i].startLifetime = 0.2f + _playerRigidbody.velocity.magnitude / balancingValue * .3f - (Mathf.Abs(_playerRigidbody.velocity.y / balancingValue * .3f));
                //waterParticleSistem[i].startSize = player.velocity.magnitude / balancingValue * 2 - (Mathf.Abs(player.velocity.y / balancingValue * 2));
            }
        }
        else if (_playerRigidbody.velocity.magnitude - Mathf.Abs(_playerRigidbody.velocity.y) <= 40)
        {
            //for (int i = 0; i < fireParticlesSistem.Count; i++)
            //{
            //    fireParticlesSistem[i].Stop();
            //}
            mainModuleSpeedLines.simulationSpeed = Mathf.Lerp(mainModuleSpeedLines.simulationSpeed, 1, .5f);
            emissionModuleSpeedLines.rateOverTime = _playerRigidbody.velocity.magnitude * 15 - (Mathf.Abs(_playerRigidbody.velocity.y * 15));
            speedLinesParticle.Stop();


            for (int i = 0; i < waterParticleSistem.Count; i++)
            {
                waterParticleSistem[i].Stop();
            }
        }
    }
}
