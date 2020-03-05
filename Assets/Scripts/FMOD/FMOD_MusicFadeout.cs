using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMOD_MusicFadeout : MonoBehaviour
{

    [FMODUnity.EventRef] public string musicEvent;
    FMOD.Studio.EventInstance musicEventInstance;

    private void Start()
    {
        musicEventInstance = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        musicEventInstance.start();
    }

    private void OnDestroy()
    {
        musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }


}
