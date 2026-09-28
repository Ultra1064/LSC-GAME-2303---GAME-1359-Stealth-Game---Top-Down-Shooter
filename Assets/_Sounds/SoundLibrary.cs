using UnityEngine;
using System;

[System.Serializable]
public struct SoundEffectsScript
{
    public string GroupID;
    public AudioClip[] clips;
}
public class SoundLibrary : MonoBehaviour
{
    public SoundEffectsScript[] soundEffects;

    public AudioClip GetClipFromName(string name)
    {
        foreach(var soundEffect in soundEffects)
        {
            if (soundEffect.GroupID == name)
                return soundEffect.clips[UnityEngine.Random.Range(0, soundEffect.clips.Length)];
        }
        return null;
    }
}
