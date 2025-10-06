using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Audio", menuName = "ScriptableObjects/AudioScriptable")]
public class AudioScriptable : ScriptableObject
{

    [SerializeField] AudioClip ExplodeSounds;
    [SerializeField] AudioClip BombHitSounds;
    [SerializeField] AudioClip DestroyObstacleSounds;

    public AudioClip _ExplodeSounds
    {
        get { return ExplodeSounds; }
    }
    public AudioClip _BombHitSounds
    {
        get { return BombHitSounds; }
    }
    public AudioClip _DestroyObstacleSounds
    {
        get { return DestroyObstacleSounds; }
    }

}
