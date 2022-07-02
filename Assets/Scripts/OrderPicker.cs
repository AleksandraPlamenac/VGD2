using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Experimental.GlobalIllumination;

public class OrderPicker : MonoBehaviour
{

    public Material[] myMaterials = new Material[5];
    public Light spotLight;
    public Material startingColor;
    public TextMeshProUGUI countText;
    private bool changeColor = true;
    private bool beingHandled = false;
    private Material currentMaterial;
    private float orderTimeout = 30f;
    private float orderTimeoutCountDown;


    private void Start()
    {
        StartCoroutine(HandleIt());
        orderTimeoutCountDown = orderTimeout;
    }

    private void FixedUpdate()
    {
        orderTimeoutCountDown -= Time.deltaTime;
        if (changeColor && !beingHandled)
        {
            StartCoroutine(HandleIt());
        }

        HandleSpotLight();
    }

    private void OnOrderTimeout()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
        changeColor = true;
        Debug.Log("FAIL!: OrderTimeout");
        SetScore(-1);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Get the names of the materials. They will come with a (Instance) at the end, so remove it and also trim all empty spaces
        string currentMaterialName = currentMaterial.name.Replace("(Instance)", "").Trim();
        string otherMaterialName = other.gameObject.GetComponent<MeshRenderer>().material.name.Replace("(Instance)", "")
            .Trim();
        if (currentMaterialName.Contains(otherMaterialName))
        {
            Debug.Log("PASS! CurrentName: " + currentMaterialName + " otherName: " + otherMaterialName);
            gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
            SetScore(1);
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
            Debug.Log("FAIL! CurrentName: " + currentMaterialName + " otherName: " + otherMaterialName);
            SetScore(-1);
        }

        // CancelInvoke OnOrderTimeout
        CancelInvoke("OnOrderTimeout");
        changeColor = true;
    }

    private IEnumerator HandleIt()
    {
        beingHandled = true;
        // process pre-yield
        yield return new WaitForSeconds(2.0f);
        Material materialtoSet = myMaterials[Random.Range(0, myMaterials.Length - 1)];
        
        // Set Invoke OnOrderTimeout
        Invoke("OnOrderTimeout", orderTimeout);
        currentMaterial = materialtoSet;
        gameObject.GetComponent<MeshRenderer>().material.color = materialtoSet.color;
        changeColor = false;
        // process post-yield
        orderTimeoutCountDown = orderTimeout;
        beingHandled = false;
    }

    private void SetScore(int points)
    {
        int newScore = int.Parse(countText.text) + points;
        countText.text = newScore.ToString();
    }

    private void HandleSpotLight()
    {
        if (orderTimeoutCountDown <= 0)
        {
            spotLight.enabled = false;
        }
        else
        {
            spotLight.enabled = true;
            if (orderTimeoutCountDown < orderTimeout * .15)
            {
                spotLight.color = new Color32(255, 0, 0, 0xFF);
            }
            else if (orderTimeoutCountDown < orderTimeout * .4)
            {
                spotLight.color = new Color32(253, 208, 0, 0xFF);
            }
            else
            {
                spotLight.color = new Color32(0, 255, 0, 0xFF);
            }
        }
    }
}