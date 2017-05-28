﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class CCamera : MonoBehaviour {

	Camera _cameraComponent; 
	public Rigidbody _targetRb;
	public Vector3 _lookOffsetPosition;
	public Vector3 _lookOffsetRotation;

	Rigidbody _rb;
	Transform _parent;
	public float _maxDistance;
	public float _minDistance;
    Vector3 _startCameraOffset;
    Transform _rotationPivot;
    public float _turnSpeed;
    public float _followSpeed;
    public int m_SpinTurnLimit = 90;
    private float currentFlatAngle;
    private float m_LastFlatAngle;
    private float m_CurrentTurnAmount;
    private float m_TurnSpeedVelocityChange;

    private void Awake()
	{
        //_startCameraOffset = _targetRb.position - transform.position;
		_cameraComponent = GetComponent<Camera>();
        _rotationPivot =_targetRb.transform.GetChild(0);

    }

	// Use this for initialization
	void Start () {

	}



	
	void FixedUpdate ()
    {
        
        var targetForward = _rotationPivot.forward;
        currentFlatAngle = Mathf.Atan2(targetForward.x, targetForward.z) * Mathf.Rad2Deg;
        if (m_SpinTurnLimit > 0)
        {
            var targetSpinSpeed = Mathf.Abs(Mathf.DeltaAngle(m_LastFlatAngle, currentFlatAngle)) / Time.deltaTime;
            var desiredTurnAmount = Mathf.InverseLerp(m_SpinTurnLimit, m_SpinTurnLimit * 0.75f, targetSpinSpeed);
            var turnReactSpeed = (m_CurrentTurnAmount > desiredTurnAmount ? .1f : 1f);
            if (Application.isPlaying)
            {
                m_CurrentTurnAmount = Mathf.SmoothDamp(m_CurrentTurnAmount, desiredTurnAmount,
                                                     ref m_TurnSpeedVelocityChange, turnReactSpeed);
            }
            else
            {
                // for editor mode, smoothdamp won't work because it uses deltaTime internally
                m_CurrentTurnAmount = desiredTurnAmount;
            }
        }
        else
        {
            m_CurrentTurnAmount = 1;
        }
        m_LastFlatAngle = currentFlatAngle;
        Debug.DrawRay(_targetRb.transform.position, targetForward * 10);
        //Debug.Log(_targetRb.transform.GetChild(0).rotation.eulerAngles);
        var rollRotation = Quaternion.LookRotation(targetForward, _rotationPivot.up);
        //Debug.Log(m_CurrentTurnAmount);
        transform.rotation = Quaternion.Slerp(transform.rotation, rollRotation, _turnSpeed * m_CurrentTurnAmount * Time.deltaTime);
        //transform.rotation = Quaternion.Slerp(transform.rotation, _rotationPivot.transform.rotation, Time.time * _turnSpeed * m_CurrentTurnAmount);
        transform.position = Vector3.Lerp(transform.position, _targetRb.transform.position - targetForward * _minDistance, 0.8f);
        Vector3 offset = _rotationPivot.up * _lookOffsetPosition.y + _rotationPivot.forward * _lookOffsetPosition.z + _rotationPivot.right * _lookOffsetPosition.x;
        transform.position += offset;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + _lookOffsetRotation);
    }

	private void LateUpdate()
	{
		//Vector3 toTarget = _targetRb.position - transform.position;
		//if ((toTarget).magnitude < _minDistance)
		//{
		//	Debug.Log("Me pase");
		//	_rb.velocity = Vector3.zero;
		//	_rb.velocity = _targetRb.velocity * (toTarget).magnitude/_minDistance;
		//}
	}

}

