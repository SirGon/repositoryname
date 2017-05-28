using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCollisionCheck : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        transform.parent.parent.parent.GetComponent<CCar>().Collision(true);
        Debug.Log("col");
    }

    void OnCollisionStay(Collision col)
    {
        transform.parent.parent.parent.GetComponent<CCar>().Collision(false);
    }
}
