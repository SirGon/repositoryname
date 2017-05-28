using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPod : MonoBehaviour {

    public LayerMask _layerMask;
    private Rigidbody _rb;

    // Use this for initialization
    void Start() {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {

    }

    void FixedUpdate()
    {
        //encuentro un punto por encima del player, independiente de su rotación
        Vector3 pointAbovePod = transform.position + transform.up.normalized * 5f;


        //casteo un rayo desde por encima del player hacia abajo del player, que ignora al player
        RaycastHit hitInfo;
        Physics.Raycast(pointAbovePod, -transform.up, out hitInfo, 100f, _layerMask);
        Debug.DrawRay(pointAbovePod, -transform.up * hitInfo.distance, Color.red);

        //vector que va desde el player al punto de impacto del raycast
        Vector3 vectorTowardsHit = (hitInfo.point + hitInfo.normal * 2f) - transform.position;
        Debug.DrawRay(transform.position, vectorTowardsHit, Color.blue);


        
        if (hitInfo.collider != null)
        {
           // _rb.position = new Vector3(_rb.position.x, hitInfo.point.y + hitInfo.normal.y * 2f, _rb.position.z);
            _rb.position = hitInfo.point + hitInfo.normal * 2f;
            //_rb.AddForce(vectorTowardsHit * 10);

        }
            
     

        transform.up = hitInfo.normal;
        if (Input.GetKey(KeyCode.J))
        {
            //Debug.Log ("entro 1");
            transform.Rotate(hitInfo.normal * -1f);
        }
        if (Input.GetKey(KeyCode.L))
        {
            //Debug.Log ("entro 1");
            transform.Rotate(hitInfo.normal * +1f);
        }
        if (Input.GetKey(KeyCode.I))
        {
            //Debug.Log ("entro 1");
            //velocity += acceleration;
            _rb.AddForce(transform.forward * 10);
        }
    }
}
