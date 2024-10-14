using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private EventInstance ambienceEventInstance;
    private EventInstance musicEventInstance;
    public EventInstance fishingEventInstance;
    public EventInstance footstepsEventInstance;

    public List<EventInstance> eventInstances;

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene.");
        }
        Instance = this;

        eventInstances = new List<EventInstance>();
        
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }
    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.start();
    }

    public void InitializeFootsteps(EventReference footstepsEventReference)
    {
        footstepsEventInstance = CreateInstance(footstepsEventReference);
        footstepsEventInstance.start();
    }

    public void InitializeFishing(EventReference fishingEventReference)
    {
        fishingEventInstance = CreateInstance(fishingEventReference);
        fishingEventInstance.start();
    }

    public void SetParameter(EventInstance sound, string name, float value)
    {
        sound.setParameterByName(name, value);
    }

    private void InitializeAmbience(EventReference ambienceEventReference)
    {
        ambienceEventInstance = CreateInstance(ambienceEventReference);
        ambienceEventInstance.start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CleanUp()
    {
        // stop and release any created instances
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
        /* stop all of the event emitters, because if we don't they may hang around in other scenes
        foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
        */
    }
}
