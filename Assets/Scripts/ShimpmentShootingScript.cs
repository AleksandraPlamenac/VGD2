using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShimpmentShootingScript : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider enter)
    {
        if (enter.CompareTag("ForceZone"))
        {
            Debug.Log("c forcezone");
            if (this.CompareTag("Filthy"))
            {
                float x, y, z;
                x = Random.Range(-5, 1);
                y = Random.Range(10, 30);
                z = Random.Range(-1, 5);
                rb.AddForce(new Vector3(x, y,z),ForceMode.Impulse);
                
                Debug.Log("Filthy");
            }
        }
    }

}



