using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctest : MonoBehaviour {
    public List<GameObject> _sons;

    private void Awake()
    {
     
    }

    private void OnDrawGizmos()
    {
        if (_sons != null && _sons.Count > 1)
        {
            Vector3 last = _sons[0].transform.position;
            Vector3 next;
            for (int i = 1; i < _sons.Count; i++)
            {
                next = _sons[i].transform.position;
                Debug.Log((last - next).magnitude);
                Gizmos.DrawLine(last, next);
                last = next;
            }
        }
       
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
