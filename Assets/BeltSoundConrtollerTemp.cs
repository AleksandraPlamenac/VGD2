using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltSoundConrtollerTemp : MonoBehaviour
{
    // Start is called before the first frame update

    public ConveyorBeltScript cbs;
    private AudioSource beltsound;
    public float temp;
    // Update is called once per frame

    void Awake()
    {

        
        beltsound = this.GetComponent<AudioSource>();
        temp = cbs.xSpeed + cbs.zSpeed;
        beltsound.pitch= temp ;
        
    }

    void Update()
    {
        temp = cbs.xSpeed + cbs.zSpeed;
        beltsound.pitch = temp;

    }
}
