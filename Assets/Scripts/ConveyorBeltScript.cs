using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script was taken from https://www.youtube.com/watch?v=hC1QZ0h4oco 
public class ConveyorBeltScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float xSpeed = 0;
    public float zSpeed = 0;
    Rigidbody rBody;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = rBody.position;
        rBody.position += new Vector3(xSpeed,0,zSpeed)* Time.fixedDeltaTime;
        rBody.MovePosition(pos);
    }
}
