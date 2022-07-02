using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//based on the course CrateCollisionReporter
public class BoxCollisionReporter : MonoBehaviour
{
    public bool IsGrounded => _groundContactCount > 0;

    void OnCollisionEnter(Collision c)
    {

        if (c.impulse.magnitude > 0.25f)
        {
            //we'll just use the first contact point for simplicity
            EventManager.TriggerEvent<ShipmentBoxCollisionEvent, Vector3, float>(c.contacts[0].point, c.impulse.magnitude);
        }

        if (c.transform.gameObject.CompareTag("ground"))
        {
            ++_groundContactCount;
        }
						
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.transform.gameObject.CompareTag("ground"))
        {
            --_groundContactCount;

        }
    }

    private int _groundContactCount;
}
