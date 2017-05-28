using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFollowTargert : MonoBehaviour {

    public GameObject _target;
    private Vector3 _origin;

	// Use this for initialization
	void Start () {
        _origin = transform.position;

    }
	
	// Update is called once per frame
	void Update () {

        transform.position = _origin + _target.transform.position;
	}
}
