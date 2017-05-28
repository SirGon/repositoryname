using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameObject : MonoBehaviour {

    public List<Coroutine> _WorkingCoroutines;

    private void Awake()
    {
        _WorkingCoroutines = new List<Coroutine>();
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
