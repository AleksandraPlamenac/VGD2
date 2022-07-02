using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftOperator : MonoBehaviour
{
    // Start is called before the first frame update
    public bool shipmentsOnPlatform;
    public bool animateLift;
    public Animator an;

    public float timer;
    public float maxTime = 4;
    void Start()
    {
        shipmentsOnPlatform = false;
        an.SetBool("GoUp", false);
        animateLift = false;
        timer = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (shipmentsOnPlatform == true)
        {
            if (timer > 0)
            {
                timer = timer - Time.deltaTime;
            }
            else
            {
                an.SetBool("GoUp", true);
                animateLift = true;
                timer = maxTime;
            }
        }  
        else
        {
            if (animateLift == false)
            {
                if (timer < maxTime)
                {
                    timer = timer + Time.deltaTime;
                }
            }
            else
            {
                an.SetBool("GoUp", false);//should have already been captured in the previous update by the animator
                animateLift = false;
            }
        }

    }

    private void OnTriggerEnter(Collider enter)
    {

        if (enter.CompareTag("ResourceBox"))
        {
            shipmentsOnPlatform = true;
            enter.gameObject.tag = "Filthy";
        }
    }
    private void OnTriggerStay(Collider stay)
    {
        if (stay.CompareTag("Filthy"))
        {
            shipmentsOnPlatform = true;
        }

    }
    private void OnTriggerExit(Collider exit)
    {
        if (exit.CompareTag("Filthy"))
        {
            shipmentsOnPlatform = false;
        }
        
    }
}
