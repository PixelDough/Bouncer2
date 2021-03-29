using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_OldComputer : Interactable
{
    public SpriteRenderer screenOn;

    [FMODUnity.EventRef]
    public string tvSoundEvent;

    public FMODUnity.StudioEventEmitter tvSoundEmitter;

    FMOD.Studio.EventInstance tvSoundEventInstance;

    protected override void Start()
    {
        base.Start();

        FMODUnity.RuntimeManager.CreateInstance(tvSoundEvent).set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(screenOn.transform.position));
        //tvSoundEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(screenOn.transform.position));
        
        //tvSoundEventInstance.start();
    }

    protected override void Interact()
    {
        screenOn.enabled = !screenOn.enabled;

        //tvSoundEventInstance.setParameterByName("Finished", screenOn ? 0 : 1);
        tvSoundEmitter.EventInstance.setParameterByName(name: "Finished", screenOn.enabled ? 0 : 1);
    }
}
