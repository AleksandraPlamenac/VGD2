using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamAndAudioAtTop : MonoBehaviour
{
    public void steam()
    {
        Debug.Log("particle steam");

    }
    public void LiftSound()
    {
        EventManager.TriggerEvent<LiftEvent, Vector3>(transform.position);

    }
}
