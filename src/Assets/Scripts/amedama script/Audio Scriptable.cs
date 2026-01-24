using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Audio", menuName = "ScriptableObjects/AudioScriptable")]
public class AudioScriptable : ScriptableObject
{

    [SerializeField] AudioClip ExplodeEnemySounds;
    [SerializeField] AudioClip CoinHitSounds;
    [SerializeField] AudioClip DestroyObstacleSounds;
    [SerializeField] AudioClip HitEnemySounds;
    [SerializeField] AudioClip HitCoinSounds;

    public AudioClip _ExplodeEnemySounds
    {
        get { return ExplodeEnemySounds; }
    }
    public AudioClip _CoinHitSounds
    {
        get { return CoinHitSounds; }
    }
    public AudioClip _DestroyObstacleSounds
    {
        get { return DestroyObstacleSounds; }
    }
    public AudioClip _HitEnemySounds
    {
        get { return HitEnemySounds; }
    }
    public AudioClip _HitCoinSounds
    {
        get { return HitCoinSounds; }
    }


}
