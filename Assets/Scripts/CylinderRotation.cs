using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CylinderRotation : MonoBehaviour
{
    // Start is called before the first frame update
    
    public ConveyorBeltScript cbs;
    public float delta;
    public float temp;
    // Update is called once per frame

    void  Awake()
    {

        temp = cbs.xSpeed + cbs.zSpeed;//using both values to cover for any combaination of the two
        delta = temp / 2;
    }

    void Update()
    {
        temp = cbs.xSpeed + cbs.zSpeed;
        delta = temp / 2;
        this.transform.Rotate(new Vector3(0, delta, 0));
        
    }
}
