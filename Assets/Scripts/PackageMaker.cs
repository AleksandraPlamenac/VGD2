using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageMaker : MonoBehaviour
{
    public float startTimer = 0f; //used for the time between packages instantiation
    public float delay = 10f; //used for the time between packages instantiation
    public bool startImmediately = true;
    public GameObject[] Packages;
    private int length;
    private int index=0;

    // Start is called before the first frame update
    void Start()
    {
        length = Packages.Length;
        
        //if the array is not empty start making pakages
        if (length != 0 && startImmediately)
        {
            InvokeRepeating("Make", startTimer, delay);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Make()
    {
        if (IsDone())
        {
            return;
        }

        var boxInstance = Instantiate(Packages[index], transform.position, transform.rotation);
        EventManager.TriggerEvent<BoxIsBorn, Vector3>(transform.position);

        // boxInstance.layer = Common.BOX_LAYER_ID;
        boxInstance.tag = Common.RESOURCE_BOX_TAG;
        boxInstance.name = name + " - " + boxInstance.name;
        index++;        
    }
    
    public bool IsDone() 
    {
        if (index == length - 1)
        {
            return true;         // need to find a cleaner way to deal stop the invoke as cancel invoke works on all the repeating invokes but this works for the prototype
        }

        return false;
    }

    public void Reset()
    {
        index = 0;
    }
}
