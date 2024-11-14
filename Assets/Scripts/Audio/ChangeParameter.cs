using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeParameter : MonoBehaviour
{
    public string parameterName;
    public float parameterValue;
    public bool isGlobal;
    public string eventName;
    // Start is called before the first frame update
    void Start()
    {
        if (!isGlobal) 
        {
            foreach (EventInstance eventInstance in SoundManager.Instance.eventInstances)
            {
                EventDescription description;
                string result;
                eventInstance.getDescription(out description);
                description.getPath(out result);
                UnityEngine.Debug.Log(result);
                if (result.EndsWith(eventName))
                {
                    SoundManager.Instance.SetParameter(eventInstance, parameterName, parameterValue);
                }
            }
        }
        else
        {
            SoundManager.Instance.SetGlobalParameter(parameterName, parameterValue);
        }
        
    }
}
