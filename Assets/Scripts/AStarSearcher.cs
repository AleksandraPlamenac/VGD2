using System;
using UnityEngine;
using UnityEngine.AI;

public class AStarSearcher : MonoBehaviour
{
    public GameObject FindObjectToPickup(String acceptedTag)
    {
        var closestDistance = Mathf.Infinity;
        GameObject target = null;

        foreach (var box in GameObject.FindGameObjectsWithTag(Common.RESOURCE_BOX_TAG))
        {
            NavMesh.SamplePosition(box.transform.position, out var hit, 5f, -1);

            if (hit.distance < closestDistance)
            {
                closestDistance = hit.distance;
                target = box;
            }
        }

        return target;
    }
}