using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float smoother = 5f;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        Transform target = player.transform;
        Vector3 newPos = target.position + offset;
        // Vector3 smPos = Vector3.Lerp (transform.position, newPos, smoother * Time.deltaTime);
        
        transform.position = newPos;
        transform.LookAt (target);
    }
}