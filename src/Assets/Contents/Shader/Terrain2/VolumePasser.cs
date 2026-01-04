using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumePasser : MonoBehaviour
{
    [SerializeField] private Material terrain_Mat;
    [SerializeField] private AudioCulcurator culcurator;

    private float volume;
    private string vol = "_Volume";

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        volume = culcurator.GetOutputData();
        // Debug.Log(culcurator.GetOutputData());
        terrain_Mat.SetFloat(vol, volume);
        // Debug.Log("Volume: " + volume);
    }
}
