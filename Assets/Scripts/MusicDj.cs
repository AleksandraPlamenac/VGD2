using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script idea is that it will pick the music track based on the game situation and scene, for the alpha we start 1 music track

public class MusicDj : MonoBehaviour
{
    // Start is called before the first frame update
    void awake()
    {
        EventManager.TriggerEvent<MusicEvent, int>(1);
    }

    // Update is called once per frame

}
