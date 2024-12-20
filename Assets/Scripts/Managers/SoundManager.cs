using FMOD;
using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private EventInstance ambienceEventInstance;
    public EventInstance musicEventInstance;
    public EventInstance fishingEventInstance;
    public EventInstance footstepsEventInstance;
    public EventInstance dialogueEventInstance;
    public EventInstance laserEventInstance;

    public List<EventInstance> eventInstances;

    // private singleton instance
    private static SoundManager _instance;

    // public accessor of instance
    public static SoundManager Instance
    {
        get
        {
            // setup SoundManager as a singleton class
            if (_instance == null)
            {
                // create new game manager object
                GameObject newManager = new();
                newManager.name = "Sound Manager";
                newManager.AddComponent<SoundManager>();
                DontDestroyOnLoad(newManager);
                _instance = newManager.GetComponent<SoundManager>();
                
                // some setup for sound instances list
                _instance.eventInstances = new List<EventInstance>();
            }
            // return new/existing instance
            return _instance;
        }
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


    public void SetGlobalParameter(string name, float value)
    {
        RuntimeManager.StudioSystem.setParameterByName(name, value);
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

    public void InitializeDialogue(EventReference dialogueEventReference)
    {
        dialogueEventInstance = CreateInstance(dialogueEventReference);
        dialogueEventInstance.start();
    }

    public void InitializeLaser(EventReference laserEventReference)
    {
        laserEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        laserEventInstance.release();
        laserEventInstance = CreateInstance(laserEventReference);
        laserEventInstance.start();
    }

    public void StopDialogue()
    {
        dialogueEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
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
    public void CleanButSpareMusic()
    {/*
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.getDescription(out EventDescription description);
            UnityEngine.Debug.Log(description);
            if (eventInstance != musicEventInstance) 
            {
                eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                if (complete)
                {
                    eventInstance.release();
                }
            }
        }*/
    }
    public void CleanButSpare(string spare, bool complete)
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.getDescription(out EventDescription description);
            description.getPath(out string result);
            //UnityEngine.Debug.Log(result);
            if (result != null && !result.EndsWith(spare)) 
            {
                eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                eventInstance.release();
            }
        }
    }

    public void PauseAll(bool pause)
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.setPaused(pause);
        }
    }

    public void Pause(EventInstance eventInstance, bool pause)
    {
        eventInstance.setPaused(pause);
    }

    public void CheckEventInstances()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.getDescription(out EventDescription description);
            description.getPath(out string result);
            //UnityEngine.Debug.Log(result);
        }
    }

    }
