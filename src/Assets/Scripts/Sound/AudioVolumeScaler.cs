using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVolumeScaler : MonoBehaviour
{
    // âπó Ç…âûÇ∂ÇƒÉXÉPÅ[ÉãÇïœâªÇ≥ÇπÇÈ
    [SerializeField] private AudioCulcurator AC;
    [SerializeField] private float changeVolume = 1.0f;

    private Transform targetObject;
    private Vector3 initialScale;

    void Start()
    {
        targetObject = this.transform;
        initialScale = targetObject.localScale;
    }

    void Update()
    {
        float vol = AC.GetOutputData() * changeVolume;
        Vector3 scale = new Vector3(initialScale.x + vol, initialScale.y + vol, initialScale.z + vol);
        targetObject.localScale = scale;
    }
}
