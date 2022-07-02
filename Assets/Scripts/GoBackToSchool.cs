using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBackToSchool : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = new Vector3(0, 3, 0);
    }
}
