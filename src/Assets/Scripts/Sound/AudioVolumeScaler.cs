using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVolumeScaler : MonoBehaviour
{
    // ‰¹—Ê‚É‰‚¶‚ÄƒXƒP[ƒ‹‚ğ•Ï‰»‚³‚¹‚é
    [SerializeField] private AudioCulcurator AC;

    void Start()
    {
        
    }

    void Update()
    {
        Debug.Log(AC.GetCurrentData().ToString());
    }
}
