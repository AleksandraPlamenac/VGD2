using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioEventManager : MonoBehaviour
{

    public EventSound3D eventSound3DPrefab;

    public AudioClip[] tbd = null;//this is should be used for an object with multiple sounds
    public AudioClip[] backgroundMusic = null;//using array for now because we can have multiple background music
    public AudioClip foe;
    public AudioClip friend;
    public AudioClip shipmentBoxCollision;
    public AudioClip liftCar;
    public AudioClip boxIsBorn;
    public AudioClip boxIsKilled;

    private UnityAction<Vector3, float> shipmentBoxCollisionEventListener;//this include float to determine the strength of the sound based on the impulse
    private UnityAction<int> musicEventListener;//to be used for with the different tracks of background music
    private UnityAction<GameObject> deathEventListener;// to be used with boxes being killed by the NPC
    private UnityAction<Vector3> liftEventListener;
    private UnityAction<Vector3> boxIsBornListener;
    //to be decided if we are going to use them, shipment box actions
    private UnityAction<Vector3> foeEventListener;
    private UnityAction<Vector3> friendEventListener;




    void Awake()
    {

        //start listening to the different events


        musicEventListener = new UnityAction<int>(musicEventHandler);
        shipmentBoxCollisionEventListener = new UnityAction<Vector3, float>(shipmentBoxCollisionEventHandler);
        deathEventListener = new UnityAction<GameObject>(deathEventHandler);

        liftEventListener = new UnityAction<Vector3>(liftEventHandler);
        boxIsBornListener = new UnityAction<Vector3>(boxIsBornEventHandler);


        //shipment box audio TBD
        foeEventListener = new UnityAction<Vector3>(foeEventHandler);
        friendEventListener = new UnityAction<Vector3>(friendEventHandler);



    }


    void OnEnable()
    {

        EventManager.StartListening<ShipmentBoxCollisionEvent, Vector3, float>(shipmentBoxCollisionEventListener);
        EventManager.StartListening<MusicEvent, int>(musicEventListener);
        EventManager.StartListening<DeathEvent, GameObject>(deathEventListener);
        EventManager.StartListening<LiftEvent, Vector3>(liftEventListener);
        EventManager.StartListening<BoxIsBorn, Vector3>(boxIsBornListener);

        //shipment box
        EventManager.StartListening<FoeEvent, Vector3>(foeEventListener);
        EventManager.StartListening<FriendEvent, Vector3>(friendEventListener);

    }

    void OnDisable()
    {

        EventManager.StopListening<ShipmentBoxCollisionEvent, Vector3, float>(shipmentBoxCollisionEventListener);
        EventManager.StopListening<MusicEvent, int>(musicEventListener);
        EventManager.StopListening<DeathEvent, GameObject>(deathEventListener);
        EventManager.StopListening<LiftEvent, Vector3>(liftEventListener);
        EventManager.StopListening<BoxIsBorn, Vector3>(boxIsBornListener);

        //shipment box
        EventManager.StopListening<FoeEvent, Vector3>(foeEventListener);
        EventManager.StopListening<FriendEvent, Vector3>(friendEventListener);
    }





    void shipmentBoxCollisionEventHandler(Vector3 worldPos, float impactForce)
    {
        //AudioSource.PlayClipAtPoint(this.boxAudio, worldPos);

        const float halfSpeedRange = 0.2f;

        EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

        snd.audioSrc.clip = this.shipmentBoxCollision;

        snd.audioSrc.pitch = Random.Range(1f - halfSpeedRange, 1f + halfSpeedRange);

        snd.audioSrc.minDistance = Mathf.Lerp(1f, 8f, impactForce / 200f);
        snd.audioSrc.maxDistance = 100f;

        snd.audioSrc.Play();
    }



    private void musicEventHandler(int track)
    {
        throw new System.NotImplementedException();
    }


    private void liftEventHandler(Vector3 worldPos)
    {
        //AudioSource.PlayClipAtPoint(this.explosionAudio, worldPos, 1f);

        if (eventSound3DPrefab)
        {

            EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

            snd.audioSrc.clip = this.liftCar;

            snd.audioSrc.minDistance = 10f;
            snd.audioSrc.maxDistance = 500f;

            snd.audioSrc.Play();
        }
    }

    private void boxIsBornEventHandler(Vector3 worldPos)
    {
        //AudioSource.PlayClipAtPoint(this.explosionAudio, worldPos, 1f);

        if (eventSound3DPrefab)
        {

            EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

            snd.audioSrc.clip = this.boxIsBorn;

            snd.audioSrc.minDistance = 10f;
            snd.audioSrc.maxDistance = 500f;

            snd.audioSrc.Play();
        }
    }


    //shipment box methods
    private void friendEventHandler(Vector3 worldPos)
    {
        //AudioSource.PlayClipAtPoint(this.explosionAudio, worldPos, 1f);

        if (eventSound3DPrefab)
        {

            EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

            snd.audioSrc.clip = this.friend;

            snd.audioSrc.minDistance = 10f;
            snd.audioSrc.maxDistance = 500f;

            snd.audioSrc.Play();
        }
    }

    private void foeEventHandler(Vector3 worldPos)
    {
        //AudioSource.PlayClipAtPoint(this.explosionAudio, worldPos, 1f);

        if (eventSound3DPrefab)
        {

            EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

            snd.audioSrc.clip = this.foe;

            snd.audioSrc.minDistance = 10f;
            snd.audioSrc.maxDistance = 500f;

            snd.audioSrc.Play();
        }
    }





    void deathEventHandler(GameObject go)
    {
        //AudioSource.PlayClipAtPoint(this.explosionAudio, worldPos, 1f);

        if (eventSound3DPrefab)
        {

            EventSound3D snd = Instantiate(eventSound3DPrefab, go.transform);

            snd.audioSrc.clip = this.boxIsKilled;

            snd.audioSrc.minDistance = 5f;
            snd.audioSrc.maxDistance = 100f;

            snd.audioSrc.Play();
        }
    }
}


