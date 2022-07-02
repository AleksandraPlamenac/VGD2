using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class DebugInputController : MonoBehaviour
{
    private PackageMaker packageMaker;

    // Start is called before the first frame update
    void Start()
    {
        packageMaker = GetComponent<PackageMaker>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            packageMaker.transform.position = new Vector3(Random.Range(-15f, 15f), 20, Random.Range(-15f, 15f));
            packageMaker.transform.eulerAngles = new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
            packageMaker.Make();
            Debug.Log("[DEBUG] making a new object");
            
            if (packageMaker.IsDone())
            {
                packageMaker.Reset();
            }
        }
    }
}
